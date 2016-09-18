using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestComPackageLength
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort sp=new SerialPort("COM2",9600);
            sp.Open();
            sp.ReadTimeout = 3000;
            sp.WriteTimeout = 3000;
            while (true)
            {
                List<byte> data = new List<byte>();
                //for (int i = 0; i < 10; i++)
                //{
                //    data.Add(0x01);
                //}
                data.AddRange(new byte[] {0x55 ,0xaa ,0x00 ,0x61, 0x43 ,0x7a ,0x00 ,0x00 ,0x43 ,0xb4 ,0x15 ,0x0d });
                List<byte> request = new List<byte>();
                request.Add(0x55);
                request.Add(0xaa);
                request.Add(0x00);
                request.AddRange(BitConverter.GetBytes((int)12));
                request.Add(0x0d);
                sp.Write(request.ToArray(), 0, request.Count);
                Console.WriteLine("发送请求");

                Thread.Sleep(1000);

                byte[] readbuffer=new byte[1024];
                int num = 0;
                try
                {
                    num = sp.Read(readbuffer, 0, readbuffer.Length);
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
                        sp.Write(data.ToArray(), 0, data.Count);
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
