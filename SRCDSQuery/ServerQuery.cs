using System;
using System.Collections.Generic;

namespace SRCDSQuery
{
    public class ServerQuery
    {
        private DSSocket _sock = null;
        private String _host   = "some.hlds.server";
        private Int32 _port    = 27015;
        private Int32 _cnum    = 0;
        private Int32 pinged   = 0;

        private Int32 latency;

        private ServerInfo info;
        private PlayerList players;

        public ServerInfo ServerInfo
        {
            get
            {
                return this.info;
            }
        }

        public PlayerList Players
        {
            get
            {
                return this.players;
            }
        }

        public Int32 Latency
        {
            get
            {
                return this.latency;
            }
        }

        public ServerQuery (String hoststring)
        {
            try
            {
                int pop;
                if ((pop = hoststring.IndexOf(':')) != -1)
                {
                    _host = hoststring.Substring(0, pop);

                    if (!Int32.TryParse(hoststring.Substring(pop + 1), out _port))
                        throw new Exception("Invalid host string format. Expected hostname:port");
                }
                else _host = hoststring;

                _sock = new DSSocket(_host, _port);

                this.pinged = Environment.TickCount;

                _sock.Send(Packets.GetChallenge());
                _sock.Send(Packets.GetInfo());

                Packet pack;

                while (_sock.OK && _sock.Connected)
                {
                    if ((pack = _sock.Dequeue()) != null)
                    {
                        if (this.latency == 0) this.latency = Environment.TickCount - this.pinged;

                        PacketType type = (PacketType)pack.Read<byte>(4);

                        //check
                        if (type == PacketType.S2A_CHALLENGE)
                        {
                            _cnum = pack.Read<Int32>();
                            _sock.Send(Packets.GetPlayers(_cnum));
                        }

                        else if (type == PacketType.S2A_PLAYER)
                        {
                            this.players = new PlayerList()
                            {
                                Count = pack.Read<Byte>(),
                                Data = new List<PlayerInfo>()
                            };

                            for (int i = 0; i < this.players.Count; i++)
                            {
                                //Int32 index     = pack.Read<Byte>();
                                pack.Skip(1);

                                PlayerInfo info = new PlayerInfo()
                                {
                                    Name = pack.Read<String>(),
                                    Score = pack.Read<Int32>(),
                                    Online = pack.Read<Single>()
                                };

                                this.players.Data.Add(info);
                            }

                            if (this.info.AppID > 0) return;
                        }

                        else if (type == PacketType.S2A_INFO)
                        {
                            this.info = new ServerInfo()
                            {
                                Protocol = pack.Read<byte>(),
                                ServerName = pack.Read<String>(),
                                MapName = pack.Read<String>(),
                                Folder = pack.Read<String>(),
                                GameName = pack.Read<String>(),
                                AppID = pack.Read<Int16>(),
                                Players = pack.Read<Byte>(),
                                MaxPlayers = pack.Read<Byte>(),
                                Bots = pack.Read<Byte>(),
                                Type = (ServerType)pack.Read<Byte>(),
                                System = (EnvironmentType)pack.Read<Byte>(),
                                Private = (Boolean)(pack.Read<Byte>() == 1),
                                VACSecure = (Boolean)(pack.Read<Byte>() == 1)
                            };

                            if (this.info.AppID == 2400)
                            {
                                this.info.GameMode = pack.Read<Byte>();
                                this.info.WitnessCount = pack.Read<Byte>();
                                this.info.WitnessTime = pack.Read<Byte>();
                            }

                            this.info.Version = pack.Read<String>();

                            if (pack.Length > pack.Location)
                            {
                                this.info.EDF = pack.Read<Byte>();

                                if ((this.info.EDF & 0x10) > 0)
                                    this.info.SteamID = pack.Read<Int64>();

                                else if ((this.info.EDF & 0x20) > 0)
                                {
                                    this.info.SpecPort = pack.Read<Int16>();
                                    this.info.SpecName = pack.Read<String>();
                                }
                                else if ((this.info.EDF & 0x40) > 0)
                                    this.info.Keywords = pack.Read<String>();

                                else if ((this.info.EDF & 0x80) > 0)
                                    this.info.Port = pack.Read<Int16>();
                            }

                            if (players.Data != null) return;
                        }
                    }
                }
            }
            catch { }
        }
    }
}
