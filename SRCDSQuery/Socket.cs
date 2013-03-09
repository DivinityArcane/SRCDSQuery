using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace SRCDSQuery
{
    public class DSSocket
    {
        private Socket _sock         = null;
        private IPEndPoint _ep       = null;
        private byte[] _buffer       = null;
        private Queue<Packet> _queue = null;

        public Boolean OK            = true;

        public Int32 rx_bytes, tx_bytes, rx_packets, tx_packets;
        public Boolean Connected
        {
            get
            {
                try
                {
                    return !(this.OK && this._sock.Poll(1, SelectMode.SelectRead) && this._sock.Available == 0);
                }
                catch { return false; }
            }
        }

        public DSSocket (String host, Int32 port, Double timeout = 5)
        {
            try
            {
                _sock = new Socket(SocketType.Dgram, ProtocolType.Udp);

                if (_sock == null)
                    throw new Exception("Unable to create UDB socket.");

                IPAddress[] ips = Dns.GetHostAddresses(host);
                if (ips.Length == 0)
                    throw new Exception("Unable to obtain IP for host: " + host);

                _ep = new IPEndPoint(ips[0], port);
                _queue = new Queue<Packet>();
                _buffer = new byte[8192];
                rx_bytes = 0;
                tx_bytes = 0;
                rx_packets = 0;
                tx_packets = 0;

                Timer t = new Timer(1000 * timeout);
                t.Elapsed += delegate { OK = false; };
                t.Start();

                _sock.Blocking = false;
                _sock.BeginConnect(_ep, new AsyncCallback(on_connect), null);
            }
            catch { }
        }

        private void on_connect (IAsyncResult result)
        {
            if (_sock == null) return;

            try
            {
                _sock.EndConnect(result);
                _sock.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(on_receive), null);
            }
            catch { }
        }

        private void on_receive (IAsyncResult result)
        {
            if (_sock == null) return;

            try
            {
                int recv;

                if ((recv = _sock.EndReceive(result)) > 0)
                {
                    rx_packets++;
                    rx_bytes += recv;

                    byte[] packet = new byte[recv];
                    Buffer.BlockCopy(_buffer, 0, packet, 0, recv);

                    _queue.Enqueue(new Packet(packet));
                }
            }
            catch { }

            try
            {
                Array.Clear(_buffer, 0, _buffer.Length);
                _sock.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(on_receive), null);
            }
            catch { }
        }

        private void on_sent (IAsyncResult result)
        {
            if (_sock == null) return;

            try
            {
                int sent;

                if ((sent = _sock.EndSend(result)) > 0)
                {
                    tx_packets++;
                    tx_bytes += sent;
                }
            }
            catch { }
        }

        public void Send (byte[] data)
        {
            if (_sock == null) return;

            try
            {
                _sock.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(on_sent), null);
            }
            catch { }
        }

        public Packet Dequeue ()
        {
            if (_queue.Count == 0) return null;
            return _queue.Dequeue();
        }
    }
}
