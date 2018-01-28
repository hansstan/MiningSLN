using System;
using Microsoft.AspNet.SignalR.Client;
using System.Diagnostics;
using System.Threading.Tasks;
using MiningClient.Models;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace MiningClient.SignalR
{
    public class MySignalRComMgr
    {
        IHubProxy hubProxy;
        HubConnection hubConnection;

        /// <summary>
        /// Eventhandler for different datatypes to send & receive data using SignalR
        /// </summary>
        public event EventHandler<MiningData> SignalRMiningDataReceivedHandler;
        public event EventHandler<string> PingReceivedHandler;


        public async Task Init(bool runOnWP)
        {
            try
            {
                hubConnection = new HubConnection(Constants.MiningCenterWebAppUrl);
                hubProxy = hubConnection.CreateHubProxy("MiningHub");

                hubProxy.On<string>("PingToClient", newText =>
                {
                    PingReceivedHandler?.Invoke(this, newText);
                });

                hubProxy.On<MiningData>("SendDataToMiningCenter", newData =>
                {
                    SignalRMiningDataReceivedHandler?.Invoke(this, newData);
                });

                // On Phone still better to use LongPollingTransport? 
                // See http://www.4sln.com/Articles/creating-a-simple-application-using-universal-apps-with-signalr-and-mobile-servic
                if (runOnWP == true)
                    await hubConnection.Start(new LongPollingTransport());
                else
                    await hubConnection.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task PingToServer(string sender, string text)
        {
            try
            {
                if (hubConnection.State == ConnectionState.Connected)
                {
                    await hubProxy.Invoke("PingToServer", sender, text);
                }
                else
                {
                    await HubReconnect();
                }
            }
            catch (Exception)
            {
                //Log.Error("SendTextToServer() failed wit ex:" + ex.Message);
            }
        }

        public async Task SendDataToMiningCenter(MiningData miningData)
        {
            try
            {
                if (hubConnection.State == ConnectionState.Connected)
                {
                    await hubProxy.Invoke("SendDataToMiningCenter", miningData);
                }
                else
                {
                    await HubReconnect();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendDataToMiningCenter() failed wit ex:" + ex.Message);
            }
        }

        private async Task HubReconnect()
        {
            //Log.Error("MySignalRComMgr:HubReconnect() hubConnection.State: " + hubConnection.State + " ... try to Start() again");
            await hubConnection.Start();
            //Log.Trace("MySignalRComMgr:hubConnection.Start() done!");
        }

    }
}

