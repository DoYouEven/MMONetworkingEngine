using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Client
{
    public class TFDebugger
    {
        public int outP;
        public int inP ;
        public int inRate ;
        public int outRate;
        public int inBytes;
        public int outBytes;
        public int inBytesRate;
        public int outBytesRate;

        bool ConsoleWrite = false;
        public bool EnableWriteConsole {set{ConsoleWrite = value;}}
        public TFDebugger()
        {
            outP = inP = inRate = outRate = inBytes = outBytes = 0;
            Thread thread = new Thread(new ThreadStart(CalculateTransferRates));
            thread.Start();
        }

     
       
        void CalculateTransferRates()
        {

            while (true)
            {
                Stopwatch stop = new Stopwatch();
                if (!stop.IsRunning)
                    stop.Start();
                if (stop.ElapsedMilliseconds > 1000)
                {
                    inRate = inP;
                    outRate = outP;
                    inBytesRate = inBytes;
                    outBytesRate = outBytes;
                    stop.Reset();
                    inP = outP = inBytes = outBytes = 0;
                }

            }
        }
    }
}
