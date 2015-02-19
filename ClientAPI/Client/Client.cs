using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Google.ProtocolBuffers;
using testing;
namespace Client
{
    public class Client
    {
        #region SocketParameters
        private string _host;
        private int _port;
        Socket _socket;

        #endregion


        #region Buffer
        byte[] buffer = new byte[1024];
        StateBuffer _receiveBuffer = null;
        int _packetLenght = 0;
        byte[] _temp = new byte[1024];
        #endregion
        public Queue<Header> PacketQueue;


        #region Access modifiers

        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        #endregion

        public Client(string Host, int Port)
        {
            Connect(Host, Port);
        }
        private void Connect(string host, int port)
        {
            try
            {
                Host = host;
                Port = port;

                IPHostEntry entry = Dns.GetHostEntry(Host);
                IPAddress address = entry.AddressList[0];
                IPEndPoint ep = new IPEndPoint(address, Port);

                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.BeginConnect(ep, new AsyncCallback(ConnectCallback), _socket);
            }
            catch (Exception e)
            {
                string error = "Failed to lookup host " + Host + " on port " + Port + ". Reason: " + e.Message;

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["success"] = false;
                dict["error"] = error;

            }

        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(OnReceive), client);

            }
            catch (Exception e)
            {
                string error = "Failed to lookup host " + Host + " on port " + Port + ". Reason: " + e.Message;
            }
        }
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                int bytes = clientSocket.EndReceive(ar);
                _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
                if (bytes > 0)
                {
                    ProcessBuffer(bytes);
                }
                // server is trying to send a response the client doesn't know about
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        void ProcessBuffer(int bytes)
        {
            byte[] mTemp = new byte[1024];
            _receiveBuffer = StateBuffer.Init();
            _receiveBuffer.BeginWriting(false).Write(buffer, 0, bytes);
            _temp = _receiveBuffer.buffer;
            _packetLenght = _receiveBuffer.PeekInt(0);
            Header ReceivedPacket = Header.CreateBuilder().MergeFrom(_receiveBuffer.ReadBytes(_packetLenght)).Build();
            PacketQueue.Enqueue(ReceivedPacket);


        }


    }
}