using System;
using System.Collections.Generic;

namespace SRCDSQuery
{
    public enum ChallengeResult : int
    {
        Failed = 0,
        Clear = 1,
        Answer = 2
    }

    public enum EngineType : int
    {
        GoldSource = 0,
        Source = 1
    }

    public enum PacketType : int
    {
        // IN
        S2A_PING = 0x6A,
        S2A_CHALLENGE = 0x41,
        S2A_INFO = 0x49,
        S2A_INFO_GS = 0x6D,
        S2A_PLAYER = 0x44,
        S2A_RULES = 0x45,
        S2A_RCON = 0x6C,

        // OUT
        A2S_PING = 0x69,
        A2S_INFO = 0x54,
        A2S_PLAYER = 0x55,
        A2S_RULES = 0x56,
        A2S_GETCHALLENGE = 0x57,
    }

    public enum RCONIN : int
    {
        ExecuteCommand = 0x02,
        Authenticate = 0x03
    }

    public enum RCONOUT : int
    {
        ResponseValue = 0x00,
        AuthResponse = 0x02
    }

    public enum ServerType
    {
        Dedicated = (byte)'d',
        Local = (byte)'l',
        SourceTV = (byte)'p',
    }

    public enum EnvironmentType
    {
        Windows = (byte)'w',
        Linux = (byte)'l',
    }

    public struct ServerInfo
    {
        public Byte Protocol;
        public String ServerName;
        public String MapName;
        public String Folder;
        public String GameName;
        public Int16 AppID;
        public Byte Players;
        public Byte MaxPlayers;
        public Byte Bots;
        public ServerType Type;
        public EnvironmentType System;
        public Boolean Private;
        public Boolean VACSecure;
        public String Version;
        // For AppID 2400
        public Byte GameMode;
        public Byte WitnessCount;
        public Byte WitnessTime;
        //Extras
        public Byte EDF;
        public Int16 Port;
        public Int64 SteamID;
        public Int16 SpecPort;
        public String SpecName;
        public String Keywords;
        public Int64 GameID;
    }

    public struct PlayerInfo
    {
        public String Name;
        public Int32 Score;
        public Double Online;
    }

    public struct PlayerList
    {
        public Byte Count;
        public List<PlayerInfo> Data;
    }
}
