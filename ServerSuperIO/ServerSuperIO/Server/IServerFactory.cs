using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSuperIO.Config;

namespace ServerSuperIO.Server
{
    public interface IServerFactory
    {
        IServer CreateServer(IServerConfig config);

        void AddServer(IServer server);

        IServer GetServer(string serverName);

        IServer[] GetServers();

        void RemoveServer(string serverName);

        void RemoveAllServer();
    }
}
