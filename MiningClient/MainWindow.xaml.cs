using MiningClient.Models;
using MiningClient.SignalR;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MiningClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string miningWebAppUrl = $"http://{Constants.MiningWebAppBaseUrl}.azurewebsites.net/api/Mining/Start";
        //private string miningWebAppUrl = "http://localhost:53378/";  // for local debugging
        private static string MinerName = Constants.MiningWebAppBaseUrl;// + DateTime.Now.Second;

        MySignalRComMgr _mySignalRComMgr;
        public MainWindow()
        {
            InitializeComponent();
            lblWebAppUrl.Text = miningWebAppUrl;
            lblMiningCenterWebAppUrl.Text = Constants.MiningCenterWebAppUrl;
        }

        private static bool bThreadStop = false;
        private static long globalRequestCounter = 0;
        private static long globalErrorCounter = 0;
        private static DateTime dtThreadStart;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitSignalR();
            _mySignalRComMgr.PingToServer(Constants.MiningWebAppBaseUrl, "Ready for mining...");

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bThreadStop = true;
            Thread.Sleep(1000);
        }
        private void btnStartMining_Click(object sender, RoutedEventArgs e)
        {
            int threadCount = 0;
            Int32.TryParse(txtCount.Text, out threadCount);
            globalRequestCounter = 0;
            ThreadUIMarkAsStarted(true);

            bThreadStop = true; // First make sure, that all threads are stopped from previous operation

            Thread.Sleep(1000);
            bThreadStop = false;
            dtThreadStart = DateTime.Now;
            
            // Start Mining threads
            for (int i = 0; i < threadCount; i++)
            {
                Thread miningThread = new Thread(new ThreadStart(MiningThread));
                miningThread.Start();
            }

            // Start Statistic threads
            Thread statisticThread = new Thread(new ThreadStart(StatisticThread));
            statisticThread.Start();
        }

        private void btnStopMining_Click(object sender, RoutedEventArgs e)
        {
            bThreadStop = true;
            ThreadUIMarkAsStarted(false);
        }

        private void ThreadUIMarkAsStarted(bool threadStarted)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                btnStartMining.IsEnabled = !threadStarted;
                btnStopMining.IsEnabled = threadStarted;
            }));
        }

        private void UpdateUI(int requestPerSecond, long copyOfTotalRequestCount, int secondsSinceThreadStart, string msg)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (lstStatus.Items.Count > 100)
                {
                    lstStatus.Items.RemoveAt(99);
                }
                var time = DateTime.Now.ToString("HH:mm:ss");
                
                if (String.IsNullOrEmpty(msg))
                {
                    msg = $"{time}: {txtCount.Text} threads: / {requestPerSecond} req/sec Total Req: {copyOfTotalRequestCount} / Seconds elapsed: {secondsSinceThreadStart}";
                }
                lstStatus.Items.Insert(0,new ListBoxItem().Content = msg);

                MiningData miningData = new MiningData() { Sender = MinerName, CrunchesPerSecond = requestPerSecond, Timestamp = time };
                _mySignalRComMgr.SendDataToMiningCenter(miningData);
            }));
        }

        private void MiningThread()
        {
            string result = String.Empty;
            WebClient wc = new WebClient();
            int requestCount = 0;
            int errorCount = 0;
            double millisec = 0.0;
            try
            {
                do
                {
                    DateTime dtStart = DateTime.Now;
                    millisec = 0.0;
                    requestCount = 0;
                    errorCount = 0;
                    do
                    {
                        result = wc.DownloadString(miningWebAppUrl);
                        millisec = (DateTime.Now - dtStart).TotalMilliseconds;
                        if (result.Length == 50)
                        {
                            requestCount++;
                        }
                        else
                        {
                            errorCount++;
                        }
                    } while (millisec < 1000.0); // Update global requestCounter only once per Second

                    Interlocked.Add(ref globalRequestCounter, requestCount);
                    Interlocked.Add(ref globalErrorCounter, errorCount);
                    Debug.WriteLine($"{DateTime.Now.Second}: MiningThread #{Thread.CurrentThread.ManagedThreadId} created {requestCount} requests! Errors: {errorCount} length: {result.Length}");
                } while (bThreadStop == false);
            }
            catch (Exception ex)
            {
                var msg = $"Exception in MiningThread: {ex.Message}";
                Debug.WriteLine(msg);
                bThreadStop = true;
                Thread.Sleep(1000);
                ThreadUIMarkAsStarted(false);
                UpdateUI(0, 0, 0, msg);
            }
            Debug.WriteLine($"Miningthread ended...");
        }

        private void StatisticThread()
        {
            int secondsSinceThreadStart = 0;
            try
            {
                do
                {
                    secondsSinceThreadStart = (int)(DateTime.Now - dtThreadStart).TotalSeconds;
                    if (secondsSinceThreadStart > 0)
                    {
                        var copyOfTotalRequestCount = Interlocked.Read(ref globalRequestCounter);
                        var requestPerSecond = (int)(copyOfTotalRequestCount / secondsSinceThreadStart);
                        Debug.WriteLine($"StatisticThread: {requestPerSecond} requests/sec Total Requ: {copyOfTotalRequestCount} Seconds elapsed: {secondsSinceThreadStart}");
                        UpdateUI(requestPerSecond, copyOfTotalRequestCount, secondsSinceThreadStart, String.Empty);
                        Thread.Sleep(1000);
                    }
                } while (bThreadStop == false);
            }
            catch (Exception ex)
            {
                var msg = $"Exception in StatisticThread: {ex.Message}";
                Debug.WriteLine(msg);
                bThreadStop = true;
                Thread.Sleep(1000);
                ThreadUIMarkAsStarted(false);
                UpdateUI(0, 0, 0, msg);
            }
            Debug.WriteLine($"StatisticThread ended...");
        }

        private async void InitSignalR()
        {
            if (_mySignalRComMgr != null)
                return; // not the first navigation cycle?
            _mySignalRComMgr = new MySignalRComMgr();
            _mySignalRComMgr.SignalRMiningDataReceivedHandler += _mySignalRMiningDataReceivedHandler;
            _mySignalRComMgr.PingReceivedHandler += _mySignalRComMgr_PingReceivedHandler;
            await _mySignalRComMgr.Init(false);
        }

        private void _mySignalRComMgr_PingReceivedHandler(object sender, string msg)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                // Got message via SignalR 
                lblPingMsg.Text = DateTime.Now.ToString("HH:mm:ss") + $": Ping from: {msg}";
                Debug.WriteLine(lblPingMsg.Text);
            }));
        }

        private void _mySignalRMiningDataReceivedHandler(object sender, MiningData miningData)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                // Got message from SignalR ==> discard here in client
                Debug.WriteLine($"Got MiningData from {miningData.Sender}");
            }));
        }

    }
}
