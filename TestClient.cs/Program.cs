using SRCDSQuery;
using System;

namespace TestClient.cs
{
    class Program
    {
        static void Main (string[] args)
        {
            var hs = "209.222.100.83:27088";
            DateTime Epoch = new DateTime(1970, 1, 1);
            try
            {
                ServerQuery query = new ServerQuery(hs);
                Console.WriteLine("Testing against server: {0}", hs);
                Console.WriteLine("Latency: {0}ms", query.Latency);
                Console.WriteLine("Server Name: {0}", query.ServerInfo.ServerName);
                Console.WriteLine("Game Name: {0}", query.ServerInfo.GameName);
                Console.WriteLine("Players Online: {0}/{1}", query.ServerInfo.Players, query.ServerInfo.MaxPlayers);
                Console.WriteLine("Bots Online: {0}", query.ServerInfo.Bots);
                Console.WriteLine();
                if (query.Players.Data != null)
                {
                    Console.WriteLine("Players: ");
                    foreach (PlayerInfo info in query.Players.Data)
                    {
                        Console.WriteLine("Name: {0} - Score: {1} - Online: {2}", info.Name, info.Score, info.Online);
                    }
                }
                Console.WriteLine();
                if (query.Cvars.Data != null)
                {
                    Console.WriteLine("Cvars: ");
                    foreach (Cvar info in query.Cvars.Data)
                    {
                        Console.WriteLine("Name: {0} - Value: {1}", info.Name, info.Value);
                    }
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
