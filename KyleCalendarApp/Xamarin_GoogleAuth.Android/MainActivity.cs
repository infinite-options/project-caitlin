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
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
            getCalendarsButton.Click += OnGetCalendarsButtonClicked;
            getEventsListButton.Click += GetEventsListButtonClicked;
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

        //------------------------------- GET EVENTS

        public void GetEventsListButtonClicked(object sender, EventArgs e)
        {
            OnGetEventsListCompleted();
        }

        public async void OnGetEventsListCompleted()
        {
            // Retrieve the user's active calendars
            var googleService = new GoogleService();
            var calendars = await googleService.GetEventsList();
            if (calendars == null)
            {
                new AlertDialog.Builder(this)
                               .SetTitle("Oops!")
                               .SetMessage("Please login before accessing your calendars")
                               .Show();
            }
            System.Diagnostics.Debug.WriteLine(calendars);
        }
    }
}
