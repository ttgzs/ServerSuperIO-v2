using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ServerSuperIO.Common;
using ServerSuperIO.Communicate;
using ServerSuperIO.DataCache;
using ServerSuperIO.Log;
using ServerSuperIO.Protocol;
using ServerSuperIO.Server;
using Timer = System.Timers.Timer;

namespace ServerSuperIO.Device
{
    public abstract class RunDevice : ServerProvider, IRunDevice
    {
        private string _SaveBytesPath = AppDomain.CurrentDomain.BaseDirectory + "原始数据";
        private bool _IsRunTimer = false;
        private System.Timers.Timer _Timer = null;
        private readonly object _SyncLock = new object();
        private bool _IsDisposed = false; //是否释放资源

        private MonitorChannelForm _monitorChannelForm = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected RunDevice()
        {
            this.Tag = null;
            this.IsRunDevice = true;
            this.DevicePriority = DevicePriority.Normal;

            this._Timer = new Timer(1000) { AutoReset = true };
            this._Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            this.IsRunTimer = false;
            this.RunTimerInterval = 1000;
        }

        /// <summary>
        /// 终结器
        /// </summary>
        ~RunDevice()
        {
            Dispose(false);
        }

        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <param name="devid"></param>
        public abstract void Initialize(string devid);

        /// <summary>
        /// 保存源始数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="desc"></param>
        public void SaveBytes(byte[] data, string desc)
        {
            if (this.DeviceParameter.IsSaveOriginBytes)
            {
                if (!System.IO.Directory.Exists(_SaveBytesPath))
                {
                    System.IO.Directory.CreateDirectory(_SaveBytesPath);
                }

                string path = String.Format("{0}/{1}_{2}.txt", _SaveBytesPath, DateTime.Now.ToString("yyyy年MM月dd日"), this.DeviceParameter.DeviceID.ToString());

                string hexString = String.Format("[{0}]{1}", desc, BinaryUtil.ByteToHex(data));

                FileUtil.WriteAppend(path, hexString);
            }
        }

        /// <summary>
        /// 获得要发送的数据信息
        /// </summary>
        /// <returns></returns>
        public byte[] GetSendBytes()
        {
            byte[] data = new byte[] { };
            //如果没有命令就增加实时数据的命令
            if (this.Protocol.SendCache.Count <= 0)
            {
                data = this.GetConstantCommand();
                this.DevicePriority = DevicePriority.Normal;
            }
            else
            {
                data = this.Protocol.SendCache.Get();
                this.DevicePriority = DevicePriority.Priority;
            }
            return data;
        }

        /// <summary>
        /// 获得实时数据
        /// </summary>
        /// <returns></returns>
        public abstract byte[] GetConstantCommand();

        /// <summary>
        /// 发送数据接口
        /// </summary>
        /// <param name="io"></param>
        /// <param name="senddata"></param>
        public virtual int Send(IChannel io, byte[] senddata)
        {
            return io.Write(senddata);
        }

        /// <summary>
        /// 接收数据接口
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        public virtual byte[] Receive(IChannel io)
        {
            return io.Read();
        }

        /// <summary>
        /// 接收数据接口
        /// </summary>
        /// <param name="io"></param>
        /// <param name="receiveFilter"></param>
        /// <returns></returns>
        public virtual IList<byte[]> Receive(IChannel io, IReceiveFilter receiveFilter)
        {
            return io.Read(receiveFilter);
        }

        /// <summary>
        /// 内部读数据
        /// </summary>
        /// <param name="io"></param>
        /// <param name="receiveFilter"></param>
        /// <returns></returns>
        private IList<byte[]> InternalReceive(IChannel io, IReceiveFilter receiveFilter)
        {
            List<byte[]> list = new List<byte[]>();
            if (!this.Server.ServerConfig.StartReceiveDataFliter)
            {
                byte[] data = Receive(io);
                if (data != null && data.Length > 0)
                {
                    list.Add(data);
                }
            }
            else
            {
                if (receiveFilter == null)
                {
                    byte[] data = Receive(io);
                    if (data != null && data.Length > 0)
                    {
                        list.Add(data);
                    }
                }
                else
                {
                    IList<byte[]> filterData = Receive(io, receiveFilter);
                    list.AddRange(filterData);
                }
            }
            return list;
        }

