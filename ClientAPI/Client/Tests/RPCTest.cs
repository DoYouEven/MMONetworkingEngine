using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Client
{
    public class RPCTest 
    {

        public int x =  5;
        public  RPCTest(int x)
        {
            this.x = x;
        }
        [MMORPC(4)]
        public void RPCtester(int x, int y)
        {
            Console.WriteLine("The Parameters are " + x + "and " + y + ". X = " + x + Environment.NewLine);
        }
    }
}
