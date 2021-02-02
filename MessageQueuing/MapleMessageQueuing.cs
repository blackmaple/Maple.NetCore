using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore.MessageQueuing
{
    [Maple.NetCore.CDISingleton(typeof(CMapleMessageQueuing<>))]
    public class CMapleMessageQueuing<T_Class>
    {
        public ConcurrentQueue<T_Class> Queue { get; }

        public CMapleMessageQueuing()
        {
            this.Queue = new ConcurrentQueue<T_Class>();
        }

        public bool TryGet(out T_Class data)
        {
            data = default;
            if (this.Queue.IsEmpty)
            {
                return false;
            }
            return this.Queue.TryDequeue(out data);
        }

        public void Push(T_Class data)
        {
            this.Queue.Enqueue(data);
        }

        public bool Any() => !this.Queue.IsEmpty;
    }
}
