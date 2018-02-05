using BurnWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
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
            GetBucketData("http://miningwebapp-stanglmayr.azurewebsites.net/api/Bucket/" + txtByteCount.Text, byteCount, count);
        }

        protected void btnBurnViaLocalService_Click(object sender, EventArgs e)
        {
            long byteCount = long.Parse(txtByteCount.Text);
            long count = long.Parse(txtCount.Text);
            GetBucketData("http://localhost:53378/api/Bucket/" + txtByteCount.Text, byteCount, count);
        }

        protected void btnBurnViaWcfService_Click(object sender, EventArgs e)
        {
            long byteCount = long.Parse(txtByteCount.Text);
            long count = long.Parse(txtCount.Text);
            GetBucketDataViaWcf("http://wcfbucketcloudservice.cloudapp.net/BucketService.svc", byteCount, count);
        }

        private void GetBucketData(string url, long byteCount, long count)
        {
            var wc = new WebClient();

            DateTime dtStart = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                var retString = wc.DownloadString(url);
                var bucket = JsonConvert.DeserializeObject<Bucket>(retString);
            }
            var elapsed = (DateTime.Now - dtStart).TotalMilliseconds;
            lblStatus.Text = $"Web API: Total-MS: {elapsed}";
        }

        private void GetBucketDataViaWcf(string url, long byteCount, long count)
        {
            var wcfClient = new WcfProxy.BucketServiceClient();
            wcfClient.Endpoint.Address = new EndpointAddress(url);
            
            DateTime dtStart = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                var bucket = wcfClient.GetData((int) byteCount);
            }
            var elapsed = (DateTime.Now - dtStart).TotalMilliseconds;
            lblStatus.Text = $"WCF: Total-MS: {elapsed}";
        }
    }
}