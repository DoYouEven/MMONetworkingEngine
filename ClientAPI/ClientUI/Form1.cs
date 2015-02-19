using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Client;
using Util;
using Google.ProtocolBuffers;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading;
namespace ClientUI
{
    public partial class Form1 : Form
    {
        string host = "188.40.137.215";
        int port = 23323;
        // string host = "127.0.0.1";
      //  int port = 9090;


        public Stopwatch SW1 = new Stopwatch();
        public float TotalPing = 0;
        public int PacketCount = 0;
        //at 20 FPS, i get 2 packets per buffer
        public int framerate = 1;
        Player player = new Player();
        string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        PingOptions options = new PingOptions(64, true);
        byte[] buffer;
        public ClientCore clientCore = new ClientCore();
        delegate void SetTextCallback(BinaryReader reader);
        int tempID;
        public Form1()
        {
            InitializeComponent();
            Ping();
            clientCore.InitializeClient(host, port);
            clientCore.onChatResponse += onChatResponse;
            clientCore.onMovementSync += OnMovementSync;
            clientCore.clientSocket.onConnection += onConnection;
            //clientCore.onPlayerDataRecv += OnPlayerDataRecv;
            clientCore.onLoginResponse += OnLoginResponse;
            rtbChatWindow.Text = ">> Welcome To ECE496 Chatroom\n";
            rtbConsole.Text = ">> Command Line\n";
            rtbReceivedPacket.Text = "<< Packet Receive Trunk\n";
            StartListening();
            Thread thread = new Thread(new ThreadStart(GetPackets));
            thread.Start();

            
        }
       public  void GetPackets()
        {
            while(true)
            {
                clientCore.GetPacketsFromRemote();
            }
        }
        private void btnPing_Click(object sender, EventArgs e)
        {
            Ping();
        }
        void Ping()
        {
            Ping pingSender = new Ping();
            buffer = Encoding.ASCII.GetBytes(data);
            PingReply reply = pingSender.Send("188.40.137.215", 10000, buffer, options);
            tbPing.Text = reply.RoundtripTime.ToString();

        }
        void StartListening()
        {
            clientCore.RetreivePacketsFromQueue();

        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.rtbConsole.AppendText("Connected to Server [Host = " + host  + " ] [Port = " + port + Environment.NewLine);
            clientCore.Connect();
            Logger.LogToFile("Report", "test");
        }
      
        public void onConnection()
        {
            this.rtbConsole.AppendText("Connected to Server [Host = " + host + SW1.ElapsedMilliseconds.ToString() + " ] [Port = " +port  +Environment.NewLine);
        }
        public void onChatResponse(Object obj)
        {

            if (this.rtbConsole.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(onChatResponse);
                this.Invoke(d, new object[] { obj });
            }
            else
            {

                BinaryReader reader = (BinaryReader)obj;
                this.rtbChatWindow.AppendText(">> PlayerID" + reader.ReadInt32() + " : " + reader.ReadString() + Environment.NewLine);
                //ChatMsgPayload chatMsgPayload = (ChatMsgPayload)obj;

                //this.rtbChatWindow.AppendText(">> " + chatMsgPayload.Username.ToString() + ": " + chatMsgPayload.Msg.ToString() + Environment.NewLine);

            }
        }

        public void OnMovementSync(BinaryReader reader)
        {
            if (this.rtbConsole.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(OnMovementSync);
                this.Invoke(d, new object[] { reader });
            }
            else
            {
                SW1.Stop();
                //this.rtbConsole.AppendText(">> EndTime :" + SW1.ElapsedMilliseconds.ToString() + "\n" + Environment.NewLine);
                this.rtbReceivedPacket.AppendText(">> PlayerID:" + reader.ReadInt32() + " : X" + reader.ReadSingle() + " : Y" + reader.ReadSingle() + " : Z" + reader.ReadSingle() + Environment.NewLine);
                SW1.Reset();
                //ChatMsgPayload chatMsgPayload = (ChatMsgPayload)obj;

                //this.rtbChatWindow.AppendText(">> " + chatMsgPayload.Username.ToString() + ": " + chatMsgPayload.Msg.ToString() + Environment.NewLine);

            }
        }
        public void OnLoginResponse(BinaryReader reader)
        {
           switch((LoginResponseEC)reader.ReadByte())
           {
               case LoginResponseEC.LoginSuccess: OnLogin(reader); break ;
           }
        }
        public void OnLogin(Object obj)
        {

            if (this.rtbConsole.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(OnLogin);
                this.Invoke(d, new object[] { obj });
            }
            else
            {


                BinaryReader reader = (BinaryReader)obj;
               // PlayerDataPayload loginPayload = (PlayerDataPayload)obj;

                   this.rtbChatWindow.AppendText(">> You may chat to other players:) " + Environment.NewLine);
                   reader.ReadString();
                   player.ID = reader.ReadInt32();
                   player.Username = reader.ReadString();
                   tempID = reader.ReadInt32();
                   this.rtbReceivedPacket.AppendText(">> Player[ID = " + player.ID + "]Username : +" + player.Username + " Has logged on \n");


            }
        }
        public void OnPlayerDataRecv(Object obj)
        {


            if (this.rtbConsole.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(OnPlayerDataRecv);
                this.Invoke(d, new object[] { obj });
            }
            else
            {

                if (PacketCount > 0)
                {
                    SW1.Stop();
                    TotalPing += SW1.ElapsedMilliseconds;
                    this.tbAveragePing.Text = (TotalPing / PacketCount).ToString() + "ms";
                    this.rtbConsole.AppendText(">> Packet#[" + PacketCount + "] Time taken for round trip is:" + SW1.ElapsedMilliseconds + "\n");
                   // rtbReceivedPacket.AppendText(((PlayerDataPayload)obj).ToString() + Environment.NewLine);
                    SW1.Reset();

                }
                else
                {
                    OnLogin(obj);
                }
                PacketCount++;

            }

            //this.rtbConsole.AppendText(((PlayerDataPayload)obj).ToString()+Environment.NewLine);

            //rtbConsole.AppendText(">> " + playerDataPayload.ToString()+Environment.NewLine);
        }


