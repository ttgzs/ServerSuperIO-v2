using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSuperIO.Common;

namespace ServerSuperIO.Config
{
    public static class GlobalConfigTool
    {
        public static string GlobalConfigPath = AppDomain.CurrentDomain.BaseDirectory + "ServerSuperIO/GlobalConfig.cfg";
        static GlobalConfigTool()
        {
            string parentDir = System.IO.Path.GetDirectoryName(GlobalConfigPath);
            if (!String.IsNullOrEmpty(parentDir))
            {
                if (!System.IO.Directory.Exists(parentDir))
                {
                    System.IO.Directory.CreateDirectory(parentDir);
                }
            }
        }

        public static void Save(GlobalConfig gc)
        {

            SerializeUtil.XmlSerialize(GlobalConfigPath,gc);
        }

        public static GlobalConfig Load()
        {
            if (!System.IO.File.Exists(GlobalConfigPath))
            {
                Save(new GlobalConfig());
            }

            return SerializeUtil.XmlDeserailize<GlobalConfig>(GlobalConfigPath);
        }
    }
}
