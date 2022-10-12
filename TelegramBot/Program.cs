using System;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    class Program
    {
        private static string token { get; set; } = "5735115570:AAH0vay6VQM1hwxq6s1eV9kQmthaxHCWi58";//токен бота
        private static TelegramBotClient client = new TelegramBotClient(token);//класс бота-клиента
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)//метод обрабатывать обновление
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                    if (message.Text != null && message.Text.ToLower() == "/start")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Я запустился");
                        return;
                    }
                switch (message.Text)
                {
                    case "phote":
                        await botClient.SendPhotoAsync(//отправка картинки
                            chatId: message.Chat,//ссылка на чат для отправки сообщения
                            photo: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",//ссылка на изображение
                            caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",//описание под картинкой
                            /*
                             * <b></b> жирный
                             * <i></i> курсив
                             * <a href=""></a> ссылка
                            */
                            parseMode: ParseMode.Html,//режим анализа текста html
                            cancellationToken: cancellationToken);//прерывание потока
                        break;
                    case "sticker":
                        Message message1 = await botClient.SendStickerAsync(//отправка стикера
                            chatId: message.Chat,//ссылка на чат для отправки сообщения
                            sticker: "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-fred.webp",//ссылка на стикер
                            cancellationToken: cancellationToken);//прерывание потока
                        break;
                    default:
                        await botClient.SendTextMessageAsync( //отправка текстового сообщения
                            chatId: message.Chat, //ссылка на чат для отправки сообщения
                            text: $"Ваше сообщение:\n{message.Text}", //текст сообщения
                            replyToMessageId: update.Message.MessageId, // ссылка на сообщение пользователя
                            replyMarkup: new InlineKeyboardMarkup(//разметка кнопок на сообщение
                            InlineKeyboardButton.WithUrl(//кнопка
                            "Check sendMessage method",//текст кнопки
                            "https://core.telegram.org/bots/api#sendmessage")),//ссылка кнопки
                            cancellationToken: cancellationToken);//прерывание потока
                        break;
                }
            }
        }
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)//метод обработки ошибки
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            client.StartReceiving(//обработка событий
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}
