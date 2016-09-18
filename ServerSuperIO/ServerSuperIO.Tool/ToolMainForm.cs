using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerSuperIO.Assembly;
using ServerSuperIO.Communicate;
using ServerSuperIO.Communicate.COM;
using ServerSuperIO.Config;
using ServerSuperIO.Device;

namespace ServerSuperIO.Tool
{
    public partial class ToolMainForm : Form
    {
        private GlobalConfig _config;
        public ToolMainForm()
        {
            InitializeComponent();

            this.treeView1.Nodes.Clear();
            this.Text += " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void ToolMainForm_Load(object sender, EventArgs e)
        {
            this.LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                _config = GlobalConfigTool.Load();

                foreach (Config.Server server in _config.Servers)
                {
                    this.AddServerOfTree(server.ServerConfig);
                    foreach (Config.Device device in server.Devices)
                    {
                        this.AddDeviceOfTree(server.ServerConfig.ServerSession, device);
                    }
                }

                WriteLog("加载配置信息成功");
            }
            catch (Exception ex)
            {
                WriteLog("错误:" + ex.Message);
            }
        }

        private void SaveConfig()
        {
            try
            {
                GlobalConfigTool.Save(_config);
                WriteLog("保存配置信息成功");
            }
            catch (Exception ex)
            {
                WriteLog("错误:" + ex.Message);
            }
        }

        private void WriteLog(string log)
        {
            this.richTextBox1.Text = DateTime.Now.ToString("HH:mm:ss") + ">>" + log + Environment.NewLine + this.richTextBox1.Text;
        }

        private void AddServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm(new ServerConfig());
            addForm.ShowDialog();
            if (addForm.ConfigObject != null)
            {
                AddServer((ServerConfig)addForm.ConfigObject);
            }
        }

