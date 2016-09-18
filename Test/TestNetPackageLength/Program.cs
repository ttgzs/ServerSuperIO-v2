using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestNetPackageLength
{
    class Program
    {
        static void Main(string[] args)
        {
           TcpClient tcp=new TcpClient();
            tcp.Connect("127.0.0.1",6699);
            while (true)
            {
                List<byte> data = new List<byte>();
                //for (int i = 0; i < 10; i++)
                //{
                //    data.Add(0x01);
                //}
                int[] lengths = new int[] {12, 14,15,16};
                data.AddRange(new byte[] { 0x55, 0xaa, 0x00, 0x61, 0x43, 0x7a, 0x00, 0x00, 0x43, 0xb4, 0x15, 0x0d });
                List<byte> request=new List<byte>();
                request.Add(0x55);
                request.Add(0xaa);
                request.Add(0x00);
                Random rd=new Random();
                int index=rd.Next(0, 3);
                request.AddRange(BitConverter.GetBytes((int)lengths[index]));
                request.Add(0x0d);
                int count = tcp.Client.Send(request.ToArray(), 0,request.Count,SocketFlags.None);
                Console.WriteLine("发送请求:"+ lengths[index].ToString());

                Thread.Sleep(1000);

                byte[] readbuffer = new byte[1024];
                int num = 0;
                try
                {
                    num = tcp.Client.Receive(readbuffer, 0, readbuffer.Length,SocketFlags.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (num > 0)
                {
                    //返回:OK
                    string ok = System.Text.Encoding.ASCII.GetString(readbuffer, 0, num);
                    Console.WriteLine(ok);
                    if (ok == "ok")
                    {
                        tcp.Client.Send(data.ToArray(), 0, data.Count,SocketFlags.None);
                        Console.WriteLine("发送数据：" + data.Count.ToString());
                    }
                    else
                    {
                        Console.WriteLine("解析返回数据不对");
                    }
                }
                else
                {
                    Console.WriteLine("无返回确认信息");
                }
                Thread.Sleep(5000);
            }
            Console.Read();
        }
    }
}
