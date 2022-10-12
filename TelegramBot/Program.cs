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
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]//шаблон клавиатуры
                                                      {
                                                            new KeyboardButton[] { "phote", "sticker" },
                                                            new KeyboardButton[] { "poll", "contact" },
                                                            new KeyboardButton[] { "venue" }
                                                      })
                                                      {
                                                            ResizeKeyboard = true//подгонка размера клавиатуры по размер телефона
                                                      };
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]//шаблон клавиатуры который не отправляет запрос в чат
                                                      {
                                                            new []
                                                      {
                                                                InlineKeyboardButton.WithCallbackData(text: "phote", callbackData: "phote"),
                                                                InlineKeyboardButton.WithCallbackData(text: "sticker", callbackData: "sticker"),
                                                      },
                                                            new []
                                                      {
                                                                InlineKeyboardButton.WithCallbackData(text: "poll", callbackData: "poll"),
                                                                InlineKeyboardButton.WithCallbackData(text: "contact", callbackData: "contact"),
                                                      },
                                                      });
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                switch (message.Text.ToLower())
                {
                    case "/start":
                        await botClient.SendTextMessageAsync( //отправка текстового сообщения
                            chatId: message.Chat, //ссылка на чат для отправки сообщения
                            text: $"Ку", //текст сообщения
                            replyMarkup: replyKeyboardMarkup);//установить клавиатуру
                        break;
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
                        await botClient.SendStickerAsync(//отправка стикера
                            chatId: message.Chat,//ссылка на чат для отправки сообщения
                            sticker: "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-fred.webp",//ссылка на стикер
                            cancellationToken: cancellationToken);//прерывание потока
                        break;
                    case "poll":
                        await botClient.SendPollAsync(//оптавка опроса
                            chatId: message.Chat,
                            question: "1",//текст опроса
                            options: new[]//варианты опроса
                            {
                                "2",
                                "3"
                            },
                            cancellationToken: cancellationToken);
                        break;
                    case "contact":
                        await botClient.SendContactAsync(//отправка контакта
                            chatId: message.Chat,
                            phoneNumber: "+1234567890",//номер тел
                            firstName: "Han han",//имя
                            vCard: "BEGIN:VCARD\n" +
                                   "VERSION:3.0\n" +
                                   "N:Solo;Han\n" +
                                   "ORG:тест\n" +
                                   "TEL;TYPE=voice,work,pref:+1234567890\n" +
                                   "EMAIL:hansolo@mfalcon.com\n" +
                                   "END:VCARD",
                            cancellationToken: cancellationToken);
                        break;
                    case "venue":
                        await botClient.SendVenueAsync(
                            chatId: message.Chat,
                            latitude: 50.0840172f,
                            longitude: 14.418288f,
                            title: "Man Hanging out",
                            address: "Husova, 110 00 Staré Město, Czechia",
                            cancellationToken: cancellationToken);
                        break;
                    default:
                        await botClient.SendTextMessageAsync( //отправка текстового сообщения
                            chatId: message.Chat, //ссылка на чат для отправки сообщения
                            text: $"Ваше сообщение:\n{message.Text}", //текст сообщения
                            replyToMessageId: update.Message.MessageId, // ссылка на сообщение пользователя
                            replyMarkup: new InlineKeyboardMarkup(new[]{
                                new[]
                                {
                                    InlineKeyboardButton.WithUrl(//кнопка перехода по ссылке
                                    "переход по ссылке",//текст кнопки
                                    "https://core.telegram.org/bots/api#sendmessage"),//ссылка
                                    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(//ответить в этот чат
                                    "Check sendMessage method",
                                    "2")},
                                new[]
                                {
                                    InlineKeyboardButton.WithSwitchInlineQuery(//ответить в другой чат
                                    "отправить в другой чат",
                                    "отправить в другой чат")}}),
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
