using System;
using System.Runtime.Serialization;

namespace BurnWebApp.Models
{
    [DataContract]
    public class Bucket
    {
        [DataMember]
        public int    id;
        [DataMember]
        public byte[] Coal;

        public Bucket(long bucketSize)
        {
            id = new Random(DateTime.Now.Millisecond).Next();
            Coal = new byte[bucketSize];
            for (int i = 0; i < bucketSize; i++)
            {
                Coal[i] = (byte)i;
            }
        }
    }
}