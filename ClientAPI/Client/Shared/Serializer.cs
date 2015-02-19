
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
using UnityEngine;

public static class CustomSerializer
{

    static Dictionary<byte, object[]> cachedObject = new Dictionary<byte, object[]>();


    static public object ReadObject(this BinaryReader reader) { return reader.ReadObject(null, 0, null); }
    static object ReadObject(this BinaryReader reader, object obj, int prefix, Type type)
    {
        type = reader.ReadType(out prefix);


        switch (prefix)
        {
            case 0: return null;
            case 1: return reader.ReadBoolean();
            case 2: return reader.ReadByte();
            case 3: return reader.ReadUInt16();
            case 4: return reader.ReadInt32();
            case 5: return reader.ReadUInt32();
            case 6: return reader.ReadSingle();
            case 7: return reader.ReadString();
        }


        return obj;

    }
    static public void WriteObject(this BinaryWriter bw, object obj) { bw.WriteObject(obj, 0,false); }
    static void WriteObject(this BinaryWriter bw, object obj, int prefix, bool typeIsKnown) 
    {
        if (obj == null)
        {
            bw.Write((byte)0);
            return;
        }


        Type type;

        if (!typeIsKnown)
        {
            type = obj.GetType();
            prefix = GetPrefix(type);
        }
        else type = GetType(prefix);

        

       // if (!typeIsKnown) bw.Write((byte)prefix);  //First write the prefix

        switch (prefix)
        {
            case 1: bw.Write((bool)obj); break;
            case 2: bw.Write((byte)obj); break;
            case 3: bw.Write((ushort)obj); break;
            case 4: bw.Write((int)obj); break;
            case 5: bw.Write((uint)obj); break;
            case 6: bw.Write((float)obj); break;
            case 7: bw.Write((string)obj); break;
            case 15: bw.Write((double)obj); break;
            case 16: bw.Write((short)obj); break;
            //case 40 : WriteSpecialObject();
          
        }
    }
    /// <summary>
    /// Read an array of ints
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    /// 
    static object[] GetTempBuffer(int count)
    {
        object[] temp;

        if (!cachedObject.TryGetValue((byte)count, out temp))  //pooling array buffers.
        {
            temp = new object[count];
            cachedObject[(byte)count] = temp;
        }
        return temp;
    }
    static public object[] ReadArray(this BinaryReader reader)
    {
        int count = reader.ReadInt();
        if (count == 0) return null;

        object[] temp = GetTempBuffer(count); //mallocing is expensive, pooling array buffers

        for (int i = 0; i < count; ++i)
            temp[i] = reader.ReadObject();

        return temp;
    }



    /// <summary>
    /// Read an array of 
    /// </summary>
    /// <param name="bw"></param>
    /// <param name="objs"></param>
    static public void WriteArray(this BinaryWriter bw, params object[] objs)
    {
        bw.WriteInt(objs.Length);
        if (objs.Length == 0) return;

        for (int b = 0, bmax = objs.Length; b < bmax; ++b)
            bw.WriteObject(objs[b]);
    }

    static public int ReadInt(this BinaryReader reader)
    {
        int bits = reader.ReadByte();
        if (bits == 255) bits = reader.ReadInt32();
        return bits;
    }

    static public BinaryWriter WriteHeader(this BinaryWriter bw, byte packet)
    {
        bw.Write(packet);
        return bw;
    }
    static public BinaryWriter WriteVector3(this BinaryWriter bw,Vector3 vec3)
    {
        bw.Write(vec3.x);
        bw.Write(vec3.y);
        bw.Write(vec3.z);
        return bw;
    }
    static public Vector3 ReadVector3(this BinaryReader bw)
    {
        return new Vector3(bw.ReadSingle(), bw.ReadSingle(), bw.ReadSingle());


    }
    static public BinaryWriter WriteVector3Short(this BinaryWriter bw, short x, short y, short z)
    {
        bw.Write(x);
        bw.Write(y);
        bw.Write(z);
        return bw;
    }
    static public BinaryWriter WriteVector3(this BinaryWriter bw, float x,float y , float z )
    {
        bw.Write(x);
        bw.Write(y);
        bw.Write(z);
        return bw;
    }
    static public BinaryWriter WriteString(this BinaryWriter bw, string data,bool writePrefix = false)
    {
        if (writePrefix) bw.Write((byte)7);

        bw.Write(data);
        return bw;
    }
    static public BinaryWriter WriteInt(this BinaryWriter bw, int val)
    {
       /* if (val < 255)
        {

           bw.Write((byte)4);
bw.Write((byte)val);
        }
        else bw.Write((byte)255);
        {*/
           
            bw.Write(val);
        //}
        return bw;
    }

    // Helper Functions


    /// <summary>
    /// dont worry about this method for now, just returns true if type can be related to interface.
    /// ///users might pass objects that have common interface.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="interfaceType"></param>
    /// <returns></returns>
    
    static public bool Implements(this Type t, Type interfaceType)
    {
        if (interfaceType == null) return false;
        return interfaceType.IsAssignableFrom(t);
        int x;
    }


    /// <summary>
    /// Gets type baed on prefix
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    static Type GetType(int type)
    {
        switch (type)
        {
            case 1: return typeof(bool);
            case 2: return typeof(byte);
            case 3: return typeof(ushort);
            case 4: return typeof(int);
            case 5: return typeof(uint);
            case 6: return typeof(float);
            case 7: return typeof(string);
                

        }
        return null;
    }
    static int GetPrefix(Type type)
    {
        if (type == typeof(bool)) return 1;
        if (type == typeof(byte)) return 2;
        if (type == typeof(ushort)) return 3;
        if (type == typeof(int)) return 4;
        if (type == typeof(uint)) return 5;
        if (type == typeof(float)) return 6;
        return 255;
    }
    static public Type ReadType(this BinaryReader reader, out int type)  
 
    {
        type = reader.ReadByte();
        return GetType(type);
    }


}

