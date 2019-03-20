using System;

namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class Document
    {
        #region Public properties



        public string file_id { get; set; }

        public PhotoSize thumb { get; set; }

        public string file_name { get; set; }

        public string mime_type { get; set; }

        public UInt64 file_size { get; set; }



        #endregion
    }
}
