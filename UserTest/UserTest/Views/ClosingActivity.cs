using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using UserTest.Helpers;

namespace UserTest.Views
{
    [Activity(Label = "ClosingActivity")]
    public class ClosingActivity : Activity
    {
        LinearLayout LlProgress;
        LinearLayout LlFinal;
        LinearLayout LlRetry;
        Button BtnStudy;
        Button BtnRetry;
        TextView TxvFinished;
        TextView TxvTestCode;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(App.Current.UserData.DarkTheme ? Resource.Style.AppTheme : Resource.Style.AppTheme_Light);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_closing);

            LlProgress = FindViewById<LinearLayout>(Resource.Id.ll_progress);
            LlFinal = FindViewById<LinearLayout>(Resource.Id.ll_final);
            LlRetry = FindViewById<LinearLayout>(Resource.Id.ll_retry);
            BtnStudy = FindViewById<Button>(Resource.Id.btn_study);
            BtnRetry = FindViewById<Button>(Resource.Id.btn_retry);
            TxvFinished = FindViewById<TextView>(Resource.Id.txv_finished);
            TxvTestCode = FindViewById<TextView>(Resource.Id.txv_test_code);

            TxvTestCode.Text = String.Format(GetString(Resource.String.text_test_code), App.Current.UserData.TestCode);

            BtnStudy.Click += BtnStudy_Click;
            BtnRetry.Click += BtnRetry_Click;

            await SyncData();
        }

        private async void BtnRetry_Click(object sender, EventArgs e)
        {
            SwapContent(EFinalActivityState.InProgress);
            await SyncData();
        }

        private void BtnStudy_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Intent.ActionView)
                .SetData(Android.Net.Uri.Parse(GetString(Resource.String.link_study)));
            StartActivity(intent);
        }

        private void SwapContent(EFinalActivityState state)
        {
            LlProgress.Visibility = (state == EFinalActivityState.InProgress ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone);
            LlFinal.Visibility = (state == EFinalActivityState.Done ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone);
            LlRetry.Visibility = (state == EFinalActivityState.NoConnection ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone);
        }

        private async Task SyncData()
        {
            if (App.Current.UserData.IsSynced)
            {
                SwapContent(EFinalActivityState.Done);
                return;
            }

            var connection = Util.HasConnection(this);
            if (connection)
            {
                if (App.Current.UserData.IsSynced)
                    return;

                var collection = Intent.GetBooleanExtra("COLLECTION", true);

                if (collection)
                {
                    var formResult = await WebServices.SendDataToForm();
                    if (!formResult)
                    {
                        Toast.MakeText(this, Resource.String.text_sync_fail, ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, Resource.String.text_sync_success, ToastLength.Long).Show();
                        App.Current.UserData.IsSynced = true;
                        App.SaveData();
                        SwapContent(EFinalActivityState.Done);
                    }
                }
                else
                {
                    TxvFinished.Visibility = Android.Views.ViewStates.Visible;
                    TxvTestCode.Visibility = Android.Views.ViewStates.Gone;
                }
            }
            else
            {
                SwapContent(EFinalActivityState.NoConnection);
            }
        }
    }

    enum EFinalActivityState
    {
        NoConnection,
        InProgress,
        Done
    }
}
