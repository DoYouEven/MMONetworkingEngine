#define debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using Util;
using System.Threading;
using System.Diagnostics;

namespace Client
{
    public enum State
    {
        Disconnected = 0,
        Connected = 1,
        Verified = 2,
        Timedout = 3
    }
    public class ClientSocket
    {

        #region SocketParameters
        private string _host;
        private int _port;
        Socket _socket;

        #endregion
        /// <summary>
        /// State of the current TCPsocket
        /// </summary>

        public bool EnabledPacketRecvEvent = false;
        public State state = State.Disconnected;
        #region Buffer
        byte[] buffer = new byte[102438400];
        PacketBuffer _receiveBuffer = null;
        int _packetLenght = 0;
        byte[] _temp = new byte[1024];
        #endregion
        public Queue<PacketBuffer> inPacketBuffer = new Queue<PacketBuffer>();
        Queue<PacketBuffer> outPacketBuffer = new Queue<PacketBuffer>();
        static PacketBuffer sendBuffer;
        public delegate void EventPacketRecv(PacketBuffer packet);
        public delegate void EventConnection();
        public event EventPacketRecv onPacketRecv = new EventPacketRecv((PacketBuffer packet) => { });
        public event EventConnection onConnection = new EventConnection(() => { });

        public int currentPacketLenght = 0;
        int nextPacketPosition = 0;
        bool skipPacket = false;
        
        #region DEBUG
        public TFDebugger rate = new TFDebugger();

       
        #endregion

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

        public ClientSocket(string Host, int Port)
        {
            _host = Host;
            _port = Port;
        }

        public bool IsConnected()
        {
            return _socket.Connected;
        }
        public void Connect()
        {
            try
            {


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
                state = State.Connected;
                // SendConnectionPacket();
                client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(OnReceive), client);

            }
            catch (Exception e)
            {
                string error = "Failed to lookup host " + Host + " on port " + Port + ". Reason: " + e.Message;
            }
        }
        public PacketBuffer CreatePacket(PacketTypes header)
        {

            sendBuffer = PacketBuffer.CreatePacket(header);
            return sendBuffer;

        }
        public void SendPacket()
        {



            sendBuffer.EndPacket();

            SendPacketAsync(sendBuffer);
            sendBuffer = null;


        }
        public void SendPacketAsync(PacketBuffer packetBuffer)
        {
            // buffer.MarkAsUsed();

            if (IsConnected())
            {

                lock (outPacketBuffer)
                {

                    outPacketBuffer.Enqueue(packetBuffer);


                    if (outPacketBuffer.Count == 1)
                    {
                        packetBuffer.StartReading();
                        try
                        {
                            //After we intiate begin send another thread can come along an add to the queue, but now its only the call backs that will handle it
                            //Review sendcallback method

#if debug
                            rate.outP++;
                            rate.outBytes += packetBuffer._size;                       
#endif
                            _socket.BeginSend(packetBuffer.buffer, packetBuffer.position, packetBuffer._size, SocketFlags.None, SendCallback, packetBuffer);
                        }
                        catch (System.Exception ex)
                        {
                            /*Todo
                            Send a packet back to myself sayin error occured under packet type of error
                             * Close the connection
                             */
                        }
                    }
                }
            }

        }

