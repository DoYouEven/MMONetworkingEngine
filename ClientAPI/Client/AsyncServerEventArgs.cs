using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AsyncEvents
{
    public class AsyncServerEventArgs<T> : AsyncCompletedEventArgs
    {
        private T result;
        public AsyncServerEventArgs(T result, Exception e, bool cancelled, object State)
            : base(e, cancelled, State)
        {
            this.result = result;
        }
        public AsyncServerEventArgs(T result)
            : this(result, null, false, null)
        {

        }


    }
}
