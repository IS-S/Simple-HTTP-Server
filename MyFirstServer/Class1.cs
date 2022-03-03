using System;
using System.Net.Sockets;
using System.Text;

namespace MyFirstServer
{
    class Client
    {
        public Client(TcpClient Client)
        {
            DateTime timestamp = DateTime.Now;
            string response;
            string Str;
            byte[] request = new byte[4046];
            Client.GetStream().Read(request, 0, request.Length);

            Console.WriteLine(Encoding.ASCII.GetString(request).ToString());

            switch(Encoding.ASCII.GetString(request).ToString().Split(' ')[1])
            {
                case "/get_summary":
                    response = "{\"Result\":\"Success\",\"Summ\":\"" + Server.GetValSum() + "\",\"Timestamp\":\""+ timestamp + "\"}";
                    Str = "HTTP/1.1 200 OK\nContent-type: application/json\nContent-Length:" + response.Length.ToString() + "\n\n" + response;
                    break;
                case "/post_data":
                    string jsonReq = Encoding.ASCII.GetString(request).Substring(Encoding.ASCII.GetString(request).ToString().IndexOf('{')).Split("\0")[0];
                    Server.AddParams(jsonReq);
                    response = "{\"Result\":\"Success\"}";
                    Str = "HTTP/1.1 200 OK\nContent-type: application/json\nContent-Length:" + response.Length.ToString() + "\n\n" + response;
                    break;
                default:
                    response = "{\"Result\":\"ERROR\",\"Message\":\"path not found\"}";
                    Str = "HTTP/1.1 400 ERROR\nContent-type: application/json\nContent-Length:" + response.Length.ToString() + "\n\n" + response;
                    break;
            }
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            Client.GetStream().Write(Buffer, 0, Buffer.Length);
            Client.Close();
        }
    }

}

