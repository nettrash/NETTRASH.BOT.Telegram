using System;

namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class Video
    {
        #region Public properties



        public string file_id { get; set; }

        public UInt64 width { get; set; }

        public UInt64 height { get; set; }

        public UInt64 duration { get; set; }

        public PhotoSize thumb { get; set; }

        public string mime_type { get; set; }

        public UInt64 file_size { get; set; }



        #endregion
    }
}