        private void tbLogin_Click(object sender, EventArgs e)
        {

            //clientCore.SendPacket(ProtoService.ToPacket[PacketType.LOGIN](tbUsername.Text, tbPassword.Text, tbEmail.Text));
            PacketBuffer buffer = clientCore.clientSocket.CreatePacket(PacketTypes.Special);  //Header
            buffer.StartWriting(true).WriteHeader((byte)SpecialRequest.LoginRequest).WriteString(tbUsername.Text).WriteString(tbPassword.Text).WriteString(tbEmail.Text);
            clientCore.clientSocket.SendPacket();

            // 0 0 00  - 1 - 1 - L -String -L String

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            PacketBuffer buffer = clientCore.clientSocket.CreatePacket(PacketTypes.Special);  //Header
            buffer.StartWriting(true).WriteHeader((byte)SpecialRequest.SendChat).WriteString(tbChatBox.Text);
            clientCore.clientSocket.SendPacket();
        }

        private void btnPositionUpdateUP_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(new ThreadStart(SpamMovePackets));
            thread.Start();
          
           // rtbConsole.AppendText("The time to convert one pcket = " + SW1.ElapsedMilliseconds); SW1.Reset();  
        }

        void SpamMovePackets()
        {


            for (int i = 0; i < 5000; i++)
            {

                try
                {
                    //Thread.Sleep(framerate);
                    SW1.Start();
                    PacketBuffer buffer = clientCore.clientSocket.CreatePacket(PacketTypes.Game);  //Header
                    buffer.StartWriting(true).WriteHeader((byte)GameHeader.Movement).WriteInt(tempID).WriteVector3(i, 5, 5).WriteVector3Short(5, 4, 13);
                    clientCore.clientSocket.SendPacket();
                }
                catch (Exception e)
                    {

                    }
            }
        }
        private void tbAveragePing_TextChanged(object sender, EventArgs e)
        {

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnLogTest_Click(object sender, EventArgs e)
        {

          //  PacketBuffer buffer = clientCore.clientSocket.CreatePacket(PacketTypes.BroadCast);  //Header
            //buffer.StartWriting(true).WriteInt(5).WriteInt(5);
           // clientCore.clientSocket.SendPacket();
           
           //}
            /*{
             * Deserialization time
             * Read(buffer)
             * */
            /*
            string creationTime;
            string serializationTime;
            string deserializationTime;
            string UnPackingTime;
            string loginInfo;
            string logData;
            string byteSize;
            string totalTimeOld;
            string totalTimeNew;
            string Winner;
            int winCountOld = 0;
            int winCountNew = 0;
            long cumalativeTimeNew = 0;
            long cumalativeTimeOld = 0;
            byte[] bytes;
            for (int i = 0; i < 1000; i++)
            {
                
                SW1.Reset();
                SW1.Start();
                Packet packet = ProtoService.ToPacket[PacketType.LOGIN]("Reuben ", " My_PASS ", " MyEmail");
                SW1.Stop();
                creationTime = SW1.ElapsedTicks.ToString();
                SW1.Reset();
                SW1.Start();
                using (MemoryStream stream = new MemoryStream())
                {
                    ////Save the person to a stream
                    packet.WriteTo(stream);
                    bytes = stream.ToArray();
                }

                SW1.Stop();
                serializationTime = SW1.ElapsedTicks.ToString();
                SW1.Reset();
                SW1.Start();
                Packet ReceivedPacket = Packet.CreateBuilder().MergeFrom(bytes).Build();
                SW1.Stop();
                deserializationTime = SW1.ElapsedTicks.ToString();
                SW1.Reset();
                SW1.Start();
                LoginPayload loginPayload = ReceivedPacket.Payload.Loginpayload;
                loginInfo = loginPayload.Username + loginPayload.Password + loginPayload.Email;
                SW1.Stop();
                UnPackingTime = SW1.ElapsedTicks.ToString();
                byteSize = ReceivedPacket.SerializedSize.ToString();
                
                totalTimeOld = (Convert.ToInt64(serializationTime) + Convert.ToInt64(deserializationTime) + Convert.ToInt64(creationTime) + Convert.ToInt64(UnPackingTime)).ToString();
                if (i > 0) cumalativeTimeOld = cumalativeTimeOld + (Convert.ToInt64(serializationTime) + Convert.ToInt64(deserializationTime) + Convert.ToInt64(creationTime) + Convert.ToInt64(UnPackingTime));
                logData = "Current: " + PacketType.LOGIN + " Creation Time = [" + creationTime + "] serializationTime = [" + serializationTime + "] Deserialization Time = [" + deserializationTime + "]" + "Unpacking Time = " + UnPackingTime + "]" + "<ByteSize : " + byteSize + " bytes> TrunkData =<" + loginInfo + "> ";
                Logger.LogToFile("ComparePacketStructureLog", logData);
                //****************
                SW1.Reset();
                SW1.Start();
                PacketSpeed packetSpeed = ProtoService.ToLoginSpeed("Reuben ", " My_PASS ", " MyEmail");
                SW1.Stop();
                creationTime = SW1.ElapsedTicks.ToString();
                SW1.Reset();
                SW1.Start();
                using (MemoryStream stream = new MemoryStream())
                {
                    //Save the person to a stream
                    packetSpeed.WriteTo(stream);
                    bytes = stream.ToArray();
                }

                SW1.Stop();
                serializationTime = SW1.ElapsedTicks.ToString();
                SW1.Reset();
                SW1.Start();
                PacketSpeed ReceivedPacketSpeed = PacketSpeed.CreateBuilder().MergeFrom(bytes).Build();
                SW1.Stop();
                deserializationTime = SW1.ElapsedTicks.ToString();
                SW1.Reset();
                SW1.Start();
                loginInfo = ReceivedPacketSpeed.StringList[0] + ReceivedPacketSpeed.StringList[1] + ReceivedPacketSpeed.StringList[2];
                SW1.Stop();
                UnPackingTime = SW1.ElapsedTicks.ToString();
                byteSize = ReceivedPacketSpeed.SerializedSize.ToString();
                totalTimeNew = (Convert.ToInt64(serializationTime) + Convert.ToInt64(deserializationTime) + Convert.ToInt64(creationTime) + Convert.ToInt64(UnPackingTime)).ToString();
                if (i > 0) cumalativeTimeNew = cumalativeTimeNew + (Convert.ToInt64(serializationTime) + Convert.ToInt64(deserializationTime) + Convert.ToInt64(creationTime) + Convert.ToInt64(UnPackingTime));
                logData = "Speed UP: " + PacketTYPE.Login + " Creation Time = [" + creationTime + "] serializationTime = [" + serializationTime + "] Deserialization Time = [" + deserializationTime + "]  Unpacking Time =" + UnPackingTime + "]" + "<ByteSize : " + byteSize + " bytes> TrunkData =<" + loginInfo + ">\n\n";
                Logger.LogToFile("ComparePacketStructureLog", logData);
                if (Convert.ToInt64(totalTimeNew) < Convert.ToInt64(totalTimeOld))
                {
                    Logger.LogToFile("ComparePacketStructureLog", "<Winner = Speedup!> [SpeedUP Total Time = " + totalTimeNew + "] [Current TotalTime = " + totalTimeOld + "]");
                    winCountNew++;
                }
                else
                {
                    Logger.LogToFile("ComparePacketStructureLog", "<Winner = Current!> [SpeedUP Total Time = " + totalTimeNew + "] [Current TotalTime = " + totalTimeOld + "]");
                    winCountOld++;
                }
                Logger.LogToFile("ComparePacketStructureLog", "<---------------------------------------------------------------------------------------------------------------------------->");
            }

            Logger.LogToFile("ComparePacketStructureLog", "Results >> [SpeedUP WON = " + winCountNew + " Rounds - Average Total Time = " + cumalativeTimeNew/999 +"] [Current WON = " + winCountOld + "Rounds - Average Total Time = " + cumalativeTimeOld/999 +"]");
            */     
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PacketBuffer buffer = clientCore.clientSocket.CreatePacket(PacketTypes.Game);  //Header
            buffer.StartWriting(true).WriteHeader((byte)GameHeader.Movement).WriteInt(tempID).WriteVector3(0, 5, 5).WriteVector3Short(0, 4, 13);
            clientCore.clientSocket.SendPacket();
            //framerate++;
        }

       

      


           



    }
}
