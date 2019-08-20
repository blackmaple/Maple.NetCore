using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    public class CArrayPoolFactory<T> : IDisposable
    {
        static ArrayPool<T> BufferPool { get; }

        static CArrayPoolFactory()
        {
            BufferPool = ArrayPool<T>.Create(1024 * 1024, 16);
        }

        private T[] Buffer { get; set; }

        public T[] Rent(int size)
        {
            this.Buffer = BufferPool.Rent(size);
            return this.Buffer;
        }

        public void Dispose()
        {
            BufferPool.Return(this.Buffer, true);
        }
    }
}
