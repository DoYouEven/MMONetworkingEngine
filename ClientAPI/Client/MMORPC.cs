using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Client
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MMORPC : Attribute
    {
       public byte id;
       public MMORPC(byte rpcID) 
       {
           id = rpcID; 
       }

     
    }

    public struct RPCMethod
    {
        public byte id;
        public object obj;
        public MethodInfo func;
        public ParameterInfo[] parameters;
    }



}
