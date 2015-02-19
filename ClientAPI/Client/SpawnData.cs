using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace NXTData
{
    public enum PrefabType
    {
        Trainer,
        Pokemon
    }
    public class SpawnData<T>
    {
        public int assetID;
        public PrefabType prefabType;
        public Vector3 spawnPosition;
        public T Data;
    }
}
