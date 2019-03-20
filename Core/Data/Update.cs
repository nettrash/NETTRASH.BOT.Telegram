using System;

namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class Update
    {
        #region Public properties



        public UInt64 update_id { get; set; }

        public Message message { get; set; }

        public InlineQuery inline_query { get; set; }

        public ChosenInlineResult chosen_inline_result { get; set; }

        public CallbackQuery callback_query { get; set; }



        #endregion
    }
}
