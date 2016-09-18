using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ServerSuperIO.Communicate.NET;
using ServerSuperIO.Device;

namespace ServerSuperIO.Tool
{
    public class DeviceProperty:Config.Device
    {
        [Category("2.设备属性"),
        DisplayName("DeviceName"),
        Description("设备名称，一般与Caption一致"),
        DefaultValue("")]
        public string DeviceName { set; get; }

        [Category("2.设备属性"),
        DisplayName("IoParameter1"),
        Description("通讯参数1，如果是COM通讯则代表串口号，如果是NET通讯则代表IP地址"),
        DefaultValue("")]
        public string IoParameter1 { set; get; }

        [Category("2.设备属性"),
        DisplayName("IoParameter2"),
        Description("通讯参数2，如果是COM通讯则代表波特率，如果是NET通讯则代表端口号"),
        DefaultValue(9600)]
        public int IoParameter2 { set; get; }

        [Category("2.设备属性"),
        DisplayName("DeviceAddr"),
        Description("设备地址，指的是设备内部设置的地址信息，一般与DeviceCode一致"),
        DefaultValue(0)]
        public int DeviceAddr { set; get; }

        [Category("2.设备属性"),
        DisplayName("DeviceCode"),
        Description("设备编号，作为识别设备的唯一编号"),
        DefaultValue("")]
        public string DeviceCode { set; get; }

        [Category("2.设备属性"),
        DisplayName("WorkMode"),
        Description("工作模式，设置设备在网络通讯下是TcpServer模式，还是TcpClient模式"),
        DefaultValue(WorkMode.TcpServer)]
        public WorkMode WorkMode { set; get; }
    }
}
