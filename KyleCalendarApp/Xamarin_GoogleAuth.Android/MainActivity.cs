using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Xamarin_GoogleAuth.Authentication;
using Xamarin_GoogleAuth.Services;
using Xamarin_GoogleAuth;

namespace Xamarin_GoogleAuth.Droid
{
    [Activity(Label = "Xamarin_GoogleAuth", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IGoogleAuthenticationDelegate
    {
        // Need to be static because we need to access it in GoogleAuthInterceptor for continuation
        public static GoogleAuthenticator Auth;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Auth = new GoogleAuthenticator(Configuration.ClientId, Configuration.Scope, Configuration.RedirectUrl, this);

            var googleLoginButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            var getCalendarsButton = FindViewById<Button>(Resource.Id.getCalendarsButton);
            var getEventsListButton = FindViewById<Button>(Resource.Id.getEventsListButton);
            var getFirebaseActivitiesButton = FindViewById<Button>(Resource.Id.getFirebaseActivitiesButton);
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
            getCalendarsButton.Click += OnGetCalendarsButtonClicked;
            getEventsListButton.Click += GetEventsListButtonClicked;
            getFirebaseActivitiesButton.Click += GetFirebaseActivitiesButtonClicked;
       }



        //------------------------------- AUTHENTICATE

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            // Display the activity handling the authentication
            var authenticator = Auth.GetAuthenticator();
            var intent = authenticator.GetUI(this);
            StartActivity(intent);
        }

        public async void OnAuthenticationCompleted()
        {

            // Display it on the UI
            var googleButton = FindViewById<Button>(Resource.Id.googleLoginButton);

            new AlertDialog.Builder(this)
                           .SetTitle("Success")
                           .SetMessage("You are now logged in")
                           .Show();

            googleButton.Text = $"Log in as different user";
        }

        public void OnAuthenticationCanceled()
        {
            new AlertDialog.Builder(this)
                           .SetTitle("Authentication canceled")
                           .SetMessage("You didn't completed the authentication process")
                           .Show();
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            new AlertDialog.Builder(this)
                           .SetTitle(message)
                           .SetMessage(exception?.ToString())
                           .Show();
        }


        //---------------------------- GET CALENDARS

        public void OnGetCalendarsButtonClicked(object sender, EventArgs e)
        {
            OnGetCalendarsCompleted();
        }

        public async void OnGetCalendarsCompleted()
        {
            // Retrieve the user's active calendars
            var googleService = new GoogleService();
            var calendars = await googleService.GetCalendars();
            if (calendars == null)
            {
                new AlertDialog.Builder(this)
                               .SetTitle("Oops!")
                               .SetMessage("Please login before accessing your calendars")
                               .Show();
            }
            System.Diagnostics.Debug.WriteLine(calendars);
        }

        //------------------------------- GET FIREBASE INFO

        public void GetFirebaseActivitiesButtonClicked(object sender, EventArgs e)
        {
            OnGetFirebaseActivitiesCompleted();
        }

        public async void OnGetFirebaseActivitiesCompleted()
        {
            // Retrieve the user's active calendars
            var firebaseService = new FirebaseService();
            var activities = await firebaseService.GetActivities();
            if (activities == (null))
            {
                new AlertDialog.Builder(this)
                               .SetTitle("Oops!")
                               .SetMessage("Please login before listing you events")
                               .Show();
            }
            System.Diagnostics.Debug.WriteLine(activities);
        }



            //------------------------------- GET EVENTS

            public void GetEventsListButtonClicked(object sender, EventArgs e)
        {
            OnGetEventsListCompleted();
        }

        public async void OnGetEventsListCompleted()
        {
            // Retrieve the user's active calendars
            var googleService = new GoogleService();
            var events = await googleService.GetEventsList();
            if (events == (null))
            {
                new AlertDialog.Builder(this)
                               .SetTitle("Oops!")
                               .SetMessage("Please login before listing you events")
                               .Show();
            }
            System.Diagnostics.Debug.WriteLine(events);


            //var eventButton = FindViewById<Button>(Resource.Id.Event1);
            //var title = "";
            //var startTime = "";
            //var endTime = "";
            //for (int i = 0; i < events.Length - 2; i++)
            //{
            //    if (events.Substring(i, 1) == "," && title == "")
            //    {
            //        title = events.Substring(0, i);
            //    }
            //    else if (startTime == "" && events.Substring(i, 2) == "PM")
            //    {
            //        startTime = events.Substring(i - 8, 8);
            //    }
            //    else if (endTime == "" && events.Substring(i, 2) == "PM")
            //    {
            //        endTime = events.Substring(i - 8, 8);
            //    }
            //}

            //eventButton.Text = $"" + title + "\n" + startTime + "\n" + endTime;
        }
    }
}