        private void SendCallback(IAsyncResult ar)
        {
            int bytes;



            try
            {
                // Socket client = (Socket)ar.AsyncState;
                bytes = _socket.EndSend(ar);
            }
            catch (System.Exception ex)
            {
                bytes = 0;
                return;
            }
            if (bytes <= 0)
            {

            }
            lock (outPacketBuffer)
            {
                //This is reffering to the previous buffer since we can remove of the toSendList
                outPacketBuffer.Dequeue().AddToFreeList();

                if (bytes > 0)
                {
                    // If there is another packet to send out, let's send it
                    PacketBuffer buffer;
                    if (outPacketBuffer.Count() > 0)
                    {
                        buffer = outPacketBuffer.Peek();
                        try
                        {
#if debug
                    
             
#endif
                            _socket.BeginSend(buffer.buffer, buffer.position, buffer.size, SocketFlags.None, new AsyncCallback(SendCallback), buffer);
                        }
                        catch (Exception ex)
                        {
                            //todo-> raise some error with exception info
                        }
                    }
                }

            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                int bytes = clientSocket.EndReceive(ar);
                if (bytes > 0)
                {
                    ProcessBuffer(bytes);

                    _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
                }
                // server is trying to send a response the client doesn't know about
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        /// <summary>
        /// Prcess the buffer upto N bytes
        /// This method can /cannot come in another  
        /// </summary>
        /// <param name="bytes"></param>
        void ProcessBuffer(int bytes)  // We will assume only 1 packet comes in per buffer
        {
#if debug
            rate.inBytesRate+= bytes;

#endif
            try
            {



                if (_receiveBuffer == null)
                {
                    _receiveBuffer = PacketBuffer.Init();
                    _receiveBuffer.StartWriting(false).Write(buffer, 0, bytes);


                }
                else
                {
                    _receiveBuffer.StartWriting(true).Write(buffer, 0, bytes);
                }


                for (int totalRemainingBytes = _receiveBuffer.size - nextPacketPosition; totalRemainingBytes >= 4; )
                {
                    if(totalRemainingBytes <= 4)
                    {

                    }
                    if (_packetLenght ==0)
                    {
                        _packetLenght = _receiveBuffer.PeekInt(nextPacketPosition);
                        
                       
                        if(_packetLenght == -1)
                        {
#if debug
                            rate.inP++;

#endif
                            
                            _receiveBuffer = null;
                            _packetLenght = 0;
                            nextPacketPosition = 0;
                           break;
                        }
                    }


                    totalRemainingBytes = totalRemainingBytes - 4;
                    if (totalRemainingBytes == _packetLenght)
                    {
                        _receiveBuffer.StartReading(nextPacketPosition + 4);
                        
                        lock (inPacketBuffer)
                        {
#if debug
                            rate.inP++;

#endif
                            inPacketBuffer.Enqueue(_receiveBuffer);
                            // onPacketRecv(_receiveBuffer);
                        }

                        _receiveBuffer = null;
                        _packetLenght = 0;
                        nextPacketPosition = 0;
                        break;
                    }

                    else if (totalRemainingBytes > _packetLenght)
                    {
                        try
                        {
                            int lenght = _packetLenght+4;
                            PacketBuffer newRecvPacket = PacketBuffer.Init();

                            newRecvPacket.StartWriting(false).Write(_receiveBuffer.buffer, nextPacketPosition, lenght);
                            newRecvPacket.StartReading(4);
                            lock (inPacketBuffer)
                            {
                                inPacketBuffer.Enqueue(newRecvPacket);
                                // onPacketRecv(_receiveBuffer);
                            }

                            nextPacketPosition += lenght;
                            totalRemainingBytes -= _packetLenght;
                            _packetLenght = 0;
                        }
                        catch (Exception e)
                        {

                        }


                    }


                    else break;
                }
                //Read lenght


            }

                //case 2 handled mroe than ne
            //onPacketRecv(ReceivedPacket);

            catch (Exception e)
            {

            }

        }
        public bool ReceivePacket(out PacketBuffer buffer)
        {


            lock (inPacketBuffer)
            {
                if (inPacketBuffer.Count > 0)
                {
                    buffer = inPacketBuffer.Dequeue();
                    return true;

                }
            }
            buffer = null;
            return false;
        }


        void SendConnectionPacket()
        {
            PacketBuffer inbuffer = CreatePacket(PacketTypes.Special);  //Header


            inbuffer.StartWriting(true).WriteHeader((byte)SpecialResponse.ConnectionResponse);
            inPacketBuffer.Enqueue(inbuffer);
        }

      


    }
}