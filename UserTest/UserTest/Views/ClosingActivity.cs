using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using UserTest.Helpers;

namespace UserTest.Views
{
    [Activity(Label = "ClosingActivity")]
    public class ClosingActivity : Activity
    {
        CardView CvProgress;
        LinearLayout LlFinal;
        Button BtnStudy;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(App.Current.UserData.DarkTheme ? Resource.Style.AppTheme : Resource.Style.AppTheme_Light);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_closing);

            CvProgress = FindViewById<CardView>(Resource.Id.progress);
            LlFinal = FindViewById<LinearLayout>(Resource.Id.ll_final);
            BtnStudy = FindViewById<Button>(Resource.Id.btn_study);

            BtnStudy.Click += BtnStudy_Click;

            //await SimulateSync();
            await SyncData();
            SwapContent();
        }

        private void BtnStudy_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Intent.ActionView)
                .SetData(Android.Net.Uri.Parse(GetString(Resource.String.link_study)));
            StartActivity(intent);
        }

        private async Task SimulateSync()
        {
            await Task.Delay(3000);
        }

        private void SwapContent()
        {
            CvProgress.Visibility = Android.Views.ViewStates.Gone;
            LlFinal.Visibility = Android.Views.ViewStates.Visible;
        }

        private async Task SyncData()
        {
            if (App.Current.UserData.IsSynced)
                return;

            var connection = Util.HasConnection(this);
            if (connection)
            {
                var collection = await WebServices.IsDataCollectionActive();

                if (App.Current.UserData.IsSynced)
                    return;

                if (collection)
                {
                    var formTest = await WebServices.SendDataToForm();
                    if (!formTest)
                    {
                        Toast.MakeText(this, Resource.String.text_sync_fail, ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, Resource.String.text_sync_success, ToastLength.Long).Show();
                        App.Current.UserData.IsSynced = true;
                        App.SaveData();
                    }
                }
            }
        }
    }
}
