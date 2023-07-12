using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TodolistApi.Service.Repository;
using TodolistApi.Domain.Models;
using System.Text;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

public class TelegramBot : BackgroundService
{
    private readonly IRepository<TodoItem> _repository;
    private readonly IConfiguration _configuration;
    private readonly Regex _regex = new(@"^/(?<command>\w+)(?:\s+(?<parameter>\w+))*$");

    public TelegramBot(IRepository<TodoItem> repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var secret = 
            _configuration.GetValue<string>("TelegramBotOptions:Token") 
            ?? throw new ArgumentNullException("secret");

        var botClient = new TelegramBotClient(secret);

        using CancellationTokenSource cts = new();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        var commands = new BotCommand[]
        {
            new () {Command = "getbyid", Description = "Get an item by Id"},
            new () {Command = "getall", Description = "Get all items"},
            new () {Command = "create", Description = "Create an item"},
            new () {Command = "deletebyid", Description = "Delete an item by Id"},
            new () {Command = "update", Description = "Update an item by Id"}
        };
        await botClient.SetMyCommandsAsync(commands, BotCommandScope.AllPrivateChats(), cancellationToken: cts.Token);

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        // Send cancellation request to stop bot
        cts.Cancel();
    }


    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        var result = new StringBuilder();

        if(TryGetCommandAndParameters(messageText, out var command, out var parameters) is false)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Invalid command\n",
                cancellationToken: cancellationToken);
            return;
        }

        switch (command)
        {
            case "getbyid":
                GetById(parameters, result);
                break;
            case "getall":
                GetAll(result);
                break;
            case "create":
                await AddItem(parameters, result);
                break;
            case "deletebyid":
                await DeleteById(parameters, result);
                break;
            case "update":
                await this.Update(parameters, result);
                break;
        }

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        // Echo received message text
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: result.ToString(),
            cancellationToken: cancellationToken);
    }

    private bool TryGetCommandAndParameters(
        string messageText,
        [NotNullWhen(true)] out string? command,
        [NotNullWhen(true)] out IList<string>? parameters)
    {
        var match = _regex.Match(messageText);
        if (!match.Success)
        {
            command = null;
            parameters = null;
            return false;
        }

        command = match.Groups["command"].Value;
        parameters = match.Groups["parameter"].Captures.Select(s => s.Value).ToList();
        return true;
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    private void GetAll(StringBuilder result)
    {
        var list = _repository.Get()
            .Take(5000)
            .ToArray();

        foreach (var item in list)
        {
            Console.WriteLine(item + "\n");
            result.Append(item + "\n");
        }

        Console.WriteLine("User chose get all");
    }

    private void GetById(IList<string> parameters, StringBuilder result)
    {
        if (parameters.Count() != 1)
        {
            result.Append("invalid command");
            return;
        }

        var firstParameter = parameters.FirstOrDefault();
        if (int.TryParse(firstParameter, CultureInfo.InvariantCulture, out var id) is false)
        {
            result.Append("invalid format");
            return;
        }

        var item = _repository
            .Get()
            .FirstOrDefault(x => x.Id == id);

        result.Append(item != null ? item.ToString() : "This user id does not exist");

        Console.WriteLine($"User get this item - {result} by {id} id");
    }

    private async Task AddItem(IList<string> parameters, StringBuilder result)
    {
        if (parameters.Any() is false)
        {
            result.Append("invalid command");
            return;
        }

        var todoItem = new TodoItem() { Name = string.Join(' ', parameters) };
        _repository.Insert(todoItem);
        await _repository.SaveChangesAsync();
        result.Append($"{todoItem.Name} was added. Id: {todoItem.Id}");
        Console.WriteLine("User chose to create");
    }

    private async Task DeleteById(IList<string> parameters, StringBuilder result)
    {
        if (parameters.Count() != 1)
        {
            result.Append("invalid command");
            return;
        }

        var firstParameter = parameters.FirstOrDefault();
        if (int.TryParse(firstParameter, CultureInfo.InvariantCulture, out var id) is false)
        {
            result.Append("invalid format");
            return;
        }

        var item = _repository
            .Get()
            .FirstOrDefault(x => x.Id == id);
        if (item is not null)
        {
            _repository.Delete(item);
            await _repository.SaveChangesAsync();
        }

        result.Append(item != null ? $"TodoItem was deleted: {item}" : "This user id does not exist");
        Console.WriteLine($"User delete this item - {result} by {id} id");
    }

    private async Task Update(IList<string> parameters, StringBuilder result)
    {
        if (parameters.Count() < 2)
        {
            result.Append("invalid command");
            return;
        }

        var firstParameter = parameters.FirstOrDefault();
        if (int.TryParse(firstParameter, CultureInfo.InvariantCulture, out var id) is false)
        {
            result.Append("invalid format");
            return;
        }

        var item = _repository
            .Get()
            .FirstOrDefault(x => x.Id == id);

        if (item is not null)
        {
            item.Name = string.Join(' ', parameters.Skip(1));
            _repository.Update(item);
            await _repository.SaveChangesAsync();
        }

        result.Append(item != null ? $"TodoItem was updated: {item}" : "This user id does not exist");
        Console.WriteLine($"User chose update {parameters.FirstOrDefault()} id");
    }
}
