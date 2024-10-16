using Telegram.Bot;
using Telegram.Bot.Types;

namespace ListLab.client
{
    public class Registration
    {
        private bool _isRegistration;

        public Registration(bool isRegistration){
            _isRegistration = isRegistration;
        }

        public bool IsRegistration{
            get{
                return _isRegistration;
            }
        }
        
        public async void Start (ITelegramBotClient botClient, Message message){
            try{// проверка есть ли этот пользователь
            
            await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Для регистации введите своё имя и фамилию");
            _isRegistration = true;
            }
            catch{
                return;
            }
        }

        public async void Regirger (ITelegramBotClient botClient,  Message message){
            var x = message.Text;
            // релеазовать регистрацию
            
            await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Регистрация прошла успешно");
            _isRegistration = false;
        }
 
    }
}