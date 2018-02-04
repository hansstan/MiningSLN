using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BurnWebApp.Models
{
    public class Bucket
    {
        public int    id;
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