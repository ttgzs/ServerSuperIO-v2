using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSuperIO.Communicate
{
    public class RequestInfo:IRequestInfo
    {
        public RequestInfo()
        {
            Key = String.Empty;
            Data = null;
            Channel = null;
            BigData = null;
        }

        public RequestInfo(string key, byte[] data, IChannel channel)
        {
            Key = key;
            Data = data;
            Channel = channel;
            BigData = null;
        }

        public RequestInfo(string key, byte[] data, IChannel channel,byte[] bigData):this(key,data,channel)
        {
            BigData = bigData;
        }

        public string Key { get; internal set; }

        public byte[] Data { get; internal set; }

        public IChannel Channel { get; internal set; }

        public byte[] BigData { get; internal set; }
    }
}
