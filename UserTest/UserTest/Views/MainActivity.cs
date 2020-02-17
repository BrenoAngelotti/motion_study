using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using UserTest.Helpers;
using Android.Views;
using Android.Content;
using System.Linq;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;

namespace UserTest.Views
{
    [Activity(Label="Main", Theme = "@style/AppTheme.OnBoarding", NoHistory = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.ScreenLayout | Android.Content.PM.ConfigChanges.Density | Android.Content.PM.ConfigChanges.UiMode | Android.Content.PM.ConfigChanges.SmallestScreenSize)]
    public class MainActivity : AppCompatActivity
    {

        ViewPager ViewPager;

        PagerAdapter Adapter;

        List<RadioButton> Indicators;

        List<OnBoardingFragment> Fragments;

        Button BtnContinue;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (!App.Current.UserData.OnBoarding)
                NextTask();

            SetContentView(Resource.Layout.activity_main);

            Fragments = new List<OnBoardingFragment>() {
                new OnBoardingFragment(Resource.String.text_onboarding_0, Resource.Drawable.ic_pizza_out),
                new OnBoardingFragment(Resource.String.text_onboarding_1, Resource.Drawable.ic_pizza_out),
                new OnBoardingFragment(Resource.String.text_onboarding_2, Resource.Drawable.ic_pizza_out),
                new OnBoardingFragment(Resource.String.text_onboarding_3, Resource.Drawable.ic_pizza_out),
                new OnBoardingFragment(Resource.String.text_onboarding_4, Resource.Drawable.ic_pizza_out),
                new OnBoardingFragment(Resource.String.text_onboarding_5, Resource.Drawable.ic_pizza_out)
            };

            Indicators = new List<RadioButton> {
                FindViewById<RadioButton>(Resource.Id.intro_indicator_0),
                FindViewById<RadioButton>(Resource.Id.intro_indicator_1),
                FindViewById<RadioButton>(Resource.Id.intro_indicator_2),
                FindViewById<RadioButton>(Resource.Id.intro_indicator_3),
                FindViewById<RadioButton>(Resource.Id.intro_indicator_4),
                FindViewById<RadioButton>(Resource.Id.intro_indicator_5)
            };

            BtnContinue = FindViewById<Button>(Resource.Id.btn_continue);

            ViewPager = FindViewById<ViewPager>(Resource.Id.view_pager);
            Adapter = new OnBoardingAdapter(SupportFragmentManager, Fragments);
            ViewPager.Adapter = Adapter;
            ViewPager.SetCurrentItem(0, true);

            ViewPager.PageSelected += ViewPager_PageSelected;
            BtnContinue.Click += BtnContinue_Click;
        }

        private void BtnContinue_Click(object sender, System.EventArgs e)
        {
            if(ViewPager.CurrentItem < Fragments.Count - 1)
            {
                ViewPager.SetCurrentItem(ViewPager.CurrentItem + 1, true);
            }
            else
            {
                App.DidOnBoarding();
                NextTask();
            }
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            UpdateIndicators(e.Position);
        }

        private async System.Threading.Tasks.Task TestActivity()
        {
            var connection = Util.HasConnection(this);

            if (connection)
            {
                var collection = await WebServices.IsDataCollectionActive();

                if (App.Current.UserData.IsSynced)
                    return;

                if (collection)
                {
                    //coleta ativa
                }
                else
                {
                    //coleta inativa
                }
            }
        }

        void UpdateIndicators(int position)
        {
            foreach(var indicator in Indicators)
            {
                if (Indicators.IndexOf(indicator) == position)
                    indicator.Checked = true;
                else
                    indicator.Checked = false;
            }
        }

        void NextTask()
        {
            var nextTask = App.Current.UserData.Tasks.FirstOrDefault(t => !t.Finished);
            if (nextTask != null)
            {
                if (nextTask.TaskIdentifier == Enums.ETask.Theme)
                {
                    var intent = new Intent(this, typeof(ThemeActivity));
                    StartActivity(intent);
                }
                else
                {
                    var intent = new Intent(this, typeof(TaskActivity));
                    intent.PutExtra("TASK", (int)nextTask.TaskIdentifier);
                    StartActivity(intent);
                }
            }
            else
            {
                StartActivity(new Intent(this, typeof(FinalEvaluationActivity)));
            }
        }
    }

    public class OnBoardingAdapter : FragmentStatePagerAdapter
    {
        List<OnBoardingFragment> Fragments;
        public OnBoardingAdapter(Android.Support.V4.App.FragmentManager manager, List<OnBoardingFragment> fragments) : base(manager)
        {
            Fragments = fragments;
        }

        public override int Count => Fragments.Count;

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return Fragments[position];
        }
    }

    public class OnBoardingFragment : Android.Support.V4.App.Fragment
    {
        public int Text;
        public int Drawable;

        public OnBoardingFragment(int text, int drawable)
        {
            Text = text;
            Drawable = drawable;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_onboarding, container, false);

            rootView.FindViewById<TextView>(Resource.Id.txv_onboarding).Text = Context.GetString(Text);
            rootView.FindViewById<ImageView>(Resource.Id.img_onboarding).SetImageDrawable(Context.GetDrawable(Drawable));

            return rootView;
        }

    }
}