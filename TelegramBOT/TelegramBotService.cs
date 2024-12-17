using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBOT.Options;

namespace TelegramBOT;

public class TelegramBotService : BackgroundService
{
    //private token
    private readonly ILogger<TelegramBotService> _logger;
    private readonly TelegramOptions _telegramOptions;
    public TelegramBotService(
        ILogger<TelegramBotService> logger, 
        IOptions<TelegramOptions> telegramOptions)
    {
        _logger = logger;
        _telegramOptions = telegramOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var botClient = new TelegramBotClient(_telegramOptions.Token);

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = [] // receive all update (типы Сообщений на прием UpdateType.Message)
        };

        while (stoppingToken.IsCancellationRequested)
        {
            await botClient.ReceiveAsync(
                HandleUpdateAsync, 
                HandleErrorAsync, 
                receiverOptions, 
                stoppingToken);

            if (_logger.IsEnabled(LogLevel.Information)) 
                _logger.LogInformation($"Worker running at {DateTime.UtcNow}", DateTimeOffset.Now);
        }
        await Task.Delay(1000, stoppingToken);
    }

    private async Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
            $" Telegram Error Api : [{apiRequestException.ErrorCode}]\n" +
            $"{apiRequestException.Message}",
            _ => exception.ToString()
        };
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if(update.Message is not { } message)
            return;
        if(message.Text is not  { } messageText)
            return;

        var chatId = message.Chat.Id;

        Console.WriteLine($"{DateTime.UtcNow}  :  {message.Chat.FirstName ?? "Аноним"}  => |   {message.Text} ");

        Message sendMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "You said: " + message.Text,
            cancellationToken: cancellationToken);
    }
}
