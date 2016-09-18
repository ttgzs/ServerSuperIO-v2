using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSuperIO.Config;

namespace ServerSuperIO.Server
{
    public sealed class Server:SocketServer
    {
        internal Server(IServerConfig config) : base(config)
        {

        }

        public override void Start()
        {
            base.Start();
            Logger.InfoFormat(false, "{0}-{1}", ServerName, "启动服务");
        }

        public override void Stop()
        {
            base.Stop();
            Logger.InfoFormat(false, "{0}-{1}", ServerName, "停止服务...");
        }
    }
}
