using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanyofy.Logging
{
    public class Logger
    {
        public static readonly log4net.ILog Log = CreateLogger();
        
        private static log4net.ILog CreateLogger()
        {
            return log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
