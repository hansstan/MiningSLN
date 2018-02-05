using BurnWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MiningWebApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WcfBucketService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WcfBucketService.svc or WcfBucketService.svc.cs at the Solution Explorer and start debugging.
    public class WcfBucketService : IWcfBucketService
    {
        public Bucket GetDataUsingDataContract(int size)
        {
            var bucket = new Bucket(size);
            return bucket;
        }
    }
}
