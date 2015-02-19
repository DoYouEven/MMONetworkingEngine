using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Client
{
    public static class Extensions
    {
        public static bool IsDefined(this MethodInfo method, Type attributeType, out byte rpcID)
        {
            if (method.IsDefined(attributeType, true))
            {

                rpcID = ((MMORPC)method.GetCustomAttributes(attributeType, true)[0]).id;

                return true;
            }
            rpcID = 0;
            return false;
        }
    }
}
