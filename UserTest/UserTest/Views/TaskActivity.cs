using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using UserTest.Enums;

namespace UserTest.Views
{
    [Activity(Label = "TaskActivity")]
    public class TaskActivity : Activity
    {
        ETask Task { get; set; }

        RadioGroup RadioGroup { get; set; }
        RadioButton RbFirstOption { get; set; }
        RadioButton RbSecondOption { get; set; }
        Button BtnNext { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(App.Current.UserData.DarkTheme ? Resource.Style.AppTheme : Resource.Style.AppTheme_Light);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_task);

            Task = (ETask)Intent.GetIntExtra("TASK", 0);

            SetLayout();
            SetOptions();

        }

        private void SetLayout()
        {
            RadioGroup = FindViewById<RadioGroup>(Resource.Id.rg_task);
            RbFirstOption = FindViewById<RadioButton>(Resource.Id.rb_first_option);
            RbSecondOption = FindViewById<RadioButton>(Resource.Id.rb_second_option);
            BtnNext = FindViewById<Button>(Resource.Id.btn_next);

            BtnNext.Enabled = false;

            RadioGroup.CheckedChange += RadioGroup_CheckedChange;
            BtnNext.Click += BtnNext_Click;
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(EvaluationActivity));
            intent.PutExtra("TASK", (int)Task);
            intent.PutExtra("ANSWER", FindViewById<RadioButton>(RadioGroup.CheckedRadioButtonId).Text);
            intent.SetFlags(ActivityFlags.ClearTop);

            StartActivity(intent);
        }

        private void RadioGroup_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            BtnNext.Enabled = true;
        }

        private void SetOptions()
        {
            switch (Task)
            {
                case ETask.GameBoard:
                    SetOption(
                        RbFirstOption,
                        Resource.String.label_game,
                        Resource.Drawable.toggle_game,
                        Resource.Drawable.toggle_avd_game);
                    SetOption(
                        RbSecondOption,
                        Resource.String.label_board,
                        Resource.Drawable.toggle_board,
                        Resource.Drawable.toggle_avd_board);
                    break;
                case ETask.CatDog:
                    SetOption(
                        RbFirstOption,
                        Resource.String.label_dog,
                        Resource.Drawable.toggle_dog,
                        Resource.Drawable.toggle_avd_dog);
                    SetOption(
                        RbSecondOption,
                        Resource.String.label_cat,
                        Resource.Drawable.toggle_cat,
                        Resource.Drawable.toggle_avd_cat);
                    break;
                case ETask.PizzaPasta:
                    SetOption(
                        RbFirstOption,
                        Resource.String.label_pizza,
                        Resource.Drawable.toggle_pizza,
                        Resource.Drawable.toggle_avd_pizza);
                    SetOption(
                        RbSecondOption,
                        Resource.String.label_pasta,
                        Resource.Drawable.toggle_pasta,
                        Resource.Drawable.toggle_avd_pasta);
                    break;
                case ETask.SingPlay:
                    SetOption(
                        RbFirstOption,
                        Resource.String.label_sing,
                        Resource.Drawable.toggle_sing,
                        Resource.Drawable.toggle_avd_sing);
                    SetOption(
                        RbSecondOption,
                        Resource.String.label_play,
                        Resource.Drawable.toggle_play,
                        Resource.Drawable.toggle_avd_play);
                    break;
                default:
                    SetOption(
                        RbFirstOption,
                        Resource.String.label_dark,
                        Resource.Drawable.toggle_test,
                        Resource.Drawable.toggle_avd_test);
                    SetOption(
                        RbSecondOption,
                        Resource.String.label_light,
                        Resource.Drawable.toggle_test,
                        Resource.Drawable.toggle_avd_test);
                    break;
            }
        }

        private void SetOption(RadioButton radioButton, int textId, int toggleId, int motionToggleId)
        {
            radioButton.Text = GetString(textId);
            if (App.Current.UserData.HasMotion)
                radioButton.SetCompoundDrawablesWithIntrinsicBounds(null, GetDrawable(motionToggleId), null, null);
            else
                radioButton.SetCompoundDrawablesWithIntrinsicBounds(null, GetDrawable(toggleId), null, null);
        }
    }
}
