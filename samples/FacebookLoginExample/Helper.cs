using System;
using System.Diagnostics;
using System.Text;

/////////////////////////////////////////////////////////////////////
////////////////////// IMPORTANT NOTE ///////////////////////////////
// Please check wiki pages for more information:
// https://github.com/ramtinak/InstagramApiSharp/wiki
////////////////////// IMPORTANT NOTE ///////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace FacebookLoginExample
{
    internal static class InstaHelper
    {
        public static string PrintException(this Exception ex, string name = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{name} exception thrown: ");
            sb.AppendLine($"Source: {ex.Source}");
            sb.AppendLine($"Stack trace: {ex.StackTrace}");
            sb.AppendLine();
            return sb.Output();
        }

        public static string Output(this object source, string start = "")
        {
            var content = $"{start} {Convert.ToString(source)}";
            Debug.WriteLine(content);
            return content;
        }
    }
}