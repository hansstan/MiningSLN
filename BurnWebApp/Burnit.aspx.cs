using BurnWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BurnWebApp
{
    public partial class Burnit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBurnViaAzureLB_Click(object sender, EventArgs e)
        {
            long byteCount = long.Parse(txtByteCount.Text);
            long count = long.Parse(txtCount.Text);
            GetBucketData("http://LiveUrl", byteCount, count);
        }

        protected void btnBurnViaLocalService_Click(object sender, EventArgs e)
        {
            long byteCount = long.Parse(txtByteCount.Text);
            long count = long.Parse(txtCount.Text);
            GetBucketData("http://LiveUrl", byteCount, count);
        }

        private void GetBucketData(string url, long byteCount, long count)
        {
            var wc = new WebClient();

            for (int i = 0; i < count; i++)
            {
                var retString = wc.DownloadString(url);
                var bucket = JsonConvert.DeserializeObject<Bucket>(retString);
            }
        }
    }
}