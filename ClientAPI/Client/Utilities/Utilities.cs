using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Util
{
    public static class Utilities
    {
        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
        public static byte[] WrapMessage(byte[] message) //protoPayload
        {
            // Get the length prefix for the message
            byte[] lengthPrefix = BitConverter.GetBytes(Convert.ToInt16(message.Length));
            lengthPrefix.Reverse();
            // Concatenate the length prefix and the message
            byte[] ret = new byte[lengthPrefix.Length + message.Length];
            lengthPrefix.CopyTo(ret, 0);
            message.CopyTo(ret, lengthPrefix.Length);

            return ret;
        }
         public static int ByteLenght(byte[] message) //protoPayload
        {
            return Convert.ToInt32(message.Length);
            // Get the length prefix for the message
          
        }
        public static byte[] ToByteArray(object source)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                return stream.ToArray();
            }
        }
    }
}
