namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class CallbackQuery
    {
        #region Public properties



        public string id { get; set; }

        public User from { get; set; }

        public Message message { get; set; }

        public string inline_message_id { get; set; }

        public string data { get; set; }



        #endregion
    }
}
