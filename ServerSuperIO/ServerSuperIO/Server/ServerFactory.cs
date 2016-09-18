using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerSuperIO.Config;

namespace ServerSuperIO.Server
{
    public class ServerFactory: IServerFactory
    {
        private IList<IServer> _Servers;
        public ServerFactory()
        {
            _Servers=new List<IServer>();
        }
        public IServer CreateServer(IServerConfig config)
        {
            try
            {
                return new Server(config);
            }
            catch
            {
                throw;
            }
        }

        public void AddServer(IServer server)
        {
            if (_Servers.FirstOrDefault(s => s.ServerName == server.ServerName) == null)
            {
                _Servers.Add(server);
            }
            else
            {
                throw new EqualException("ServerName已经存在");
            }
        }

        public IServer GetServer(string serverName)
        {
            return _Servers.FirstOrDefault(s => s.ServerName == serverName);
        }

        public IServer[] GetServers()
        {
            return _Servers.ToArray();
        }

        public void RemoveServer(string serverName)
        {
            IServer server=_Servers.FirstOrDefault(s => s.ServerName == serverName);
            if (server != null)
            {
                try
                {
                    server.Stop();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    _Servers.Remove(server);
                }
            }
        }

        public void RemoveAllServer()
        {
            Parallel.ForEach(_Servers.ToArray(), s =>
            {
                try
                {
                    s.Stop();
                }
                catch
                {
                    // ignored
                }
            });

            this._Servers.Clear();
        }
    }
}
