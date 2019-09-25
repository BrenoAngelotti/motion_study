
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace UserTest.Views
{
    [Activity(Label = "EvaluationActivity")]
    public class EvaluationActivity : Activity
    {
        TextView TxvSecondQuestion { get; set; }
        Spinner SpnFirstQuestion { get; set; }
        Spinner SpnSecondQuestion { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(App.Current.DarkTheme ? Resource.Style.AppTheme : Resource.Style.AppTheme_Light);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_evaluation);

            TxvSecondQuestion = FindViewById<TextView>(Resource.Id.txv_second_question);
            SpnFirstQuestion = FindViewById<Spinner>(Resource.Id.spn_first_question);
            SpnSecondQuestion = FindViewById<Spinner>(Resource.Id.spn_second_question);
            SetLayout();
        }

        private void SetLayout()
        {
            TxvSecondQuestion.Text = String.Format(GetString(Resource.String.text_second_question), Intent.GetStringExtra("ANSWER").ToUpper());

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.arr_answer_options, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            SpnFirstQuestion.Adapter = adapter;
            SpnSecondQuestion.Adapter = adapter;
        }
    }
}
