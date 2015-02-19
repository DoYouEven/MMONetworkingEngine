using UnityEngine;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace Client
{
    /// <summary>
    /// This class is responsible at the the highest hierarchy of keeping the game state sync, creating objects etc
    /// </summary>
    public class MMOManager : MonoBehaviour
    {

        #region Debug
        public TFDebugger GetTrafficData { get { return clientCore.clientSocket.rate; } }
        #endregion
        public ClientCore clientCore = new ClientCore();
        static MMOManager instance;
        public enum ObjectType
        {
            Trainer,
            Pokemon
        }
        public GameObject[] Trainers;
        public GameObject[] Pokemon;
        public GameObject[] testObjects;
        public Player player;
        static public Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        public int OwnerID { get { return player.ID; } }


        public static MMOManager Instance { get { return instance; } }

        public bool isConnected { get { return clientCore.IsConnected(); } }
        void Awake()
        {
            clientCore.onMovementSync += OnMovementSync;
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
       void Update()
        {
            if (isConnected)
            {
                clientCore.GetPacketsFromRemote();
            }
        }

        #region PlayerEvents
        public void removePlayer(int id)
        {
            playerList.Remove(id);
        }
        public bool isPlayerRegistered(int id)
        {
            return playerList.ContainsKey(id);
        }
        #endregion

        #region UnityCreationRequests


        void StoreGameState(PacketBuffer buffer)
        {

            //ToDo Store Player Data, Store PokeData

        }
        static public int GetNetworkObject(ObjectType objectType, GameObject go)
        {
            switch (objectType)
            {
                case ObjectType.Trainer: for (int i = 0, imax = instance.Trainers.Length; i < imax; ++i)
                        if (instance.Trainers[i] == go) return i; break;
            }


            //TODO : NetworkObject doesnt exit

            return -1;
        }

        void OnMovementSync(PacketBuffer buffer)
        {
            int assetId = buffer.StartReading().ReadInt32();
            MMObject obj = MMObject.FindObj(assetId);
            if(obj == null)
            {
                InstantiateObject(assetId,buffer);
                return;
            }
            obj.SyncPosition(buffer);

        }

   public GameObject InstantiateObject(int assetID,int ownerID,Vector3 position)
        {

            GameObject go = (GameObject)Instantiate(Trainers[0], position, Quaternion.identity);
            go.GetComponent<MMObject>().RegisterObj(assetID, ownerID);
            return go;

        }
        void InstantiateObject(int assetID,PacketBuffer buffer)
        {
            BinaryReader reader = buffer.StartReading();
            Vector3 vec3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            GameObject go = (GameObject)Instantiate(Trainers[0], vec3, Quaternion.identity);
            go.GetComponent<MMObject>().RegisterObj(assetID, 99);

        }
        void InstantiateObject(int assetID, Vector3 position)
        {

            GameObject go = (GameObject)Instantiate(Trainers[0], position, Quaternion.identity);
            go.GetComponent<MMObject>().RegisterObj(assetID, 99);

        }
        void RequestPlayerDataByAssetID(int AssetID)
        {
            PacketBuffer buffer = clientCore.clientSocket.CreatePacket(PacketTypes.Special);  //Header
            buffer.StartWriting(true).WriteHeader((byte)SpecialRequest.SendChat).WriteString("Packet");

            clientCore.clientSocket.SendPacket();
        }
        static public void RequestCreateNetworkObject(ObjectType objectType, bool isBuffer, GameObject go, BinaryReader reader, params object[] objs)
        {
            if (go != null)
            {
                int index = GetNetworkObject(objectType, go);

                if (instance.clientCore.IsConnected())
                {
                    if (index != -1)
                    {
                        BinaryWriter writer = instance.clientCore.CreatePacket(PacketTypes.Game).StartWriting(true);

                        writer.Write((short)index);
                        writer.WriteArray(objs);
                        instance.clientCore.SendPacket();
                        return;
                    }
                    else
                    {

                    }
                }

                // = BinaryExtensions.CombineArrays(go, objs);
                // UnityTools.ExecuteAll(GetRCCs(), (byte)rccID, objs);
                // UnityTools.Clear(objs);
            }
        }

        public void InstantiateNetworkObject(int ownerID, int index, int objID, BinaryReader reader)
        {
            //GameObject.mObjectOwner = creator;
            GameObject go = null;
            go = testObjects[0];
            go = CreateGameObject(go, reader);
            MMObject obj = go.GetComponent<MMObject>();
            obj.RegisterObj(objID, ownerID);
        }

        /*
        if (index == 40000)
        {
          TODO:  // Load the object from the resources folder
           
        }
        else
        */
        /*
        if (index >= 0 && index < NetworkObjects.Length)
        {
            // Reference the object from the provided list
            go = NetworkObjects[index];
        }
        else
        {
            Debug.LogError("Attempting to create an invalid object. Index: " + index);
            return;
        }

        //GameObjectType gameObjectType =  (GameObjectType)reader.readbyte();
        switch()
        // Create the object
        go = CreateGameObject(go, reader);

        //Based object ID is the ID created from the server
        if (go != null && objID != 0)
        {
            NetworkObject obj = go.GetComponent<NetworkObject>();

            if (obj != null)
            {
                obj.uid = objID;
              //TODO:  obj.Register();
            }
            else
            {
             //TODO
            }
        }
         * */

        static GameObject CreateGameObject(GameObject prefab, BinaryReader reader)
        {
            return Instantiate(prefab) as GameObject;
        }
        /*
                static GameObject SpawnGameObject(GameObject prefab, BinaryReader reader)
                {
                    if (prefab != null)
                    {
                        // The first byte is always the type that identifies what kind of data will follow
                        byte type = reader.ReadByte();

                        if (type == 0)
                        {
                            // Just a plain game object
                            return Instantiate(prefab) as GameObject;
                        }
                        else
                        {
                            // Custom creation function
                            object[] objs = reader.ReadArray(prefab);
                            object retVal;

                            UnityTools.ExecuteFirst(GetRCCs(), (byte)type, out retVal, objs);
                            UnityTools.Clear(objs);

                            if (retVal == null)
                            {
                                Debug.LogError("Instantiating \"" + prefab.name + "\" returned null. Did you forget to return the game object from your RCC?");
                            }
                            return retVal as GameObject;
                        }
                    }
                    return null;
                }


            }*/
        #endregion
        #region Connection
        public void Connect()
        {
            if(!isConnected)
            clientCore.Connect();
        }
        void Connect(string host, int port)
        {
            clientCore.InitializeClient(host, port);
            clientCore.Connect();
        }

        public void OnSceneLoad()
        {
            Connect();
        }
        #endregion

    }
}
