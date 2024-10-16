using ListLab.client;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ListLab;

class Program
{
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient _botClient;
    
    // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
    private static ReceiverOptions _receiverOptions;
    
    static async Task Main()
    {
        
        _botClient = new TelegramBotClient("7414554042:AAEHzhLiuxU80EUZ7VohbzMT3Vrwfyp-29g"); // Присваиваем нашейd переменной значение, в параметре передаем Token, полученный от BotFather
        _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
        {
            AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                UpdateType.CallbackQuery // Inline кнопки
            },
            // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
            // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
            ThrowPendingUpdates = true, 
        };
        
        using var cts = new CancellationTokenSource();
        
        // UpdateHander - обработчик приходящих Update`ов
        // ErrorHandler - обработчик ошибок, связанных с Bot API
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота
        
        var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");
        
        await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
    }

    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
        try
        {
            // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    // Эта переменная будет содержать в себе все связанное с сообщениями
                    var message = update.Message;

                    // From - это от кого пришло сообщение (или любой другой Update)
                    var user = message.From;
                    
                    // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                    Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                    // Chat - содержит всю информацию о чате
                    var chat = message.Chat;

                    // Добавляем проверку на тип Message
                    switch (message.Type)
                    {
                        // Тут понятно, текстовый тип
                        case MessageType.Text:
                        {
                            // тут обрабатываем команду /start, остальные аналогично
                            if (message.Text == "/start")
                            {
                                await Registration.Start(_botClient, message);
                            }
                            if(message.Text == "Записаться на лабу"){
                                if(await Registration.IsUserRegister(user)){
                                    await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Выберите дату для записи", replyMarkup: await InlineKeyboards.DateInlineKeyboard());
                                    return;
                                }
                                await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Вы не зарегистрированны напишите /start");
                            }

                            if(message.Text == "Посмотреть очередь"){
                                await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Выберите дату очереди", replyMarkup: await InlineKeyboards.DateLineInlineKeyboard());
                                    
                            }

                            if (message.ReplyToMessage.Text == "Для регистации введите своё имя и фамилию")
                            {
                                await Registration.Register(_botClient, message);
                            }

                            return;
                        }
                        case MessageType.Document:
                        {
                            return;
                        }
                        // Добавил default , чтобы показать вам разницу типов Message
                        default:
                        {
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Используй только текст!");
                            return;
                        }
                    }
                    
                    return;
                }

                case UpdateType.CallbackQuery:
                {
                    // Переменная, которая будет содержать в себе всю информацию о кнопке, которую нажали
                    var callbackQuery = update.CallbackQuery;
                    
                    // Аналогично и с Message мы можем получить информацию о чате, о пользователе и т.д.
                    var user = callbackQuery.From;
                    var idButton = callbackQuery.Data.Split(':');
                    // Выводим на экран нажатие кнопки
                    Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");
                  
                    // Вот тут нужно уже быть немножко внимательным и не путаться!
                    // Мы пишем не callbackQuery.Chat , а callbackQuery.Message.Chat , так как
                    // кнопка привязана к сообщению, то мы берем информацию от сообщения.
                    var chat = callbackQuery.Message.Chat; 
                    
                    // Добавляем блок switch для проверки кнопок
                    switch (idButton[0])
                    {
                        // Data - это придуманный нами id кнопки, мы его указывали в параметре
                        // callbackData при создании кнопок. У меня это button1, button2 и button3

                        case "date":
                        {
                            // В этом типе клавиатуры обязательно нужно использовать следующий метод
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                            // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку
                            await EntryInLine.WriteDate(callbackQuery.From,idButton[1]);
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Выберите Лабораторную, которую сдаёте:", replyMarkup: await InlineKeyboards.LabInlineKeyboard());
                            return;
                        }
                        
                        case "dateline":
                        {
                            // В этом типе клавиатуры обязательно нужно использовать следующий метод
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                            // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку
                            
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                $"Вот список на {idButton[1]}:");
                            await LineSearch.SendLine(_botClient, chat, idButton[1]);
                            return;
                        }

                        case "lab":
                        {
                            // А здесь мы добавляем наш сообственный текст, который заменит слово "загрузка", когда мы нажмем на кнопку
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Тут может быть ваш текст!");
                        
                            await EntryInLine.WriteLab(callbackQuery.From,idButton[1]);
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                               "Вышлите свой отчёт в ответ на это сообщение",replyMarkup: new ForceReplyMarkup());
                            return;
                        }
                        
                        case "file":
                        {
                            // А тут мы добавили еще showAlert, чтобы отобразить пользователю полноценное окно
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "А это полноэкранный текст!", showAlert: true);
                            ///файл?*???
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                $"Вы нажали на {callbackQuery.Data}");
                            return;
                        }
                    }
                    
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}
