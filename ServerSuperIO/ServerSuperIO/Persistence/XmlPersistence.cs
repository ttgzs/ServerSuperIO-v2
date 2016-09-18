using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSuperIO.Common;

namespace ServerSuperIO.Persistence
{
    public abstract class XmlPersistence:IXmlPersistence
    {
        public void Save<T>(T t)
        {
            SerializeUtil.XmlSerialize<T>(this.SavePath, t);
        }

        public T Load<T>()
        {
            try
            {
                return SerializeUtil.XmlDeserailize<T>(this.SavePath);
            }
            catch
            {
                return (T)Repair();
            }
        }

        public void Delete()
        {
            if (System.IO.File.Exists(this.SavePath))
            {
                System.IO.File.Delete(this.SavePath);
            }
        }

        public abstract string SavePath { get; }

        public abstract object Repair();
    }
}
