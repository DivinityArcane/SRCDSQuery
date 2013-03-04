using SRCDSQuery;
using System;

namespace TestClient.cs
{
    class Program
    {
        static void Main (string[] args)
        {
            var hs = "74.91.123.66:27015";
            try
            {
                ServerQuery query = new ServerQuery(hs);
                Console.WriteLine("Testing against server: {0}", hs);
                Console.WriteLine("Latency: {0}ms", query.Latency);
                Console.WriteLine("Server Name: {0}", query.ServerInfo.ServerName);
                Console.WriteLine("Game Name: {0}", query.ServerInfo.GameName);
                Console.WriteLine("Players Online: {0}/{1}", query.ServerInfo.Players, query.ServerInfo.MaxPlayers);
                Console.WriteLine("Bots Online: {0}", query.ServerInfo.Bots);
                Console.WriteLine("Players: ");
                foreach (PlayerInfo info in query.Players.Data)
                {
                    Console.WriteLine("Name: {0} - Score: {1} - Online: {2}", info.Name, info.Score, info.Online);
                }
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }
            Console.Write(".");
            Console.ReadKey();
        }
    }
}
