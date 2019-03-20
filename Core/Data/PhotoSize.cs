using System;

namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class PhotoSize
    {
        #region Public properties



        public string file_id { get; set; }

        public UInt64 width { get; set; }

        public UInt64 height { get; set; }

        public UInt64 file_size { get; set; }



        #endregion
    }
}
