using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace ChattingClient
{
    public class Client
    {
        private IPAddress s_Ip;
        private int s_Port;
        private Socket c_Socket;
        private byte[] buffer;
        private bool isConnected;


        public Client()
        {
            s_Ip= IPAddress.Parse("127.0.0.1");
            s_Port = 9999;
            c_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            buffer = new byte[1024];
            isConnected = false;
        }

        public void Connect()
        {

            Console.WriteLine("Connecting... IP: {0} / PORT {1}", s_Ip, s_Port);
            try
            {
                IPEndPoint c_ep = new IPEndPoint(s_Ip, s_Port);

                c_Socket.Connect(c_ep);
                Console.WriteLine("============= Success Connect =============");
                Console.WriteLine("============= If you want exit, you must input \"Exit\" =============");

                isConnected =  true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[Connection Error]" + e.Message);
                isConnected = false;
            }
        }

        public bool GetIsConnected()
        {
            return isConnected;
        }

        public async Task SendMessage()
        {
            string msg = await Console.In.ReadLineAsync();

            byte[] data = Encoding.UTF8.GetBytes(msg);

            await Task.Run(() =>
            {
                try
                {
                    c_Socket.Send(data); //데이터 전송
                    Console.WriteLine("[Send Data]");
                }
                catch (Exception e)
                {
                    Console.WriteLine("[SendError]: " + e.Message);
                }
            });
        }

        public async Task ReceiveMessage()
        {
            await Task.Run(() =>
            {
                try
                {
                    int receiveByte = c_Socket.Receive(buffer);

                    if (receiveByte > 0)
                    {
                        string msg = Encoding.UTF8.GetString(buffer);
                        Console.WriteLine("[Message]: " + msg);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ReceiveError]: " + e.Message);
                }

            });
        }
    }
}
