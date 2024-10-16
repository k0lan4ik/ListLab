using Telegram.Bot.Types.ReplyMarkups;

namespace ListLab.client
{
    public class InlineKeyboards
    {   
        public static async Task<InlineKeyboardMarkup> DateInlineKeyboard(){
            var keyboard = new List<InlineKeyboardButton[]>();
            // получаем массив доступных дат
            var massDate = new string[]{"15.10.2024","20.10.2024"};
            for(int i = 0; i < massDate.Length; i++){
                keyboard.Add(new InlineKeyboardButton[]{InlineKeyboardButton.WithCallbackData(massDate[i],"date:" + massDate[i])});
            }
            return new InlineKeyboardMarkup(keyboard);    
        }
        public static async Task<InlineKeyboardMarkup> DateLineInlineKeyboard(){
            var keyboard = new List<InlineKeyboardButton[]>();
            // получаем массив доступных дат
            var massDate = new string[]{"15.10.2024","20.10.2024"};
            for(int i = 0; i < massDate.Length; i++){
                keyboard.Add(new InlineKeyboardButton[]{InlineKeyboardButton.WithCallbackData(massDate[i],"dateline:" + massDate[i])});
            }
            return new InlineKeyboardMarkup(keyboard);    
        }
        public static async Task<InlineKeyboardMarkup> LabInlineKeyboard(){
            var keyboard = new List<InlineKeyboardButton[]>();
            // получаем массив доступных лаб
            var massLab = new string[]{"Лабораторная 1","Лабораторная 2"};
            for(int i = 0; i < massLab.Length; i++){
                keyboard.Add(new InlineKeyboardButton[]{InlineKeyboardButton.WithCallbackData(massLab[i],"lab:" + massLab[i])});
            }
            return new InlineKeyboardMarkup(keyboard);    
        }  
    }
}