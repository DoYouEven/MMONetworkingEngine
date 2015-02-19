
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using Client;
/*  ID   Object
 *  1    Pikachu
 *  4    TrainerTX
 */

// Owner-> Asset ID -> Invoke a method
namespace Client
{
   public class MMObject : MonoBehaviour
    {
        public int AssetID { get; protected set; }
        public int OwnerID { get; protected set; }
        public bool IsMine { get { return MMOManager.Instance.OwnerID== OwnerID; } }

        static public Dictionary<int ,MMObject> objList = new Dictionary<int, MMObject>();

        private Dictionary<int, Action<PacketBuffer>> RPCList = new Dictionary<int, Action<PacketBuffer>>();   //Read from from buffer.
       // private Dictionary<int, Action<PacketBuffer>> RPCList = new Dictionary<int, Action<PacketBuffer>>();   
        private Dictionary<byte, MethodInfo> RPCListNew = new Dictionary<byte, MethodInfo>();  
        public Action<Vector3> SyncPositionRPC;


        public ClientCore client;
       void Awake()
        {
            client = MMOManager.Instance.clientCore;
        }


       void IntializeRPCList()
       {
           //MonoBehaviour[] mbs = GetComponentsInChildren<MonoBehaviour>(true); 

           MethodInfo[] methods = typeof(MonoBehaviour).GetMethods(BindingFlags.Public |
				BindingFlags.NonPublic);
          byte rpcID;
           for (int i = 0; i < methods.Length; i++)
           {
              
               if (methods[i].IsDefined(typeof(MMORPC),out rpcID))
               {
                   RPCListNew.Add((byte)rpcID, methods[i]);
                   Console.WriteLine("Registerd Method with RPC ID =  " + rpcID);
               }
           }
       }
         

       /// <summary>
       /// Adds a method to a RPCList indexed by an INT
       /// </summary>
       void AddRPC(int RpcID, Action<PacketBuffer> action)
        {
            RPCList[RpcID] = action;
        }
       static void AddRPC(int AssetID, int RpcID, Action<PacketBuffer> action)
        {
           FindObj(AssetID).AddRPC(RpcID,action);
        }
       

       public void InvokeRPC(byte rpcID, params object[] obj)
       {
           MethodInfo rpcMethod;
           if(RPCListNew.TryGetValue(rpcID, out rpcMethod))
           {
               rpcMethod.Invoke(null, obj);
           }
       }
     
           
#region Legacy
      public void InvokeRPC(int RpcID, PacketBuffer buffer)
       {
           //TODO: Always check if RPC exists first
           RPCList[RpcID].Invoke(buffer); // Call an action with reader as the buffer
           return;
       }
#endregion      
       static public MMObject FindObj(int AssetID)
        { 
           MMObject obj = null;
           objList.TryGetValue(AssetID, out obj); // will return either Obj or nULL
           return obj;
       }
        public void RegisterObj(int AssetID, int OwnerID)
        {
            this.AssetID = AssetID;
            this.OwnerID = OwnerID;
            objList[AssetID] = this;

        }

       /// <summary>
       /// Custom method to handle all translations and rotations
       /// </summary>
       /// <param name="reader"></param>
       public void SyncPosition(PacketBuffer buffer)
        {
            BinaryReader reader = buffer.StartReading();
            Vector3 vec3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            SyncPositionRPC.Invoke(vec3);
        }
       public void UpdatePosition(Vector3 vec3)
       {
           PacketBuffer buffer = client.clientSocket.CreatePacket(PacketTypes.Game);  //Header
           buffer.StartWriting(true).WriteHeader((byte)GameHeader.Movement).WriteInt(AssetID).WriteVector3(vec3).WriteVector3Short(0, 0, 0);
           client.clientSocket.SendPacket();
       }

    }
}
