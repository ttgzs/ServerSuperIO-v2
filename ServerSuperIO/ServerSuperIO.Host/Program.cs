using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerSuperIO.Assembly;
using ServerSuperIO.Common;
using ServerSuperIO.Config;
using ServerSuperIO.Device;
using ServerSuperIO.Server;

namespace ServerSuperIO.Host
{
    class Program
    {
        static IServerFactory _serverFactory = null;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            ConsoleUtil.SetConsoleCtrlHandler(new ConsoleUtil.ControlCtrlDelegate(HandlerRoutine), true);
            bool success = true;
            Console.WriteLine("正在初始化服务程序......");
            IObjectBuilder builder = new TypeCreator();
            _serverFactory = new ServerFactory();
            try
            {
                GlobalConfig gc = GlobalConfigTool.Load();
                foreach (ServerSuperIO.Config.Server serverCfg in gc.Servers)
                {
                    IServer server = _serverFactory.CreateServer(serverCfg.ServerConfig);
                    server.AddDeviceCompleted += server_AddDeviceCompleted;
                    server.DeleteDeviceCompleted += server_DeleteDeviceCompleted;
                    server.Start();
                    _serverFactory.AddServer(server);

                    foreach (Config.Device devCfg in serverCfg.Devices)
                    {
                        try
                        {
                            IRunDevice runDev = builder.BuildUp<IRunDevice>(devCfg.AssemblyFile, devCfg.Instance);

                            runDev.DeviceParameter.DeviceID = devCfg.DeviceID;
                            runDev.DeviceDynamic.DeviceID = devCfg.DeviceID;
                            runDev.CommunicateType = devCfg.CommunicateType;
                            runDev.Initialize(devCfg.DeviceID);

                            if (server.AddDevice(runDev) != devCfg.DeviceID)
                            {
                                Console.WriteLine("增加设备：" + devCfg.DeviceID + " 失败！");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                Console.WriteLine(ex.Message);
            }

            if (success)
            {
                Console.WriteLine("初始化服务程序完成");
            }

            while ("exit" == Console.ReadLine())
            {
                _serverFactory.RemoveAllServer();
                break;
            }
        }

        private static bool HandlerRoutine(int ctrlType)
        {
            if (ctrlType == 0 || ctrlType == 2)
            {
                _serverFactory.RemoveAllServer();
            }
            return false;
        }

        private static void server_AddDeviceCompleted(string devid, string devName, bool isSuccess)
        {
            Console.WriteLine(devName + ",增加" + (isSuccess == true ? "成功" : "失败"));
        }

        private static void server_DeleteDeviceCompleted(string devid, string devName, bool isSuccess)
        {
            Console.WriteLine(devName + ",删除" + (isSuccess == true ? "成功" : "失败"));
        }
    }
}
