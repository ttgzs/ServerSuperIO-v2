using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSuperIO.Service
{
    public abstract class Service:IService
    {
        protected Service()
        {
            
        }

        public abstract string ThisKey { get; }

        public abstract string ThisName { get; }

        public abstract void UpdateDevice(string devid, object obj);

        public abstract void RemoveDevice(string devid);

        public abstract void StartService();

        public abstract void StopService();

        public event ServiceLogHandler ServiceLog;

        protected void OnAppServiceLog(string log)
        {
            if (ServiceLog != null)
            {
                ServiceLog(log);
            }
        }

        public abstract bool IsDisposed { get;}

        public abstract void Dispose();
    }
}
