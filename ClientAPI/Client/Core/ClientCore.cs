using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Util;
namespace Client
{
    public class ClientCore
    {

        /// <summary>
        /// These will all be included in some form of event dispatcher instead of a long list of events
        /// </summary>
        /// <param name="chatmsgPayload"></param>

        
        private bool isInit = false;
        private bool canLog = true;
        //shared variable to handle the current buffer which responsible for sending messages.
        static PacketBuffer stateBuffer;
        /// <summary>
        /// This class does the connects and will provide events
        /// </summary>
        public ClientSocket clientSocket;
        /// <summary>
        /// Need to improve this into a form of callback messaging system
        /// </summary>
        /// <param name="response"></param>
        public void Connect() {clientSocket.Connect(); }

        public bool IsConnected() { return clientSocket.IsConnected(); }

        /// <summary>
        /// Priary events
        /// </summary>
        /// <returns></returns>
        /// 
      
        public delegate void EventLoginResponse(PacketBuffer buffer);
        public delegate void EventChatResponse(PacketBuffer reader);
        public delegate void EventMovementSync(PacketBuffer reader);
        public delegate void EventConnectionResponse(PacketBuffer reader);
         public delegate void EventPacketReceived(PacketBuffer reader);
         public delegate void EventPlayerLogin(PacketBuffer reader);
        public event EventLoginResponse onLoginResponse = new EventLoginResponse((PacketBuffer buffer) => { });
        public event EventChatResponse onChatResponse = new EventChatResponse((PacketBuffer reader) => { });
        public event EventMovementSync onMovementSync = new EventMovementSync((PacketBuffer reader) => { });
        public event EventConnectionResponse onConnectionResponse = new EventConnectionResponse((PacketBuffer reader) => { });
        public event EventPlayerLogin onPlayerLogin = new EventPlayerLogin((PacketBuffer reader) => { });
        //TODO Migrate to this event handling
        public static Dictionary<SpecialResponse, EventPacketReceived> ServerResponse = new Dictionary<SpecialResponse, EventPacketReceived>();
        public static Dictionary<GameHeader, EventPacketReceived> GameResponse = new Dictionary<GameHeader, EventPacketReceived>();
        /// <summary>
        /// Initialize Client with Host and Port// Will need to initalize with more values later.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void InitializeClient(string host, int port)
        {
            Logger.ResetLogFile("SendLog"); //Testing
            clientSocket = new ClientSocket(host, port);
            isInit = true;

            clientSocket.onPacketRecv += OnPacketRecv;
        }
    
        
        /// <summary>
        /// Creates and sends a packet 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="obj"></param>
        public void CreateSendPacket(PacketTypes header,params object[] obj)
        {
          BinaryWriter bw = clientSocket.CreatePacket(header).StartWriting(true);
            for(int i = 0; i < obj.Length; i++)
            {
                bw.WriteObject(obj);
            }
            clientSocket.SendPacket();
        }
        /// <summary>
        /// Creates a packet with a header and returns the buffer
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public PacketBuffer CreatePacket(PacketTypes header)
        {
            return clientSocket.CreatePacket(header);
        }
       /// <summary>
       /// Every packet must be followed by a SendPacket() Call
       /// </summary>
        public void SendPacket()
        {
            clientSocket.SendPacket();
        }
        public void SendPacket(PacketBuffer packet)
        {

        }
        public void OnPacketRecv(PacketBuffer packet)
        {
            ParsePacket(packet);
        }

        // We can toss this in a while loop
        public void RetreivePacketsFromQueue()
        {/*
            Packet packet = null;
            if (clientSocket.PacketQueue.Count != 0)
            {
                while (clientSocket.DequeuePacket(out packet))
                {
                    ParsePacket(packet);
                    //Need to add to this.
                }
            }
          
            return;
          */   Thread thread = new Thread(new ThreadStart(ReceivePacketWorker));
             thread.Start();

        }

   
        void ParsePacket(PacketBuffer packet)
        {
            try
            {


                BinaryReader readPacket = packet.StartReading();
                switch ((PacketTypes)readPacket.ReadByte()) //char byte same shit
                {
                    case PacketTypes.Special: SpecialPacketHandler(packet); break;
                    case PacketTypes.Game: GamePacketHandler(packet); break;

                    //case PacketTypes.BroadCast: break;

                    /*
                    if (packet == null)
                        return;
                    Header header = packet.Header;
                    switch (header.Type)
                    { 
                        case PacketType.CHAT: onChatMessageRecv(packet.Payload.Chatmsgpayload); break;

                        case PacketType.LOGIN: onLogin(packet.Payload.Loginpayload); break;

                        case PacketType.PLAYER_DATA: onPlayerDataRecv(packet.Payload.Playerdatapayload); break;
                    }
                    */
                }
            }
            catch (Exception e)
            {

            }

        }

        void SpecialPacketHandler(PacketBuffer buffer)
        {

            switch((SpecialResponse)buffer.StartReading().ReadByte())
            {
                case SpecialResponse.ConnectionResponse: onConnectionResponse(buffer); break;
                case SpecialResponse.LoginResponse: onLoginResponse(buffer); break;
                case SpecialResponse.PlayerLoginEvent: onPlayerLogin(buffer); break;
                case SpecialResponse.RecieveChat: onChatResponse(buffer); break;
               // case SpecialResponse.LoginResponse: onLoginResponse(reader); break;
            }
        }


        public void GetPacketsFromRemote()
        {
            PacketBuffer incomingBuffer = null;
          
                while (clientSocket.ReceivePacket(out incomingBuffer))
                {
                    ParsePacket(incomingBuffer);
                }
            
        }
        void GamePacketHandler(PacketBuffer buffer)
        {
             switch((GameHeader)buffer.StartReading().ReadByte())
            {
             case GameHeader.Movement: onMovementSync(buffer); break;
             }
        }
        public void ReceivePacketWorker()
        {
            /*
            Packet packet = null;
            while (true)
            {
                if (clientSocket.PacketQueue.Count != 0)
                {
                    try
                    {
                        while (clientSocket.DequeuePacket(out packet))
                        {
                            ParsePacket(packet);
                            //Need to add to this.
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
             * /*/
        }




    }
}
