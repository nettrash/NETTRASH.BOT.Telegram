using System;

namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class Contact
    {
        #region Public properties



        public string phone_number { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public UInt64 user_id { get; set; }



        #endregion
    }
}
