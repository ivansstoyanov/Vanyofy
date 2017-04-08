using System.Xml;

namespace Vanyofy.Logging
{
    public class Logger
    {
        public static readonly log4net.ILog Log = CreateLogger();
        
        private static log4net.ILog CreateLogger()
        {
            XmlDocument doc = new XmlDocument();
            string xml = "<log4net><root><level value=\"ALL\" />" +
                "<appender-ref ref=\"MyFileAppender\"/>" +
                "</root>" +
                "<appender name=\"MyFileAppender\" type=\"log4net.Appender.FileAppender\" >" +
                "<file value=\"${LOCALAPPDATA}\\Vanyofy\\Logs\\application.log\" />" +
                "<appendToFile value=\"true\" />" +
                "<lockingModel type=\"log4net.Appender.FileAppender+MinimalLock\" />" +
                "<layout type=\"log4net.Layout.PatternLayout\">" +
                "<conversionPattern value=\"%date - %message%newline\" />" +
                "</layout>" +
                "</appender></log4net>";
            doc.LoadXml(xml);

            log4net.Config.XmlConfigurator.Configure(doc.DocumentElement);
            return log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
