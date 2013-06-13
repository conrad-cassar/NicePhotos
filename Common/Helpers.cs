using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class Helpers
    {
        public static string GetWebMessageScriptInTags(MessageType messageType, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>");
            sb.Append(Helpers.GetWebMessageScript(messageType, message));
            sb.Append("</script>");
            return sb.ToString();
        }

        public static string GetWebMessageScript(MessageType messageType, string message)
        {
            string tmpType;

            if (messageType == MessageType.Error) { tmpType = "error"; }
            else { tmpType = "info"; }

            return "$('#msg').message({type:'"
                + tmpType
                + "', message:'<b>"
                + message
                + "</b>&nbsp;&nbsp;'});";
        }

        public enum MessageType
        {
            Error = 0,
            Information = 1
        }

        public static string FormatMessage(string message)
        {
            string m = message.Replace("\n", "<br />");

            m = m.Replace(":-)", "<img src='/Styling/Images/emoticons/smile.gif' alt=':-)'>");
            m = m.Replace(":)", "<img src='/Styling/Images/emoticons/smile.gif' alt=':-)'>");
            m = m.Replace(":-(", "<img src='/Styling/Images/emoticons/frown.gif' alt=':-('>");
            m = m.Replace(":(", "<img src='/Styling/Images/emoticons/frown.gif' alt=':-('>");

            return m;
        }
    }
}
