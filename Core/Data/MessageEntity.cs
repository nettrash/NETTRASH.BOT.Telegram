using System;

namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class MessageEntity
    {
        #region Public properties



        public string type { get; set; }

        public UInt64 offset { get; set; }

        public UInt64 length { get; set; }

        public string url { get; set; }



        #endregion
    }
}
