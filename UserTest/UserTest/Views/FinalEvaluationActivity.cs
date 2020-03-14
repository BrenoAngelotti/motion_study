using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace UserTest.Views
{
    [Activity(Label = "FinalEvaluationActivity", NoHistory = true)]
    public class FinalEvaluationActivity : Activity
    {
        Spinner SpnClarityQuestion { get; set; }
        Spinner SpnEnjoyabilityQuestion { get; set; }
        Button BtnFinish { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(App.Current.UserData.DarkTheme ? Resource.Style.AppTheme : Resource.Style.AppTheme_Light);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_final_evaluation);

            SetLayout();
        }

        private void SetLayout()
        {
            SpnClarityQuestion = FindViewById<Spinner>(Resource.Id.spn_clarity_question);
            SpnEnjoyabilityQuestion = FindViewById<Spinner>(Resource.Id.spn_enjoyability_question);
            BtnFinish = FindViewById<Button>(Resource.Id.btn_finish);

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.arr_answer_options, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            SpnClarityQuestion.Adapter = adapter;
            SpnEnjoyabilityQuestion.Adapter = adapter;

            SpnClarityQuestion.ItemSelected += SpnClarityQuestion_ItemSelected;
            SpnEnjoyabilityQuestion.ItemSelected += SpnEnjoyabilityQuestion_ItemSelected;

            BtnFinish.Click += BtnFinish_Click;
        }

        private void SpnClarityQuestion_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            CheckAnswers();
        }

        private void SpnEnjoyabilityQuestion_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            CheckAnswers();
        }

        private void CheckAnswers()
        {
            BtnFinish.Enabled = SpnClarityQuestion.SelectedItemPosition != 0 && SpnEnjoyabilityQuestion.SelectedItemPosition != 0;

            SpnClarityQuestion.Alpha = SpnClarityQuestion.SelectedItemPosition != 0 ? 1f : 0.4f;
            SpnEnjoyabilityQuestion.Alpha = SpnEnjoyabilityQuestion.SelectedItemPosition != 0 ? 1f : 0.4f;
        }

        private void BtnFinish_Click(object sender, EventArgs e)
        {
            SaveAnswers();

            var intent = new Intent(this, typeof(ClosingActivity));
            intent.SetFlags(ActivityFlags.ClearTop);
            StartActivity(intent);
        }

        private void SaveAnswers()
        {
            App.Current.UserData.ClarityQuestion = SpnClarityQuestion.SelectedItemPosition;
            App.Current.UserData.EnjoyabilityQuestion = SpnEnjoyabilityQuestion.SelectedItemPosition;
            App.Current.UserData.FinishedAnswers = true;
            App.SaveData();
        }
    }
}
