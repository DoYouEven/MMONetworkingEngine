using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NXTData;
using System.Collections;
namespace Client
{
    public class Player
    {
       
       int id;
       string username;


        public int ID { get { return id; } set { id = value; } }
        public string Username { get { return username; } set { username = value; } }

        public int TrainerAssetID;
        public Trainer trainer;

        //Public List Objects to Spawn 

        public Queue<SpawnData<Object>> spawnQueue; 
        // Asset ID/ Object Prefab Type/ Object Data

        public static void SpawnObject<T>(SpawnData<T> data)
        {

        }

    }
}
