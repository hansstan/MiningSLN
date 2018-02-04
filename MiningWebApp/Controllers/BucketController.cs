using BurnWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MiningWebApp.Controllers
{
    public class BucketController : ApiController
    {
        // GET: api/Data
        public string Get()
        {
            return "Not used";
        }

        // GET: api/Bucket/1024
        public Bucket Get(long id)
        {
            return new Bucket(id);
        }

        // POST: api/Data
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Data/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Data/5
        public void Delete(int id)
        {
        }
    }
}
