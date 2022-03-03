using System.Text.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace MyFirstServer
{
    class Server
    {
        TcpListener Listener;
        static Stack<DB> LocalDB = new Stack<DB>();
        static DB LocalDataBaseObj = new DB();
        public Server(int Port)
        {
            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start();
            while (true)
            {
                new Client(Listener.AcceptTcpClient());
            }
        }
        static public void AddParams(string Data)
        {
            LocalDataBaseObj = JsonSerializer.Deserialize<DB>(Data);
            LocalDB.Push(LocalDataBaseObj);
        }
        static public int GetValSum()
        {
            int sum = 0;
            foreach(DB obj in LocalDB)
            {
                sum += obj.Val;
            }

            return sum;
        }
        ~Server()
        {
            if(Listener != null)
            {
                Listener.Stop();
            }
        }
        static void Main(string[] args)
        {
            new Server(80);
        }
    }
    class DB
    {
        public string Name { get; set; }
        public int Val { get; set; }
    }
}