        private void DeleteServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = GetRootNode(this.treeView1.SelectedNode);
            if (tn != null)
            {
                RemoveServer(tn.Tag.ToString());
            }
        }

        private void AddDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm(new DeviceProperty());
            addForm.ShowDialog();
            if (addForm.ConfigObject != null)
            {
                TreeNode tn = this.treeView1.SelectedNode;
                if (tn != null)
                {
                    DeviceProperty property = (DeviceProperty)addForm.ConfigObject;
                    Config.Device device = new Config.Device
                    {
                        Caption = property.Caption,
                        DeviceID = property.DeviceID,
                        AssemblyFile = property.AssemblyFile,
                        CommunicateType = property.CommunicateType,
                        DeviceType = property.DeviceType,
                        Instance = property.Instance,
                        Remarks = property.Remarks
                    };

                    try
                    {
                        IObjectBuilder builder = new TypeCreator();
                        IRunDevice runDev = builder.BuildUp<IRunDevice>(property.AssemblyFile, property.Instance);

                        runDev.DeviceParameter.DeviceID = property.DeviceID;
                        runDev.DeviceDynamic.DeviceID = property.DeviceID;
                        runDev.CommunicateType = property.CommunicateType;
                        if (runDev.CommunicateType == CommunicateType.COM)
                        {
                            runDev.DeviceParameter.COM.Port = ComUtils.PortToInt(property.IoParameter1);
                            runDev.DeviceParameter.COM.Baud = property.IoParameter2;
                        }
                        else if (runDev.CommunicateType == CommunicateType.NET)
                        {
                            runDev.DeviceParameter.NET.RemoteIP = property.IoParameter1;
                            runDev.DeviceParameter.NET.RemotePort = property.IoParameter2;
                        }
                        runDev.DeviceParameter.DeviceCode = property.DeviceCode;
                        runDev.DeviceParameter.DeviceAddr = property.DeviceAddr;
                        runDev.DeviceParameter.DeviceName = property.DeviceName;
                        runDev.DeviceParameter.NET.WorkMode = property.WorkMode;
                        runDev.Initialize(runDev.DeviceParameter.DeviceID);

                        TreeNode parentNode = GetRootNode(tn);
                        AddDevice(parentNode.Tag.ToString(), device);
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                }
            }
        }

        private void DeleteDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = this.treeView1.SelectedNode;
            if (tn != null)
            {
                if (tn.Level == 2)
                {
                    TreeNode rootNode = GetRootNode(tn);
                    RemoveDevice(rootNode.Tag.ToString(), tn.Tag.ToString());
                }
            }
        }

        private void AddServer(ServerConfig sc)
        {
            this._config.Servers.Add(new Config.Server(sc));
            this.SaveConfig();
            AddServerOfTree(sc);
        }

        private void RemoveServer(string serverId)
        {
            Config.Server server = _config.Servers.FirstOrDefault(s => s.ServerConfig.ServerSession == serverId);
            if (server != null)
            {
                if (_config.Servers.Remove(server))
                {
                    this.SaveConfig();
                    RemoveServerOfTree(serverId);
                }
            }
        }

        private void AddDevice(string serverId, Config.Device device)
        {
            Config.Server server = _config.Servers.FirstOrDefault(s => s.ServerConfig.ServerSession == serverId);
            if (server != null)
            {
                server.Devices.Add(device);
                this.SaveConfig();
                AddDeviceOfTree(serverId, device);
            }
        }

        private void RemoveDevice(string serverId, string deviceId)
        {
            Config.Server server = _config.Servers.FirstOrDefault(s => s.ServerConfig.ServerSession == serverId);
            if (server != null)
            {
                Config.Device device = server.Devices.FirstOrDefault(d => d.DeviceID == deviceId);
                if (device != null)
                {
                    if (server.Devices.Remove(device))
                    {
                        this.SaveConfig();
                        RemoveDeviceOfTree(serverId, deviceId);
                    }
                }
            }
        }

        private void AddServerOfTree(ServerConfig sc)
        {
            TreeNode tnServer = NewNode(sc.ServerName, sc.ServerSession, 0, 0);
            tnServer.Nodes.AddRange(new TreeNode[]
            {
                 NewNode("COM", "COM", 1, 1),
                 NewNode("TCP/IP", "NET", 2, 2)
            });
            this.treeView1.Nodes.Add(tnServer);
        }

        private void RemoveServerOfTree(string serverId)
        {
            TreeNode[] tnServers = this.treeView1.Nodes.Find(serverId, true);
            if (tnServers.Length > 0)
            {
                this.treeView1.Nodes.Remove(tnServers[0]);
            }
        }

        private void AddDeviceOfTree(string serverId, Config.Device device)
        {
            TreeNode[] tnServers = this.treeView1.Nodes.Find(serverId, true);
            if (tnServers.Length > 0)
            {
                if (device.CommunicateType == CommunicateType.COM)
                {
                    tnServers[0].Nodes[0].Nodes.Add(NewNode(device.Caption, device.DeviceID, 3, 3));
                }
                else if (device.CommunicateType == CommunicateType.NET)
                {
                    tnServers[0].Nodes[1].Nodes.Add(NewNode(device.Caption, device.DeviceID, 3, 3));
                }

                tnServers[0].ExpandAll();
            }
        }

        private void RemoveDeviceOfTree(string serverId, string deviceId)
        {
            TreeNode[] tnDevices = this.treeView1.Nodes.Find(deviceId, true);
            if (tnDevices.Length > 0)
            {
                TreeNode parent = tnDevices[0].Parent;
                if (parent != null)
                {
                    parent.Nodes.Remove(tnDevices[0]);
                }
            }
        }

        private TreeNode NewNode(string text, string tag, int imageIndex, int selectImageIndex)
        {
            return new TreeNode()
            {
                Name = tag,
                Text = text,
                Tag = tag,
                ImageIndex = imageIndex,
                SelectedImageIndex = selectImageIndex
            };
        }

        private TreeNode GetRootNode(TreeNode tn)
        {
            if (tn.Parent != null)
            {
                return GetRootNode(tn.Parent);
            }
            else
            {
                return tn;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            if (tn.Level == 0) //服务
            {
                string serverId = tn.Tag.ToString();
                Config.Server server = _config.Servers.FirstOrDefault(s => s.ServerConfig.ServerSession == serverId);
                if (server != null)
                {
                    this.propertyGrid1.SelectedObject = server.ServerConfig;
                    this.WriteLog("选择服务：" + server.ServerConfig.ServerName);
                }
            }
            else if (tn.Level == 2) //设备
            {
                string serverId = GetRootNode(tn).Tag.ToString();
                Config.Server server = _config.Servers.FirstOrDefault(s => s.ServerConfig.ServerSession == serverId);
                if (server != null)
                {
                    string deviceId = tn.Tag.ToString();
                    Config.Device device = server.Devices.FirstOrDefault(d => d.DeviceID == deviceId);
                    if (device != null)
                    {
                        this.propertyGrid1.SelectedObject = device;
                        this.WriteLog("选择设备：" + device.Caption);
                    }
                }
            }
        }

        private void propertyGrid1_PropertyValueChanged(object obj, PropertyValueChangedEventArgs e)
        {
            TreeNode tn = this.treeView1.SelectedNode;
            if (tn == null)
                return;

            if (tn.Level == 0) //服务
            {
                string serverId = tn.Tag.ToString();
                Config.Server server = _config.Servers.FirstOrDefault(s => s.ServerConfig.ServerSession == serverId);
                if (server != null)
                {
                    server.ServerConfig = (ServerConfig)this.propertyGrid1.SelectedObject; ;

                    this.SaveConfig();

                    if (tn.Text != server.ServerConfig.ServerName)
                    {
                        tn.Text = server.ServerConfig.ServerName;
                    }
                    this.WriteLog("修改服务:"+ server.ServerConfig.ServerName+",属性：" + e.ChangedItem.Label);
                }
            }
            else if (tn.Level == 2) //设备
            {
                string serverId = GetRootNode(tn).Tag.ToString();
                Config.Server server = _config.Servers.FirstOrDefault(s => s.ServerConfig.ServerSession == serverId);
                if (server != null)
                {
                    string deviceId = tn.Tag.ToString();
                    Config.Device device = server.Devices.FirstOrDefault(d => d.DeviceID == deviceId);
                    if (device != null)
                    {
                        device = (Config.Device)this.propertyGrid1.SelectedObject;
                        this.SaveConfig();

                        if (tn.Text != device.Caption)
                        {
                            tn.Text = device.Caption;
                        }
                        this.WriteLog("修改设备:" + device.Caption + ",属性：" + e.ChangedItem.Label);
                    }
                }
            }
        }
    }
}
