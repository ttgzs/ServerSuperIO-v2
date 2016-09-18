using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
using ServerSuperIO.Communicate;
using ServerSuperIO.Device;
using System.Xml.Serialization;

namespace ServerSuperIO.Config
{
    [Serializable]
    public class Device
    {
        public Device()
        {
            DeviceID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 设备ID，一般为Guid
        /// </summary>
        [Category("1.配置属性"),
        DisplayName("DeviceID"),
        Description("标识设备的唯ID，一般为Guid"),
        DefaultValue(""),
        ReadOnly(true)]
        [XmlAttribute(AttributeName = "DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// 程序集标题
        /// </summary>
        [Category("1.配置属性"),
        DisplayName("Caption"),
        Description("设备的标题"),
        DefaultValue("")]
        [XmlAttribute(AttributeName = "Caption")]
        public string Caption { get; set; }

        /// <summary>
        /// 通讯类型
        /// </summary>
        [Category("1.配置属性"),
        DisplayName("CommunicateType"),
        Description("通讯类型，包括：COM和NET"),
        DefaultValue(CommunicateType.COM)]
        [XmlAttribute(AttributeName = "CommunicateType")]
        public CommunicateType CommunicateType { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        [Category("1.配置属性"),
        DisplayName("DeviceType"),
        Description("设备类型，包括：Common和Virtual"),
        DefaultValue(DeviceType.Common)]
        [XmlAttribute(AttributeName = "DeviceType")]
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// 程序集文件 路径
        /// </summary>
        [Category("1.配置属性"),
        DisplayName("AssemblyFile"),
        Description("程序集文件的路径"),
        DefaultValue("")]
        [XmlAttribute(AttributeName = "AssemblyFile")]
        public string AssemblyFile { get; set; }

        /// <summary>
        /// 实例，命名空间.类名称
        /// </summary>
        [Category("1.配置属性"),
        DisplayName("Instance"),
        Description("设备驱动的实例，包括命令空间和类名称"),
        DefaultValue("")]
        [XmlAttribute(AttributeName = "Instance")]
        public string Instance { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Category("1.配置属性"),
        DisplayName("Remarks"),
        Description("备注信息"),
        DefaultValue("")]
        [XmlAttribute(AttributeName = "Remarks")]
        public string Remarks { get; set; }
    }
}
