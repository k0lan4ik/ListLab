using Telegram.Bot;
using Telegram.Bot.Types;

namespace ListLab.client
{
    public class LineSearch
    {
        public static async Task SendLine(ITelegramBotClient botClient, Chat chat, string date){
            //ишем эту херь
            var line = new string[]{"Колосун Николай","Арсений Лайтнинг","Стасевич Александр"};
            string endLine = "";
            for(int i = 0; i < line.Length; i++){
                endLine += $"{i+1}. {line[i] }" + '\n';
            }
            await botClient.SendTextMessageAsync(chat.Id,endLine);
        }
    }
}