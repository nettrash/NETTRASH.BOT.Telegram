namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class InlineQuery
    {
        #region Public properties



        public string id { get; set; }

        public User from { get; set; }

        public Location location { get; set; }

        public string query { get; set; }

        public string offset { get; set; }



        #endregion
    }
}
