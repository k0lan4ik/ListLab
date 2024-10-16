using System.ComponentModel;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ListLab.client
{
    public class Registration
    {
        
        public static async Task Start (ITelegramBotClient botClient, Message message){
            try{// проверка есть ли этот пользователь
            
             await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Для регистации введите своё имя и фамилию", replyMarkup: new ForceReplyMarkup());
                return;
            }
            catch{
                return;
            }
        }
        public static async Task Register (ITelegramBotClient botClient,  Message message){
            var x = message.Text;
            // релезовать регистрацию
            try{
             await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Регистрация прошла успешно", replyMarkup: await IsUserAdmin(message.From) ? ReplyKeyboards.AdminReplyKeyboard : ReplyKeyboards.MainReplyKeyboard);
                return;
            }
            catch{
                return;
            }
        }

        public static async Task<bool> IsUserRegister(User user){
            // тут проверка на то зарегистрирован ли усер
            return true;
        }

        public static async Task<bool> IsUserAdmin(User user){
            // тут проверка на то админ ли усер
            return false;
        }
 
    }
}