        /// <summary>
        /// 如果开启StartCheckPackageLength,读取包长数据
        /// </summary>
        /// <param name="io"></param>
        /// <param name="filterBytes"></param>
        /// <returns></returns>
        internal IList<IRequestInfo> InternalReceiveChannelPackageData(IChannel io, IList<byte[]> filterBytes)
        {
            if (filterBytes != null && filterBytes.Count > 0)
            {
                IList<IRequestInfo> requestInfos = new List<IRequestInfo>(filterBytes.Count);
                if (this.Server.ServerConfig.StartCheckPackageLength)
                {
                    bool isReceiveData = false;
                    foreach (byte[] b in filterBytes)
                    {
                        int readTimeout = 1000;
                        int dataLength = 0;
                        try
                        {
                            dataLength = this.Protocol.GetPackageLength(b, io, ref readTimeout);
                        }
                        catch (Exception ex)
                        {
                            this.Server.Logger.Error(true, "", ex);
                        }

                        byte[] bigData=null;
                        if (!isReceiveData)
                        {
                            if (dataLength > 0)
                            {
                                bigData=((BaseChannel) io).ReceivePackageData(dataLength, readTimeout);
                                isReceiveData = true;
                            }
                        }

                        if (bigData != null)
                        {
                            requestInfos.Add(new RequestInfo(io.Key,b,io,bigData));
                        }
                        else
                        {
                            requestInfos.Add(new RequestInfo(io.Key, b, io));
                        }
                    }
                }
                else
                {
                    foreach (byte[] b in filterBytes)
                    {
                        requestInfos.Add(new RequestInfo(io.Key, b, io));
                    }
                }
                return requestInfos;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 运行设备
        /// </summary>
        /// <param name="io"></param>
        public void Run(IChannel io)
        {
            //不运行设备
            if (!this.IsRunDevice)
            {
                OnDeviceRuningLog("设备已经停止运行");
                return;
            }

            if (io == null)
            {
                this.UnknownIO();

                if (this.DeviceDynamic.CommunicateState != CommunicateState.None)
                {
                    this.DeviceDynamic.CommunicateState = CommunicateState.None;
                    this.CommunicateStateChanged(CommunicateState.None);
                }
            }
            else
            {
                //-------------------获得发送数据命令--------------------//
                byte[] data = this.GetSendBytes();

                if (data != null && data.Length > 0)
                {
                    //-------------------发送数据----------------------------//
                    this.Send(io, data);

                    this.ShowMonitorData(data, "发送");

                    this.SaveBytes(data, "发送");
                }

                //---------------------读取数据--------------------------//
                //过滤数据
                IList<byte[]> filterBytes = InternalReceive(io, Protocol.ReceiveFilter);

                //二次读取包长数据
                IList<IRequestInfo> requestInfos = this.InternalReceiveChannelPackageData(io, filterBytes);

                if (requestInfos != null && requestInfos.Count > 0)
                {
                    foreach (IRequestInfo ri in requestInfos)
                    {
                        InternalRun(io.Key, null, ri);
                    }
                }
                else
                {
                    InternalRun(io.Key, null, new RequestInfo(io.Key,new byte[] {},io));
                }
            }
        }

        /// <summary>
        /// 运行设备
        /// </summary>
        /// <param name="key"></param>
        /// <param name="channel"></param>
        /// <param name="requestInfo"></param>
        public void Run(string key, IChannel channel, IRequestInfo requestInfo)
        {
            //不运行设备
            if (!this.IsRunDevice)
            {
                OnDeviceRuningLog("设备已经停止运行");
                return;
            }

            if (requestInfo == null)
            {
                this.UnknownIO();

                if (this.DeviceDynamic.CommunicateState != CommunicateState.None)
                {
                    this.DeviceDynamic.CommunicateState = CommunicateState.None;
                    this.CommunicateStateChanged(CommunicateState.None);
                }
            }
            else
            {
                InternalRun(key, channel, requestInfo);
            }
        }

        private void InternalRun(string key, IChannel io, IRequestInfo info)
        {
            #region
            this.ShowMonitorData(info.Data, "接收");

            this.SaveBytes(info.Data, "接收");

            //---------------------检测通讯状态----------------------//
            CommunicateState state = this.CheckCommunicateState(info.Data);

            if (this.DeviceDynamic.CommunicateState != state)
            {
                this.DeviceDynamic.CommunicateState = state;
                this.CommunicateStateChanged(state);
            }

            if (state == CommunicateState.Communicate)
            {
                this.Communicate(info);
            }
            else if (state == CommunicateState.Interrupt)
            {
                this.CommunicateInterrupt(info);
            }
            else if (state == CommunicateState.Error)
            {
                this.CommunicateError(info);
            }
            else
            {
                this.CommunicateNone();
            }

            this.Alert();

            this.Save();

            this.Show();
            #endregion
        }

        /// <summary>
        /// 通讯正常时调用此函数
        /// </summary>
        /// <param name="info"></param>
        public abstract void Communicate(IRequestInfo info);

        /// <summary>
        /// 通讯中断时调用此函数
        /// </summary>
        /// <param name="info"></param>
        public abstract void CommunicateInterrupt(IRequestInfo info);

        /// <summary>
        /// 通讯干扰时调用此函数
        /// </summary>
        /// <param name="info"></param>
        public abstract void CommunicateError(IRequestInfo info);

        /// <summary>
        /// 通讯未知状态，默认
        /// </summary>
        public abstract void CommunicateNone();

        /// <summary>
        /// 检测通讯状态
        /// </summary>
        /// <param name="revdata"></param>
        /// <returns></returns>
        public CommunicateState CheckCommunicateState(byte[] revdata)
        {
            CommunicateState state = CommunicateState.None;
            if (revdata.Length <= 0)
            {
                state = CommunicateState.Interrupt;
            }
            else
            {
                state = this.Protocol.CheckData(revdata) ? CommunicateState.Communicate : CommunicateState.Error;
            }
            return state;
        }

        /// <summary>
        /// 报警函数，每次调度都会调用
        /// </summary>
        public abstract void Alert();

        /// <summary>
        /// 保存数据，每次调度都会调用
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// 展示
        /// </summary>
        public abstract void Show();

        /// <summary>
        /// 当IO为空的时候，调用此函数接口
        /// </summary>
        public abstract void UnknownIO();

        /// <summary>
        /// 当通讯状态改变的时候调用此函数
        /// </summary>
        /// <param name="comState">状态改变后的通讯状态</param>
        public abstract void CommunicateStateChanged(CommunicateState comState);

        /// <summary>
        /// 通道状态改变
        /// </summary>
        /// <param name="channelState"></param>
        public abstract void ChannelStateChanged(ChannelState channelState);

        ///// <summary>
        ///// 当有网络连接的时候调用此函数
        ///// </summary>
        ///// <param name="ip"></param>
        ///// <param name="port"></param>
        //public abstract void SocketConnect(string ip, int port);

        ///// <summary>
        ///// 当有网络连接断开的时候调用此函数
        ///// </summary>
        ///// <param name="ip"></param>
        ///// <param name="port"></param>
        //public abstract void SocketDisconnect(string ip, int port);

        /// <summary>
        /// 退出软件或平台的宿主程序的时候调用此函数
        /// </summary>
        public abstract void Exit();

        /// <summary>
        /// 删除设备
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// 预留接口，当其他服务调用特定对象。
        /// </summary>
        /// <returns></returns>
        public abstract object GetObject();

        /// <summary>
        /// 是否启动设备时钟，如果为真，则调用定时执行OnRunTimer函数
        /// </summary>
        public bool IsRunTimer
        {
            set
            {
                this._IsRunTimer = value;
                if (this._IsRunTimer)  //
                {
                    this._Timer.Start();
                    this._Timer.Enabled = true;
                }
                else
                {
                    this._Timer.Stop();
                    this._Timer.Enabled = false;
                }
            }
            get
            {
                return this._IsRunTimer;
            }
        }

        /// <summary>
        /// 时钟的定时周期，决定多长时间调用一次OnRunTimer函数
        /// </summary>
        public int RunTimerInterval
        {
            set { this._Timer.Interval = value; }
            get { return (int)this._Timer.Interval; }
        }

        /// <summary>
        /// 时钟定时回调函数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            OnRunTimer();
        }

        /// <summary>
        /// 时钟定时调用的函数，可以重写此函数接口
        /// </summary>
        public virtual void OnRunTimer()
        {

        }

        /// <summary>
        /// 显示上下文菜单
        /// </summary>
        public abstract void ShowContextMenu();

        /// <summary>
        /// 显示IO监视器的窗体
        /// </summary>
        public void ShowMonitorDialog()
        {
            if (_monitorChannelForm != null)
            {
                if (_monitorChannelForm.IsDisposed)
                {
                    _monitorChannelForm = new MonitorChannelForm(this.DeviceParameter.DeviceName);
                    _monitorChannelForm.Show();
                    _monitorChannelForm.Focus();
                }
                else
                {
                    _monitorChannelForm.Show();
                    _monitorChannelForm.Focus();
                }
            }
            else
            {
                _monitorChannelForm = new MonitorChannelForm(this.DeviceParameter.DeviceName);
                _monitorChannelForm.Show();
                _monitorChannelForm.Focus();
            }
        }

        /// <summary>
        /// 在IO监视器上显示byte[]数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="desc"></param>
        public void ShowMonitorData(byte[] data, string desc)
        {
            if (_monitorChannelForm != null && !_monitorChannelForm.IsDisposed)
            {
                string hexString = String.Format("[{0}]:{1}", desc, BinaryUtil.ByteToHex(data));
                _monitorChannelForm.Update(hexString);
            }
        }

        /// <summary>
        /// 设备驱动的临时标签
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 同步对象锁
        /// </summary>
        public object SyncLock
        {
            get { return this._SyncLock; }
        }

        /// <summary>
        /// 实时数据接口
        /// </summary>
        public abstract IDeviceDynamic DeviceDynamic { get; }

        /// <summary>
        /// 参数数据接口
        /// </summary>
        public abstract IDeviceParameter DeviceParameter { get; }

        /// <summary>
        /// 协议驱动接口
        /// </summary>
        public abstract IProtocolDriver Protocol { get; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public abstract DeviceType DeviceType { get; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public abstract string ModelNumber { get; }

        /// <summary>
        /// 设备运行的优先级别，决定是否优先调用设备运行
        /// </summary>
        public DevicePriority DevicePriority { get; set; }

        /// <summary>
        /// 通讯类型
        /// </summary>
        public CommunicateType CommunicateType { get; set; }

        /// <summary>
        /// 是否运行设备
        /// </summary>
        public bool IsRunDevice { set; get; }


        public abstract System.Windows.Forms.Control DeviceGraphics { get; }

        /// <summary>
        /// 发送数据事件
        /// </summary>
        public event SendDataHandler SendData;

        public void OnSendData(byte[] senddata)
        {
            if (this.SendData == null) return;
            SendDataArgs args = null;
            if (this.CommunicateType == CommunicateType.COM)
            {
                args = new SendDataArgs(
                    this.DeviceParameter.DeviceID,
                    this.DeviceParameter.DeviceCode,
                    this.DeviceParameter.DeviceAddr,
                    this.DeviceParameter.DeviceName,
                    this.DeviceParameter.COM.Port,
                    this.DeviceParameter.COM.Baud,
                    senddata);
            }
            else if (this.CommunicateType == CommunicateType.NET)
            {
                args = new SendDataArgs(
                    this.DeviceParameter.DeviceID,
                    this.DeviceParameter.DeviceCode,
                    this.DeviceParameter.DeviceAddr,
                    this.DeviceParameter.DeviceName,
                    this.DeviceParameter.NET.RemoteIP,
                    this.DeviceParameter.NET.RemotePort,
                    senddata);
            }

            this.SendData(this, args);
        }

        /// <summary>
        /// 显示的日志事件
        /// </summary>
        public event DeviceRuningLogHandler DeviceRuningLog;

        public void OnDeviceRuningLog(string statetext)
        {
            if (this.DeviceRuningLog == null) return;

            DeviceRuningLogArgs args = new DeviceRuningLogArgs(
                this.DeviceParameter.DeviceID,
                this.DeviceParameter.DeviceCode,
                this.DeviceParameter.DeviceAddr,
                this.DeviceParameter.DeviceName,
                statetext);

            this.DeviceRuningLog(this, args);
        }

        /// <summary>
        /// 更改串口事件
        /// </summary>
        public event ComParameterExchangeHandler ComParameterExchange;

        public void OnComParameterExchange(int oldcom, int oldbaud, int newcom, int newbaud)
        {
            if (this.ComParameterExchange == null) return;

            ComParameterExchangeArgs args = new ComParameterExchangeArgs(
                this.DeviceParameter.DeviceID,
                this.DeviceParameter.DeviceCode,
                this.DeviceParameter.DeviceAddr,
                this.DeviceParameter.DeviceName,
                this.DeviceParameter.COM.Port,
                this.DeviceParameter.COM.Baud,
                oldcom,
                oldbaud,
                newcom,
                newbaud);

            this.ComParameterExchange(this, args);
        }

        /// <summary>
        /// 对象数据改变事件
        /// </summary>
        public event DeviceObjectChangedHandler DeviceObjectChanged;

        public void OnDeviceObjectChanged(object obj)
        {
            if (this.DeviceObjectChanged == null) return;

            DeviceObjectChangedArgs args = new DeviceObjectChangedArgs(
                this.DeviceParameter.DeviceID,
                this.DeviceParameter.DeviceCode,
                this.DeviceParameter.DeviceAddr,
                this.DeviceParameter.DeviceName,
                obj,
                this.DeviceType);

            this.DeviceObjectChanged.BeginInvoke(this, args, null, null);
        }

        /// <summary>
        /// 虚拟设备运行接口
        /// </summary>
        /// <param name="devid"></param>
        /// <param name="obj"></param>
        public virtual void RunVirtualDevice(string devid, object obj)
        {
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                if (disposing)
                {
                    this._Timer.Close();
                    this._Timer.Dispose();
                    this._Timer = null;
                }

                _IsDisposed = true;
            }
        }

        /// <summary>
        /// 是否释放资源
        /// </summary>
        public bool IsDisposed
        {
            get { return _IsDisposed; }
        }
    }
}
