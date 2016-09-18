using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ServerSuperIO.Config;

namespace ServerSuperIO.Tool
{
    [XmlInclude(typeof(ServerConfig))]
    [Serializable]
    
    public class ServerProperty:ServerConfig
    {

    }
}
