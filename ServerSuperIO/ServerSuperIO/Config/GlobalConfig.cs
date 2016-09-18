using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ServerSuperIO.Config
{
    [Serializable]
    public class GlobalConfig
    {
        public GlobalConfig()
        {
            Servers=new List<Server>();
        }

        [XmlArray(ElementName = "Servers")]
        public List<Server> Servers { get; set; }
    }
}
