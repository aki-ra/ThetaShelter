using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ThetaShelter
{
    [Activity(Label = "ThetaShelter", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button button;
        Button button2;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            button = FindViewById<Button>(Resource.Id.MyButton);
            button2 = FindViewById<Button>(Resource.Id.MyButton2);


            button.Click += delegate { Connect(); };
            button2.Click += delegate { TakePicture(); };
        }

        private void Connect()
        {
            string sid = Theta.Instance.Connect().ToString();
            button.Text = sid;

        }

        private void TakePicture()
        {
            button2.Text = Theta.Instance.TakePicture();
        }
    }
}

