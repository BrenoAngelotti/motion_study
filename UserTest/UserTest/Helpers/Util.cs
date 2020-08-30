using Android.Content;
using Android.Net;
using System;
using System.Linq;

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

        public static string GetTestCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Range(1, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }
    }
}