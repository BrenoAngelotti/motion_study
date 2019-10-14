using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using UserTest.Enums;

namespace UserTest.Views
{
    [Activity(Label = "EvaluationActivity", NoHistory = true)]
    public class EvaluationActivity : Activity
    {
        ETask Task { get; set; }
        TextView TxvSecondQuestion { get; set; }
        Spinner SpnFirstQuestion { get; set; }
        Spinner SpnSecondQuestion { get; set; }
        Button BtnNext { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(App.Current.UserData.DarkTheme ? Resource.Style.AppTheme : Resource.Style.AppTheme_Light);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_evaluation);

            Task = (ETask)Intent.GetIntExtra("TASK", 0);
            
            SetLayout();
        }

        private void SetLayout()
        {
            TxvSecondQuestion = FindViewById<TextView>(Resource.Id.txv_second_question);
            SpnFirstQuestion = FindViewById<Spinner>(Resource.Id.spn_first_question);
            SpnSecondQuestion = FindViewById<Spinner>(Resource.Id.spn_second_question);
            BtnNext = FindViewById<Button>(Resource.Id.btn_next);

            TxvSecondQuestion.Text = String.Format(GetString(Resource.String.text_second_question), Intent.GetStringExtra("ANSWER").ToUpper());

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.arr_answer_options, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            SpnFirstQuestion.Adapter = adapter;
            SpnSecondQuestion.Adapter = adapter;
            
            BtnNext.Click += NextTask;
        }

        private void NextTask(object sender, EventArgs e)
        {
            SaveTask();

            var nextTask = App.Current.UserData.Tasks.FirstOrDefault(t => !t.Finished);
            if(nextTask != null)
            {
                var intent = new Intent(this, typeof(TaskActivity));
                intent.PutExtra("TASK", (int) nextTask.TaskIdentifier);
                intent.SetFlags(ActivityFlags.ClearTop);
                StartActivity(intent);
            }
            else
            {
                var intent = new Intent(this, typeof(FinalEvaluationActivity));
                intent.SetFlags(ActivityFlags.ClearTop);
                StartActivity(intent);
            }
        }

        private void SaveTask()
        {
            var task = App.Current.UserData.Tasks.FirstOrDefault(t => t.TaskIdentifier == Task);
            task.FirstQuestion = SpnFirstQuestion.SelectedItemPosition;
            task.SecondQuestion = SpnSecondQuestion.SelectedItemPosition;
            task.Finished = true;
            App.SaveData();
        }
    }
}
