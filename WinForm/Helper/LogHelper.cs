using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Helper
{
    public class LogHelper
    {
        private ILog Instance;
        static LogHelper()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private Object obj = new object();
        public LogHelper(Type type)
        {
            if (Instance == null)
            {
                lock (obj)
                {
                    if (Instance == null)
                    {
                        Instance = LogManager.GetLogger(type);
                    }
                }
            }
        }

        public void Error(string str)
        {
            Instance.Error(str);
        }
    }
}
