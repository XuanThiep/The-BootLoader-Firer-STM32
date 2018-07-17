using System.Collections;
using System.Collections.Concurrent;

namespace ThanhNTLibs.DeviceController.Communication.Serial
{
    class SerialPortBuffer
    {
        private ConcurrentQueue<byte> byteQueue = new ConcurrentQueue<byte>();
        public SerialPortBuffer()
        {
        }
        public void PutByte(byte b)
        {
            byteQueue.Enqueue(b);
        }
        public void PutBytes(byte[] b)
        {
            int count = 0;
            if (b != null)
            {
                while (count < b.Length)
                {
                    byteQueue.Enqueue(b[count++]);
                }
            }
        }
        public byte GetFirstByte()
        {
            byte first;
            if (byteQueue.TryPeek(out first))
            {
                return first;
            }
            else
            {
                throw new System.Exception("Cannot GetFirstByte in queue");
            }
        }
        public byte RetrieveByte()
        {
            byte first;
            if (byteQueue.TryDequeue(out first))
            {
                return first;
            }
            else
            {
                throw new System.Exception("Cannot RetriveByte in queue");
            }
        }
        public byte[] RetrieveBytes(int length)
        {
            try
            {
                byte[] bts = new byte[length];
                byte temp;
                int i = 0;
                while (i < length)
                {
                    if (byteQueue.TryDequeue(out temp))
                    {
                        bts[i++] = temp;
                    }
                }
                return bts;
            }
            catch
            {
                return null;
            }
        }
        public int Length
        {
            get { return byteQueue.Count; }
        }
    }
}
