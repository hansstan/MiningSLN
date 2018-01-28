using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiningClient.Models
{
    public class MiningData
    {
        public string Sender { get; set; }
        public string Timestamp { get; set; }

        public double CrunchesPerSecond{ get; set; }
    }
}