
namespace SRCDSQuery
{
    public class Packets
    {
        public static byte[] GetPing ()
        {
            Packet dpack = new Packet();
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)PacketType.A2S_PING);
            return dpack.Finalize();
        }

        public static byte[] GetInfo ()
        {
            Packet dpack = new Packet();
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)PacketType.A2S_INFO);
            dpack.Write("Source Engine Query");
            return dpack.Finalize();
        }

        public static byte[] GetChallenge ()
        {
            Packet dpack = new Packet();
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)PacketType.A2S_GETCHALLENGE);
            return dpack.Finalize();
        }

        public static byte[] GetPlayers (int chal)
        {
            Packet dpack = new Packet();
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)PacketType.A2S_PLAYER);
            dpack.Write(chal);
            return dpack.Finalize();
        }

        public static byte[] GetRules (int chal)
        {
            Packet dpack = new Packet();
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)0xFF);
            dpack.Write((byte)PacketType.A2S_RULES);
            dpack.Write(chal);
            return dpack.Finalize();
        }
    }
}
