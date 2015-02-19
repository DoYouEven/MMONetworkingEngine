using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
using System.IO;
using System.Threading;
using UnityEngine;
using NXTData;
using System.Reflection;
class Program
{


    public static ClientCore clientCore = new ClientCore();
    public static int PacketCount = 5000;
    public static Player player = new Player();
 
    delegate void LogAction( params object[] args);
    static LogAction RPCTest;
    static void Main(string[] args)
    {
        string host = "188.40.137.215";
        int port = 23323;
       //string host = "99.229.37.128";
      // int port = 9990;
       RPCTest = DoSomething;
       
        RPCTest.Invoke(5, 4);
        byte rpcID;
        RPCTest test = new RPCTest(2);
        RPCTest test2 = new RPCTest(4);
        Type type = typeof(RPCTest);
        int x1 = 5;
        int x2 = 3;
        object[] obj = { 1, 2 };
  
        MethodInfo[] methods = type.GetMethods();
 
        for ( int i =0 ; i < methods.Count(); i++)
        {
            if(methods[i].IsDefined(typeof(MMORPC),out rpcID))
            {
                Console.WriteLine("Registerd Method with RPC ID =  " + rpcID);
                methods[i].Invoke(test, obj);
            }
        }
        Console.WriteLine("Connected to Server [Host = " + host + " ] [Port = " + port + Environment.NewLine);
        clientCore.InitializeClient(host, port);

        clientCore.onMovementSync += OnMovementSync;
        clientCore.onChatResponse += onChatResponse;
        clientCore.onLoginResponse += OnLoginResponse;
        clientCore.Connect();
        Thread thread = new Thread(new ThreadStart(KeyListen));
        thread.Start();
        while (true)
        {
            switch (Console.ReadLine().ToLower())
            {
                case "l": tbLogin_Click(); break;
                case "m": new Thread(new ThreadStart(SpamMovePackets)).Start(); break;
            }
        }




    }
    public static void DoSomething(params object[] parameters)
    {
        Console.WriteLine("Method Invoked" + Environment.NewLine);
    }
    public static void KeyListen()
    {
        while (true)
            clientCore.GetPacketsFromRemote();
    }
    static void OnLoginResponse(PacketBuffer buffer)
    {
        switch ((LoginResponseEC)buffer.StartReading().ReadByte())
        {
            case LoginResponseEC.LoginSuccess: OnLogin(buffer); break;
        }
    }
    static void onChatResponse(PacketBuffer buffer)
    {

        BinaryReader reader = buffer.StartReading();

        Console.WriteLine(">> PlayerID" + reader.ReadInt32() + " : " + reader.ReadString());
        //ChatMsgPayload chatMsgPayload = (ChatMsgPayload)obj;

        //this.rtbChatWindow.AppendText(">> " + chatMsgPayload.Username.ToString() + ": " + chatMsgPayload.Msg.ToString() + Environment.NewLine);

        
    }
    static void OnLogin(PacketBuffer buffer)
    {//TRAINER_DATA		TrainerID (Integer)				Prefab Index(short)	AssetID (Integer)				Username (String) ...				LOCATION    

        BinaryReader reader = buffer.StartReading();
        reader.ReadString();//String Message
        player.ID = reader.ReadInt32();//TrainerID
        reader.ReadInt16(); //Prefab Index
        player.TrainerAssetID = reader.ReadInt32(); //AssetID (Integer
        player.Username = reader.ReadString(); //Username (String

        Pokemon pokeData = new Pokemon();
        SpawnData<System.Object> SpawnData = new SpawnData<System.Object>();
        SpawnData.Data = (System.Object)pokeData;
        player.spawnQueue = new Queue<SpawnData<System.Object>>();
        player.spawnQueue.Enqueue(SpawnData);
        //Pokemon pokeData1 = (Pokemon)player.spawnQueue.Peek();
        Console.WriteLine(">> Player[ID = " + player.ID + "]Username : +" + player.Username + " Has logged on ");
    }
    static void SpamMovePackets()
    {


        for (int i = 0; i < PacketCount; i++)
        {

            try
            {
                // Thread.Sleep(1);

                //PacketBuffer buffer = clientCore.clientSocket.CreatePacket(PacketTypes.Game);  //Header
                //buffer.StartWriting(true).WriteHeader((byte)GameHeader.Movement).WriteInt(player.TrainerAssetID).WriteVector3(i, 5, 5).WriteVector3Short(5, 4, 13);
                PacketBuffer buffer = clientCore.clientSocket.CreatePacket(PacketTypes.Special);  //Header
                buffer.StartWriting(true).WriteHeader((byte)SpecialRequest.SendChat).WriteString(i + "Packet");

                clientCore.clientSocket.SendPacket();
            }
            catch (Exception e)
            {

            }
        }
    }
    static int test = 0;
    public static void OnMovementSync(PacketBuffer buffer)
    {

        BinaryReader reader = buffer.StartReading();
        Console.WriteLine(">> PlayerID:" + reader.ReadInt32() + " : X" + reader.ReadSingle() + " : Y" + reader.ReadSingle() + " : Z" + reader.ReadSingle());

    }

    private static void tbLogin_Click()
    {

        //clientCore.SendPacket(ProtoService.ToPacket[PacketType.LOGIN](tbUsername.Text, tbPassword.Text, tbEmail.Text));
        PacketBuffer buffer = clientCore.clientSocket.CreatePacket(PacketTypes.Special);  //Header
        buffer.StartWriting(true).WriteHeader((byte)SpecialRequest.LoginRequest).WriteString("user23213222345").WriteString("password").WriteString("Email1231212123");
        clientCore.clientSocket.SendPacket();

        // 0 0 00  - 1 - 1 - L -String -L String

    }

}


