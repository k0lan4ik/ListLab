
using Telegram.Bot.Types.ReplyMarkups;

namespace ListLab.client
{
    public class ReplyKeyboards
    {
        private static ReplyKeyboardMarkup mainReplyKeyboard = new ReplyKeyboardMarkup(new List<KeyboardButton[]>()
        {new KeyboardButton[]{new KeyboardButton("Записаться на лабу"),new KeyboardButton("Посмотреть очередь")}})
        {
           ResizeKeyboard = true,
        }; 
        public static ReplyKeyboardMarkup MainReplyKeyboard{
            get{
                return mainReplyKeyboard;
            }
        }
        private static ReplyKeyboardMarkup adminReplyKeyboard = new ReplyKeyboardMarkup(new List<KeyboardButton[]>()
        {new KeyboardButton[]{new KeyboardButton("Записаться на лабу"),new KeyboardButton("Посмотреть очередь")},
        new KeyboardButton[]{new KeyboardButton("Потвердить записи"),},
        new KeyboardButton[]{new KeyboardButton("Добавить лабораторные"),new KeyboardButton("Добавить даты сдачи")}})
        {
           ResizeKeyboard = true,
        }; 
        public static ReplyKeyboardMarkup AdminReplyKeyboard{
            get{
                return adminReplyKeyboard;
            }
        }
    }
}