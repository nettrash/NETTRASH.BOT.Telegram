using System;
using System.Collections.Specialized;
using System.Web;

namespace NETTRASH.BOT.Telegram
{
    public static class Extensions
    {
        #region Uri extensions



        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            UriBuilder builder = new UriBuilder(uri);
            NameValueCollection httpValueCollection = HttpUtility.ParseQueryString(uri.Query);
            httpValueCollection.Add(name, value);
            builder.Query = httpValueCollection.ToString();
            return builder.Uri;
        }



        #endregion
    }
}
