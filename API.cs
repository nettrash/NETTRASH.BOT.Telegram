using NETTRASH.BOT.Telegram.Core;
using NETTRASH.BOT.Telegram.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Specialized;
using System.Web;

namespace NETTRASH.BOT.Telegram
{
    public class API
    {
        #region Private properties



        private static NLog.Logger m_Log = NLog.LogManager.GetCurrentClassLogger();

        private string _AuthToken { get; set; }



        #endregion
        #region Public properties



        public string AuthToken
        {
            get
            {
                return _AuthToken;
            }
        }

        public BotInfo Info { get; private set; }



        #endregion
        #region Private methods



        private string _GetBoundary()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        private async Task<string> _GetAsync(Uri uri, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri, cancellationToken);

                return await response.Content.ReadAsStringAsync();
            }
        }

        private async Task<string> _PostAsync(Uri uri, FormUrlEncodedContent form, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(uri, form, cancellationToken);

                return await response.Content.ReadAsStringAsync();
            }
        }

        private async Task<string> _PostAsync(Uri uri, MultipartFormDataContent form, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(uri, form, cancellationToken);

                return await response.Content.ReadAsStringAsync();
            }
        }



        #endregion
        #region Public constructors



        public API()
        {
            _AuthToken = string.Empty;
        }

        public API(string sAuthToken)
        {
            _AuthToken = sAuthToken;
        }



        #endregion
        #region Public methods



        public async Task InitializeAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Info = await GetInfoAsync(cancellationToken);
        }

        public async Task<BotInfo> GetInfoAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<BotInfo>(await _GetAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/getMe"), cancellationToken));
        }

        /// <summary>
        /// getUpdates
        ///     Use this method to receive incoming updates using long polling(wiki). An Array of Update objects is returned.
        ///
        ///     Notes
        ///         1. This method will not work if an outgoing webhook is set up.
        ///         2. In order to avoid getting duplicate updates, recalculate offset after each server response.
        /// </summary>
        /// <param name="offset">Optional Identifier of the first update to be returned.Must be greater by one than the highest among the identifiers of previously received updates.By default, updates starting with the earliest unconfirmed update are returned.An update is considered confirmed as soon as getUpdates is called with an offset higher than its update_id.The negative offset can be specified to retrieve updates starting from -offset update from the end of the updates queue.All previous updates will forgotten.</param>
        /// <param name="limit">Optional Limits the number of updates to be retrieved.Values between 1—100 are accepted. Defaults to 100.</param>
        /// <param name="timeout">Optional Timeout in seconds for long polling. Defaults to 0, i.e.usual short polling</param>
        public async Task<Updates> GetUpdatesAsync(UInt64? offset = null, UInt64? limit = null, UInt64? timeout = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Uri uri = new Uri($"https://api.telegram.org/bot{_AuthToken}/getUpdates");
            if (offset.HasValue)
                uri = uri.AddQuery("offset", offset.Value.ToString());
            if (limit.HasValue)
                uri = uri.AddQuery("limit", limit.Value.ToString());
            if (timeout.HasValue)
                uri = uri.AddQuery("timeout", timeout.Value.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Updates>(await _GetAsync(uri, cancellationToken));
        }

        /// <summary>
        /// Use this method to specify a url and receive incoming updates via an outgoing webhook. Whenever there is an update for the bot, we will send an HTTPS POST request to the specified url, containing a JSON-serialized Update. In case of an unsuccessful request, we will give up after a reasonable amount of attempts.
        /// If you'd like to make sure that the Webhook request comes from Telegram, we recommend using a secret path in the URL, e.g. https://www.example.com/<token>. Since nobody else knows your bot‘s token, you can be pretty sure it’s us.
        /// </summary>
        /// <param name="url">HTTPS url to send updates to. Use an empty string to remove webhook integration</param>
        /// <param name="certificate">Upload your public key certificate so that the root certificate in use can be checked.</param>
        public async Task<Result> SetWebhookAsync(string url = null, InputFile certificate = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Uri uri = new Uri($"https://api.telegram.org/bot{_AuthToken}/setWebhook");
            if (!string.IsNullOrEmpty(url))
                uri = uri.AddQuery("url", url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(await _GetAsync(uri, cancellationToken));
        }

        /// <summary>
        /// sendMessage
        /// Use this method to send text messages. On success, the sent Message is returned.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="text">Text of the message to be sent</param>
        /// <param name="parse_mode">Optional Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in your bot's message.</param>
        /// <param name="disable_web_page_preview">Optional Disables link previews for links in this message</param>
        /// <param name="disable_notification">Optional Sends the message silently.iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id">Optional If the message is a reply, ID of the original message</param>
        /// <param name="reply_markup">InlineKeyboardMarkup or ReplyKeyboardMarkup or ReplyKeyboardHide or ForceReply  Optional    Additional interface options. A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions to hide reply keyboard or to force a reply from the user.</param>
        public async Task<Core.Message> SendMessageAsync(string chat_id, string text, string parse_mode = null, bool? disable_web_page_preview = null, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Dictionary<string, string> prms = new Dictionary<string, string>();
            prms.Add("chat_id", chat_id);
            prms.Add("text", text);
            if (!string.IsNullOrEmpty(parse_mode))
                prms.Add("parse_mode", parse_mode);
            if (disable_web_page_preview.HasValue)
                prms.Add("disable_web_page_preview", disable_web_page_preview.Value.ToString().ToLower());
            if (disable_notification.HasValue)
                prms.Add("disable_notification", disable_notification.Value.ToString().ToLower());
            if (reply_to_message_id.HasValue)
                prms.Add("reply_to_message_id", reply_to_message_id.Value.ToString());
            if (!string.IsNullOrEmpty(reply_markup))
                prms.Add("reply_markup", reply_markup);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendMessage"), new FormUrlEncodedContent(prms), cancellationToken));
        }

        /// <summary>
        /// Use this method to forward messages of any kind. On success, the sent Message is returned.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="from_chat_id">Unique identifier for the chat where the original message was sent (or channel username in the format @channelusername)</param>
        /// <param name="disable_notification">Optional	Sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="message_id">Unique message identifier</param>
        /// <returns></returns>
        public async Task<Core.Message> ForwardMessageAsync(string chat_id, string from_chat_id, bool? disable_notification, UInt64 message_id, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Dictionary<string, string> prms = new Dictionary<string, string>();
            prms.Add("chat_id", chat_id);
            prms.Add("from_chat_id", from_chat_id);
            if (disable_notification.HasValue)
                prms.Add("disable_notification", disable_notification.Value.ToString().ToLower());
            prms.Add("message_id", message_id.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/forwardMessage"), new FormUrlEncodedContent(prms), cancellationToken));
        }

        /// <summary>
        /// Use this method to send photos. On success, the sent Message is returned.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="new_photo">(new image PNG)Photo to send. You can either pass a file_id as String to resend a photo that is already on the Telegram servers, or upload a new photo using multipart/form-</param>
        /// <param name="exists_photo">(file_id) Photo to send. You can either pass a file_id as String to resend a photo that is already on the Telegram servers, or upload a new photo using multipart/form-</param>
        /// <param name="caption">Optional  Photo caption (may also be used when resending photos by file_id), 0-200 characters</param>
        /// <param name="disable_notification">Optional    Sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id">Optional	If the message is a reply, ID of the original message</param>
        /// <param name="reply_markup">Optional	Additional interface options. A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions to hide reply keyboard or to force a reply from the user.</param>
        /// <returns></returns>
        public async Task<Core.Message> SendPhotoAsync(string chat_id, InputFile new_photo, string exists_photo = null, string caption = null, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            if (new_photo != null)
            {
                form.Add(new ByteArrayContent(new_photo.FileContent, 0, new_photo.FileContent.Length), "photo", new_photo.FileName);
            }
            else
            {
                form.Add(new StringContent(exists_photo), "photo");
            }
            if (!string.IsNullOrEmpty(caption))
            {
                form.Add(new StringContent(caption), "caption");
            }
            if (disable_notification.HasValue)
            {
                form.Add(new StringContent(disable_notification.Value.ToString().ToLower()), "disable_notification");
            }
            if (reply_to_message_id.HasValue)
            {
                form.Add(new StringContent(reply_to_message_id.Value.ToString()), "reply_to_message_id");
            }
            if (!string.IsNullOrEmpty(reply_markup))
            {
                form.Add(new StringContent(reply_markup), "reply_markup");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendPhoto"), form, cancellationToken));
        }

        /// <summary>
        /// Use this method to send audio files, if you want Telegram clients to display them in the music player. Your audio must be in the .mp3 format. On success, the sent Message is returned. Bots can currently send audio files of up to 50 MB in size, this limit may be changed in the future.
        ///
        /// For sending voice messages, use the sendVoice method instead.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="new_audio">Audio file to send. You can either pass a file_id as String to resend an audio that is already on the Telegram servers, or upload a new audio file using multipart/form-</param>
        /// <param name="exists_audio">Audio file to send. You can either pass a file_id as String to resend an audio that is already on the Telegram servers, or upload a new audio file using multipart/form-</param>
        /// <param name="duration">Optional Duration of the audio in seconds</param>
        /// <param name="performer">Optional    Performer</param>
        /// <param name="title">Optional	Track name</param>
        /// <param name="disable_notification">Optional	Sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id">Optional	If the message is a reply, ID of the original message</param>
        /// <param name="reply_markup">Optional	Additional interface options. A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions to hide reply keyboard or to force a reply from the user.</param>
        /// <returns></returns>
        public async Task<Core.Message> SendAudioAsync(string chat_id, InputFile new_audio, string exists_audio = null, UInt64? duration = null, string performer = null, string title = null, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            if (new_audio != null)
            {
                form.Add(new ByteArrayContent(new_audio.FileContent, 0, new_audio.FileContent.Length), "audio", new_audio.FileName);
            }
            else
            {
                form.Add(new StringContent(exists_audio), "audio");
            }
            if (duration.HasValue)
            {
                form.Add(new StringContent(duration.Value.ToString()), "duration");
            }
            if (!string.IsNullOrEmpty(performer))
            {
                form.Add(new StringContent(performer), "performer");
            }
            if (!string.IsNullOrEmpty(title))
            {
                form.Add(new StringContent(title), "title");
            }
            if (disable_notification.HasValue)
            {
                form.Add(new StringContent(disable_notification.Value.ToString().ToLower()), "disable_notification");
            }
            if (reply_to_message_id.HasValue)
            {
                form.Add(new StringContent(reply_to_message_id.Value.ToString()), "reply_to_message_id");
            }
            if (!string.IsNullOrEmpty(reply_markup))
            {
                form.Add(new StringContent(reply_markup), "reply_markup");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendAudio"), form, cancellationToken));
        }

        /// <summary>
        /// Use this method to send general files. On success, the sent Message is returned. Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the future.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="new_document">File to send. You can either pass a file_id as String to resend a file that is already on the Telegram servers, or upload a new file using multipart/form-</param>
        /// <param name="exists_document">File to send. You can either pass a file_id as String to resend a file that is already on the Telegram servers, or upload a new file using multipart/form-</param>
        /// <param name="caption">Optional	Document caption (may also be used when resending documents by file_id), 0-200 characters</param>
        /// <param name="disable_notification">Optional	Sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id">Optional	If the message is a reply, ID of the original message</param>
        /// <param name="reply_markup">Optional	Additional interface options. A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions to hide reply keyboard or to force a reply from the user.</param>
        /// <returns></returns>
        public async Task<Core.Message> SendDocumentAsync(string chat_id, InputFile new_document, string exists_document = null, string caption = null, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            if (new_document != null)
            {
                form.Add(new ByteArrayContent(new_document.FileContent, 0, new_document.FileContent.Length), "document", new_document.FileName);
            }
            else
            {
                form.Add(new StringContent(exists_document), "document");
            }
            if (!string.IsNullOrEmpty(caption))
            {
                form.Add(new StringContent(caption), "caption");
            }
            if (disable_notification.HasValue)
            {
                form.Add(new StringContent(disable_notification.Value.ToString().ToLower()), "disable_notification");
            }
            if (reply_to_message_id.HasValue)
            {
                form.Add(new StringContent(reply_to_message_id.Value.ToString()), "reply_to_message_id");
            }
            if (!string.IsNullOrEmpty(reply_markup))
            {
                form.Add(new StringContent(reply_markup), "reply_markup");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendDocument"), form, cancellationToken));
        }

        /// <summary>
        /// Use this method to send .webp stickers. On success, the sent Message is returned.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="new_sticker">Sticker to send. You can either pass a file_id as String to resend a sticker that is already on the Telegram servers, or upload a new sticker using multipart/form-</param>
        /// <param name="exists_sticker">Sticker to send. You can either pass a file_id as String to resend a sticker that is already on the Telegram servers, or upload a new sticker using multipart/form-</param>
        /// <param name="disable_notification">Optional	Sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id">Optional	If the message is a reply, ID of the original message</param>
        /// <param name="reply_markup">Optional	Additional interface options. A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions to hide reply keyboard or to force a reply from the user.</param>
        /// <returns></returns>
        public async Task<Core.Message> SendStickerAsync(string chat_id, InputFile new_sticker, string exists_sticker = null, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            if (new_sticker != null)
            {
                form.Add(new ByteArrayContent(new_sticker.FileContent, 0, new_sticker.FileContent.Length), "sticker", new_sticker.FileName);
            }
            else
            {
                form.Add(new StringContent(exists_sticker), "sticker");
            }
            if (disable_notification.HasValue)
            {
                form.Add(new StringContent(disable_notification.Value.ToString().ToLower()), "disable_notification");
            }
            if (reply_to_message_id.HasValue)
            {
                form.Add(new StringContent(reply_to_message_id.Value.ToString()), "reply_to_message_id");
            }
            if (!string.IsNullOrEmpty(reply_markup))
            {
                form.Add(new StringContent(reply_markup), "reply_markup");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendSticker"), form, cancellationToken));
        }

        /// <summary>
        /// Use this method to send video files, Telegram clients support mp4 videos (other formats may be sent as Document). On success, the sent Message is returned. Bots can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="new_photo">Video to send. You can either pass a file_id as String to resend a video that is already on the Telegram servers, or upload a new video file using multipart/form-</param>
        /// <param name="exists_photo">Video to send. You can either pass a file_id as String to resend a video that is already on the Telegram servers, or upload a new video file using multipart/form-</param>
        /// <param name="duration">Optional	Duration of sent video in seconds</param>
        /// <param name="width">Optional	Video width</param>
        /// <param name="height">Optional	Video height</param>
        /// <param name="caption">Optional	Video caption (may also be used when resending videos by file_id), 0-200 characters</param>
        /// <param name="disable_notification">Optional	Sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id">Optional	If the message is a reply, ID of the original message</param>
        /// <param name="reply_markup">Optional	Additional interface options. A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions to hide reply keyboard or to force a reply from the user.</param>
        /// <returns></returns>
        public async Task<Core.Message> SendVideoAsync(string chat_id, InputFile new_video, string exists_video = null, UInt64? duration = null, UInt64? width = null, UInt64? height = null, string caption = null, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            if (new_video != null)
            {
                form.Add(new ByteArrayContent(new_video.FileContent, 0, new_video.FileContent.Length), "video", new_video.FileName);
            }
            else
            {
                form.Add(new StringContent(exists_video), "video");
            }
            if (duration.HasValue)
            {
                form.Add(new StringContent(duration.Value.ToString()), "duration");
            }
            if (width.HasValue)
            {
                form.Add(new StringContent(width.Value.ToString()), "width");
            }
            if (height.HasValue)
            {
                form.Add(new StringContent(height.Value.ToString()), "height");
            }
            if (!string.IsNullOrEmpty(caption))
            {
                form.Add(new StringContent(caption), "caption");
            }
            if (disable_notification.HasValue)
            {
                form.Add(new StringContent(disable_notification.Value.ToString().ToLower()), "disable_notification");
            }
            if (reply_to_message_id.HasValue)
            {
                form.Add(new StringContent(reply_to_message_id.Value.ToString()), "reply_to_message_id");
            }
            if (!string.IsNullOrEmpty(reply_markup))
            {
                form.Add(new StringContent(reply_markup), "reply_markup");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendVideo"), form, cancellationToken));
        }

        /// <summary>
        /// Use this method to send audio files, if you want Telegram clients to display the file as a playable voice message. For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent as Audio or Document). On success, the sent Message is returned. Bots can currently send voice messages of up to 50 MB in size, this limit may be changed in the future.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="new_voice">Audio file to send. You can either pass a file_id as String to resend an audio that is already on the Telegram servers, or upload a new audio file using multipart/form-</param>
        /// <param name="exists_voice">Audio file to send. You can either pass a file_id as String to resend an audio that is already on the Telegram servers, or upload a new audio file using multipart/form-</param>
        /// <param name="duration">Optional	Duration of sent audio in seconds</param>
        /// <param name="disable_notification">Optional	Sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id">Optional	If the message is a reply, ID of the original message</param>
        /// <param name="reply_markup">Optional	Additional interface options. A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions to hide reply keyboard or to force a reply from the user.</param>
        /// <returns></returns>
        public async Task<Core.Message> SendVoiceAsync(string chat_id, InputFile new_voice, string exists_voice = null, UInt64? duration = null, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            if (new_voice != null)
            {
                form.Add(new ByteArrayContent(new_voice.FileContent, 0, new_voice.FileContent.Length), "voice", new_voice.FileName);
            }
            else
            {
                form.Add(new StringContent(exists_voice), "voice");
            }
            if (duration.HasValue)
            {
                form.Add(new StringContent(duration.Value.ToString()), "duration");
            }
            if (disable_notification.HasValue)
            {
                form.Add(new StringContent(disable_notification.Value.ToString().ToLower()), "disable_notification");
            }
            if (reply_to_message_id.HasValue)
            {
                form.Add(new StringContent(reply_to_message_id.Value.ToString()), "reply_to_message_id");
            }
            if (!string.IsNullOrEmpty(reply_markup))
            {
                form.Add(new StringContent(reply_markup), "reply_markup");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendVoice"), form, cancellationToken));
        }

        /// <summary>
        /// Use this method to send point on the map. On success, the sent Message is returned.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="latitude">Latitude of location</param>
        /// <param name="longitude">Longitude of location</param>
        /// <param name="disable_notification">Optional	Sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id">Optional	If the message is a reply, ID of the original message</param>
        /// <param name="reply_markup">Optional	Additional interface options. A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions to hide reply keyboard or to force a reply from the user.</param>
        /// <returns></returns>
        public async Task<Core.Message> SendLocationAsync(string chat_id, decimal latitude, decimal longitude, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            form.Add(new StringContent(latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)), "latitude");
            form.Add(new StringContent(longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)), "longitude");
            if (disable_notification.HasValue)
            {
                form.Add(new StringContent(disable_notification.Value.ToString().ToLower()), "disable_notification");
            }
            if (reply_to_message_id.HasValue)
            {
                form.Add(new StringContent(reply_to_message_id.Value.ToString()), "reply_to_message_id");
            }
            if (!string.IsNullOrEmpty(reply_markup))
            {
                form.Add(new StringContent(reply_markup), "reply_markup");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendLocation"), form, cancellationToken));
        }

        /// <summary>
        /// Use this method to send information about a venue. On success, the sent Message is returned.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="latitude">Latitude of the venue</param>
        /// <param name="longitude">Longitude of the venue</param>
        /// <param name="title">Name of the venue</param>
        /// <param name="address">Address of the venue</param>
        /// <param name="foursquare_id">Optional Foursquare identifier of the venue</param>
        /// <param name="disable_notification">Optional	Sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id">Optional	If the message is a reply, ID of the original message</param>
        /// <param name="reply_markup">Optional	Additional interface options. A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions to hide reply keyboard or to force a reply from the user.</param>
        /// <returns></returns>
        public async Task<Core.Message> SendVenueAsync(string chat_id, decimal latitude, decimal longitude, string title, string address, string foursquare_id = null, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            form.Add(new StringContent(latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)), "latitude");
            form.Add(new StringContent(longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)), "longitude");
            form.Add(new StringContent(title), "title");
            form.Add(new StringContent(address), "address");
            if (!string.IsNullOrEmpty(foursquare_id))
            {
                form.Add(new StringContent(foursquare_id), "foursquare_id");
            }
            if (disable_notification.HasValue)
            {
                form.Add(new StringContent(disable_notification.Value.ToString().ToLower()), "disable_notification");
            }
            if (reply_to_message_id.HasValue)
            {
                form.Add(new StringContent(reply_to_message_id.Value.ToString()), "reply_to_message_id");
            }
            if (!string.IsNullOrEmpty(reply_markup))
            {
                form.Add(new StringContent(reply_markup), "reply_markup");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendVenue"), form, cancellationToken));
        }

        public async Task<Core.Message> SendContactAsync(string chat_id, string phone_number, string first_name, string last_name = null, bool? disable_notification = null, UInt64? reply_to_message_id = null, string reply_markup = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            form.Add(new StringContent(phone_number), "phone_number");
            form.Add(new StringContent(first_name), "first_name");
            if (!string.IsNullOrEmpty(last_name))
            {
                form.Add(new StringContent(last_name), "last_name");
            }
            if (disable_notification.HasValue)
            {
                form.Add(new StringContent(disable_notification.Value.ToString().ToLower()), "disable_notification");
            }
            if (reply_to_message_id.HasValue)
            {
                form.Add(new StringContent(reply_to_message_id.Value.ToString()), "reply_to_message_id");
            }
            if (!string.IsNullOrEmpty(reply_markup))
            {
                form.Add(new StringContent(reply_markup), "reply_markup");
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Message>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendContact"), form, cancellationToken));
        }

        /// <summary>
        /// Use this method when you need to tell the user that something is happening on the bot's side. The status is set for 5 seconds or less (when a message arrives from your bot, Telegram clients clear its typing status).
        /// Example: The ImageBot needs some time to process a request and upload the image.Instead of sending a text message along the lines of “Retrieving image, please wait…”, the bot may use sendChatAction with action = upload_photo.The user will see a “sending photo” status for the bot.
        /// We only recommend using this method when a response from the bot will take a noticeable amount of time to arrive.
        /// </summary>
        /// <param name="chat_id">Unique identifier for the target chat or username of the target channel (in the format @channelusername)</param>
        /// <param name="action">Type of action to broadcast. Choose one, depending on what the user is about to receive: typing for text messages, upload_photo for photos, record_video or upload_video for videos, record_audio or upload_audio for audio files, upload_document for general files, find_location for location </param>
        /// <returns></returns>
        public async Task<Result> SendChatActionAsync(string chat_id, string action, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(chat_id), "chat_id");
            form.Add(new StringContent(action), "action");

            return Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(await _PostAsync(new Uri($"https://api.telegram.org/bot{_AuthToken}/sendChatAction"), form, cancellationToken));
        }



        #endregion
    }
}
