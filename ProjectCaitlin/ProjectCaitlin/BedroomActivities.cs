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

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using System.Timers;

namespace ProjectCaitlin
{
    [Activity(Label = "BedroomActivities")]
    public class BedroomActivities : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.bedroomActivity);

            TextView timerPrint;
            Timer timer;
            int mins = 0; int secs = 0; int milliseconds = 0;


            Button brush = FindViewById<Button>(Resource.Id.brush);
            Button stop = FindViewById<Button>(Resource.Id.stop);
            timerPrint = FindViewById<TextView>(Resource.Id.timer);
            timer = new Timer();

            brush.Click += delegate
            {
                timer.Interval = 1;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

            };

            stop.Click += delegate
            {
                timer.Stop();
                mins = 0; secs = 0; milliseconds = 0;
                RunOnUiThread(() =>
                {
                    timerPrint.Text = String.Format("{0}:{1:00}:{2:000}", mins, secs, milliseconds);
                });
            };

            void Timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                milliseconds++;
                if (milliseconds >= 1000)
                {
                    secs++;
                    milliseconds = 0;
                }
                if (secs == 59)
                {
                    mins++;
                    secs = 0;
                }
                RunOnUiThread(() =>
                {
                    timerPrint.Text = String.Format("{0}:{1:00}:{2:000}", mins, secs, milliseconds);
                }


                );
            };
        }
    }
}