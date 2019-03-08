using Android.Content;
using Android.Net;

namespace UserTest.Helpers
{
    public static class Util
    {
        public static bool HasConnection(Context context)
        {
            var connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            var test = activeNetworkInfo != null && activeNetworkInfo.IsConnected;
            return test;
        }
    }
}