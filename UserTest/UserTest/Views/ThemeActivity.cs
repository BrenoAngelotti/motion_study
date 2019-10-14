
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using UserTest.Enums;

namespace UserTest.Views
{
    [Activity(Label = "ThemeActivity")]
    public class ThemeActivity : Activity
    {
        RadioGroup RgTheme { get; set; }
        View VwBackground { get; set; }
        View VwSkylineDark { get; set; }
        Button BtnNext { get; set; }

        RelativeLayout RlSelectionIndicator { get; set; }
        bool DarkTheme;

        const int DURATION = 400;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_theme);

            RgTheme = FindViewById<RadioGroup>(Resource.Id.rg_theme);
            RlSelectionIndicator = FindViewById<RelativeLayout>(Resource.Id.rl_selection_indicator);
            VwBackground = FindViewById(Resource.Id.vw_background);
            VwSkylineDark = FindViewById(Resource.Id.vw_skyline_dark);
            BtnNext = FindViewById<Button>(Resource.Id.btn_next);

            RgTheme.CheckedChange += ToggleTheme;

            BtnNext.Click += SetThemeAndProceed;
        }

        private void SetThemeAndProceed(object sender, EventArgs e)
        {
            App.Current.UserData.DarkTheme = DarkTheme;
            var intent = new Intent(this, typeof(EvaluationActivity));
            intent.PutExtra("TASK", (int)ETask.Theme);
            intent.PutExtra("ANSWER", DarkTheme ? GetString(Resource.String.label_dark) : GetString(Resource.String.label_light));

            StartActivity(intent);
        }

        private void ToggleTheme(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            DarkTheme = FindViewById<RadioButton>(Resource.Id.rb_dark).Checked;

            var rotation = 180f * (DarkTheme ? 1 : 0);
            var alpha = DarkTheme ? 1f : 0f;

            var interpolator = new AccelerateDecelerateInterpolator();
            
            if(App.Current.UserData.HasMotion)
            {
                RlSelectionIndicator.Animate()
                    .Rotation(rotation)
                    .SetInterpolator(interpolator)
                    .SetDuration(DURATION)
                    .Start();

                VwBackground.Animate()
                    .Alpha(alpha)
                    .SetInterpolator(interpolator)
                    .SetDuration(DURATION)
                    .Start();

                VwSkylineDark.Animate()
                    .Alpha(alpha)
                    .SetInterpolator(interpolator)
                    .SetDuration(DURATION)
                    .Start();
            }

            VwSkylineDark.Alpha = alpha;
            VwBackground.Alpha = alpha;
            RlSelectionIndicator.Rotation = rotation;
        }
    }
}
