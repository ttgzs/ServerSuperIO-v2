using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ServerSuperIO.Config;
using System.Xml.Serialization;

namespace ServerSuperIO.Config
{
    [Serializable]
    public class Server
    {
        public Server():this(new ServerConfig())
        {
        }

        public Server(ServerConfig serverConfig)
        {
            ServerConfig = serverConfig;
            Devices=new List<Device>();
        }

        [XmlElement(ElementName = "ServerConfig")]
        public ServerConfig ServerConfig { get; set; }

        [XmlArray(ElementName = "Devices")]
        public List<Device> Devices { get; set; }
    }
}
