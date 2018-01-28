using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

namespace MiningWebApp.Controllers
{
    public class MiningController : ApiController
    {
        // GET: api/Mining
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Mining/5
        public string Get(string id)
        {
            DateTime dtStart = DateTime.Now;
            int count = 1;// 10;
            string savedPasswordHash = String.Empty;
            for (int i = 0; i < count; i++)
            {
                string password = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr";
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 3000); //10000
                byte[] hash = pbkdf2.GetBytes(20);

                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                savedPasswordHash = Convert.ToBase64String(hashBytes);
                //savedPasswordHash = savedPasswordHash.Replace('\\', '');
                Debug.WriteLine($"Miner: {savedPasswordHash}");
            }
            var millisec = (DateTime.Now - dtStart).TotalMilliseconds;
            Debug.WriteLine($"Finished loop with {count} iterations in {millisec} ms");
            return savedPasswordHash;
        }
    }
}
