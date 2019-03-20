using System;

namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class Audio
    {
        #region Public properties



        public string file_id { get; set; }

        public UInt64 duration { get; set; }

        public string performer { get; set; }

        public string title { get; set; }

        public string mime_type { get; set; }

        public UInt64 file_size { get; set; }



        #endregion
    }
}
