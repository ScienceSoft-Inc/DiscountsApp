using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Net;
using Java.Net;
using ScnDiscounts.DependencyInterface;
using Xamarin.Forms;
using Application = Android.App.Application;
using Network = ScnDiscounts.Droid.DependencyInterface.Network.Network;

[assembly: Dependency(typeof(Network))]

namespace ScnDiscounts.Droid.DependencyInterface.Network
{
    /// <summary>
    /// Android <see cref="INetwork" /> implementation.
    /// </summary>
    public class Network : BroadcastMonitor, INetwork
    {
        private Action<NetworkStatus> reachabilityChanged;
        private readonly object lockObject = new object();

        /// <summary>
        /// Internets the connection status.
        /// </summary>
        /// <returns>NetworkStatus.</returns>
        public NetworkStatus InternetConnectionStatus()
        {
            var status = NetworkStatus.NotReachable;

            using (var cm = (ConnectivityManager) Application.Context.GetSystemService(Context.ConnectivityService))
            using (var ni = cm.ActiveNetworkInfo)
            {
                if (ni != null && ni.IsConnectedOrConnecting)
                {
                    var name = ni.TypeName.ToUpper();
                    if (name.Contains("WIFI"))
                    {
                        status = NetworkStatus.ReachableViaWiFiNetwork;
                    }
                    else if (name.Contains("MOBILE"))
                    {
                        status = NetworkStatus.ReachableViaCarrierDataNetwork;
                    }
                    else
                    {
                        status = NetworkStatus.ReachableViaUnknownNetwork;
                    }
                }
            }

            return status;
        }

        /// <summary>
        /// Occurs when [reachability changed].
        /// </summary>
        public event Action<NetworkStatus> ReachabilityChanged
        {
            add
            {
                lock (lockObject)
                {
                    if (reachabilityChanged == null)
                    {
                        Start();
                    }

                    reachabilityChanged += value;
                }
            }

            remove
            {
                lock (lockObject)
                {
                    reachabilityChanged -= value;

                    if (reachabilityChanged == null)
                    {
                        Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified host is reachable.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="timeout">The timeout.</param>
        public Task<bool> IsReachable(string host, TimeSpan timeout)
        {
            return Task.Run(
                () =>
                    {
                        try
                        {
                            var address = InetAddress.GetByName(host);

                            return address != null; // && (address.IsReachable((int)timeout.TotalMilliseconds) || );
                        }
                        catch (UnknownHostException)
                        {
                            return false;
                        }
                    });
        }

        //        public bool CanPing(string host)
        //        {
        //            Process p1 = Java.Lang.Runtime.GetRuntime().Exec(string.Format("ping -c 1 {0}", host));
        //
        //
        //            int returnVal = p1.();
        //            boolean reachable = (returnVal==0);
        //        }

        /// <summary>
        /// Determines whether [is reachable by wifi] [the specified host].
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="timeout">The timeout.</param>
        public async Task<bool> IsReachableByWifi(string host, TimeSpan timeout)
        {
            return InternetConnectionStatus() == NetworkStatus.ReachableViaWiFiNetwork && await IsReachable(host, timeout);
        }

        /// <summary>
        /// This gets called by OS when the <see cref="ConnectivityManager.ConnectivityAction"/> <see cref="Intent"/> fires.
        /// </summary>
        /// <param name="context">Context for the intent.</param>
        /// <param name="intent">Intent information.</param>
        public override void OnReceive(Context context, Intent intent)
        {
            var handler = reachabilityChanged;
            if (handler != null)
            {
                handler(InternetConnectionStatus());
            }
        }

        /// <summary>
        /// <see cref="ConnectivityManager.ConnectivityAction"/> <see cref="Intent"/>
        /// </summary>
        protected override IntentFilter Filter
        {
            get { return new IntentFilter(ConnectivityManager.ConnectivityAction); }
        }
    }
}