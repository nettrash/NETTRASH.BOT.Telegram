using System.Threading.Tasks;

namespace NETTRASH.BOT.Telegram.Core.Data
{
    public class InputFile
    {
        #region Public properties



        public string FileName { get; set; }

        public byte[] FileContent { get; set; }



        #endregion
        #region Public static methods



        public static async Task<InputFile> LoadAsync(string sFileName)
        {
            InputFile retVal = new InputFile();
            retVal.FileName = System.IO.Path.GetFileName(sFileName);
            retVal.FileContent = await Task.Run(() => System.IO.File.ReadAllBytes(sFileName));
            return retVal;
        }



        #endregion
    }
}
