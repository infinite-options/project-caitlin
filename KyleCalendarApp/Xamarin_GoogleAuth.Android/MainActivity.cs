using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Android.Net;
using Xamarin_GoogleAuth.Authentication;
using Xamarin_GoogleAuth.Services;
using Xamarin_GoogleAuth;
using static Android.Bluetooth.BluetoothClass;
using Android.Content;
using System.Drawing;
using System.Net;
using Android.Graphics;
using System.Threading.Tasks;
using static Android.App.DatePickerDialog;
using Android.Views.InputMethods;

namespace Xamarin_GoogleAuth.Droid
{
    [Activity(Label = "Xamarin_GoogleAuth", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IGoogleAuthenticationDelegate, IOnDateSetListener
    {
        // Need to be static because we need to access it in GoogleAuthInterceptor for continuation
        public static GoogleAuthenticator Auth;
        ImageView dispPhoto;

        public int publicYear;
        public int publicMonth;
        public int publicDay;
        public string[] tasks;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);


            Auth = new GoogleAuthenticator(Configuration.ClientId, Configuration.Scope, Configuration.RedirectUrl, this);

            dispPhoto = FindViewById<ImageView>(Resource.Id.displayPhoto);
            var getPhotosButton = FindViewById<Button>(Resource.Id.getPhotosButton);
            var googleLoginButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            var getCalendarsButton = FindViewById<Button>(Resource.Id.getCalendarsButton);
            var getEventsListButton = FindViewById<Button>(Resource.Id.getEventsListButton);
            var dateEditText = FindViewById<EditText>(Resource.Id.date_EditText);
            //var getFirebaseActivitiesButton = FindViewById<Button>(Resource.Id.getFirebaseActivitiesButton);
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
            getCalendarsButton.Click += OnGetCalendarsButtonClicked;
            getPhotosButton.Click += OnGetPhotosButtonClicked;
            getEventsListButton.Click += GetEventsListButtonClicked;
            dateEditText.Click += OnClickDateEditText;
            //getFirebaseActivitiesButton.Click += GetFirebaseActivitiesButtonClicked;
            //OnGetFirebaseActivitiesCompleted();
        }

        //-------------------------------- DATE PICKER

        private void OnClickDateEditText(object sender, EventArgs e)
        {
            var dateTimeNow = DateTime.Now;
            DatePickerDialog datePicker = new DatePickerDialog(this, this, dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day);
            datePicker.Show();
            DismissKeyboard();
        }

        public async void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            var monthFixed = month + 1;

            FindViewById<EditText>(Resource.Id.date_EditText).Text = new DateTime(year, monthFixed, dayOfMonth).ToShortDateString();

            publicYear = year;
            publicDay = dayOfMonth;
            publicMonth = monthFixed;

            var googleService = new GoogleService();
            var events = await googleService.GetSpecificEventsList(publicYear, publicMonth, publicDay);

            var todoButton1 = FindViewById<Button>(Resource.Id.TodoEvent1);
            var todoButton2 = FindViewById<Button>(Resource.Id.TodoEvent2);

            todoButton1.Text = "";
            todoButton2.Text = "";

            try
            {
                tasks = events.Split(',');
            }
            catch { }

                try
                {
                    if (tasks[1] == null)
                    {

                    }
                    else
                    {
                        try
                        {

                        string time1 = tasks[1];
                        string time2 = tasks[2];
                        string time3 = tasks[4];
                        string time4 = tasks[5];
                        string sub1 = time1.Substring(11, 11);
                        string sub2 = time2.Substring(11, 11);
                        string sub4 = time3.Substring(11, 11);
                        string sub5 = time4.Substring(11, 11);

                        todoButton1.Text = $"" + tasks[0] + "\n" + sub1 + "\n" + sub2;
                        todoButton2.Text = $"" + tasks[3] + "\n" + sub4 + "\n" + sub5;

                    }
                        catch { }

                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                string error = String.Format("You don't have any events scheduled on {0}/{1}/{2}", publicMonth, publicDay, publicYear);

                new AlertDialog.Builder(this)
                        .SetTitle("Error")
                        .SetMessage(error)
                        .Show();
            }
            catch (NullReferenceException e)
            {
                new AlertDialog.Builder(this)
                        .SetTitle("Error")
                        .SetMessage("You are not logged in")
                        .Show();
            }
        }

        private void DismissKeyboard()
        {
            var view = CurrentFocus;
            if (view != null)
            {
                var imm = (InputMethodManager)GetSystemService(InputMethodService);
                imm.HideSoftInputFromWindow(view.WindowToken, 0);
            }
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

            //Find Toothbrush, Completed, Find Toothpaste, Completed, Put Toothpaste on Toothbrush, Completed, Brush teeth with Toothbrush, In Progress, Rinse mouth, Planned, Rinse Toothbrush, Planned, Put Toothbrush in holder, Planned, Press Com
            //System.Diagnostics.Debug.WriteLine(calendars);
        }


        //----------------------------- BRING UP PHOTO ALBUM

        public async void OnGetPhotosButtonClicked(object sender, EventArgs e)
        {
            var googlePhotoService = new GooglePhotoService();
            var photos = await googlePhotoService.GetPhotos();

            if (photos == null)
            {
                new AlertDialog.Builder(this)
                               .SetTitle("Oops!")
                               .SetMessage("Please login before accessing your photos")
                               .Show();
            }

            var uri = Android.Net.Uri.Parse(photos);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);

            //Android.Net.Uri UriForImage = Android.Net.Uri.Parse(photos);
            //dispPhoto.SetImageURI(UriForImage);

            /*Android.Graphics.Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(photos);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            dispPhoto.SetImageBitmap(imageBitmap);*/


            System.Diagnostics.Debug.WriteLine(photos);
        }

        /*public async Android.Graphics.Bitmap GetImageBitmapFromUrl(string url)
        {
            Android.Graphics.Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }*/



        //------------------------------- GET FIREBASE INFO

        public void GetFirebaseActivitiesButtonClicked(object sender, EventArgs e)
        {
            OnGetFirebaseActivitiesCompleted();
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


            var todoButton1 = FindViewById<Button>(Resource.Id.TodoEvent1);
            var todoButton2 = FindViewById<Button>(Resource.Id.TodoEvent2);


            var title = "";
            var startTime = "";
            var endTime = "";
            for (int i = 0; i < events.Length - 2; i++)
            {
                if (events.Substring(i, 1) == "," && title == "")
                {
                    title = events.Substring(0, i);
                }
                else if (startTime == "" && events.Substring(i, 2) == "AM")
                {
                    startTime = events.Substring(i - 8, 8);
                }
                else if (endTime == "" && events.Substring(i, 2) == "AM")
                {
                    endTime = events.Substring(i - 8, 8);
                }
            }

            todoButton1.Text = "Eat Breakfast";
            todoButton2.Text = $"" + title + "\n" + startTime + " AM" + "\n" + endTime + " AM";

        }

        public async void OnGetFirebaseActivitiesCompleted()
        {
            // Retrieve the user's active calendars
            var firebaseService = new FirebaseService();
            var activities = await firebaseService.GetActivities();

            //System.Diagnostics.Debug.WriteLine(activities);
            //System.Diagnostics.Debug.WriteLine("hmmmm");


            if (activities == (null))
            {
                new AlertDialog.Builder(this)
                               .SetTitle("Oops!")
                               .SetMessage("Please login before listing you events")
                               .Show();
            }

            string[] tasks = activities.Split(',');

            var eventButton1 = FindViewById<Button>(Resource.Id.Event1);
            var eventButton2 = FindViewById<Button>(Resource.Id.Event2);
            var eventButton3 = FindViewById<Button>(Resource.Id.Event3);
            var eventButton4 = FindViewById<Button>(Resource.Id.Event4);

            eventButton1.Text = "";
            eventButton2.Text = "";
            eventButton3.Text = "";
            eventButton4.Text = "";

            //---------
            try
            {

                if (tasks[0] == null)
                {
                    eventButton1.Text = "N/A";
                }
                else
                {
                    eventButton1.Text = tasks[0];
                }

                //-----------

                if (tasks[1] == null)
                {
                    eventButton2.Text = "N/A";
                }
                else
                {
                    eventButton2.Text = tasks[1];
                }

                //------------

                if (tasks[2] == null)
                {
                    eventButton3.Text = "N/A";
                }
                else
                {
                    eventButton3.Text = tasks[2];
                }

                //--------------

                if (tasks[3] == null)
                {
                    eventButton4.Text = "N/A";
                }
                else
                {
                    eventButton4.Text = tasks[3];
                }
            }

            catch {} 
            //eventButton1.Text = tasks[0];
            //eventButton2.Text = tasks[1];
            //eventButton3.Text = tasks[2];
            //eventButton4.Text = tasks[3];

            //String calendar = activities;

            //int counter = 0;
            //int skip = 0;
            //string event1 = "";
            //string event2 = "";
            //string event3 = "";
            //string event4 = "";

            //for (int i = 0; i < calendars.Length; i++)
            //{
            //    if (calendars.Substring(i, 1) == "," && skip == 1)
            //    {
            //        skip = 0;
            //        counter = i + 1;
            //    }
            //    else if (event1 == "" && calendars.Substring(i, 1) == ",")
            //    {
            //        skip = 1;
            //        event1 = calendars.Substring(counter, i - counter);
            //    }
            //    else if (event2 == "" && calendars.Substring(i, 1) == ",")
            //    {
            //        skip = 1;
            //        event2 = calendars.Substring(counter, i - counter);
            //    }
            //    else if (event3 == "" && calendars.Substring(i, 1) == ",")
            //    {
            //        skip = 1;
            //        event3 = calendars.Substring(counter, i - counter);
            //    }
            //    else if (event4 == "" && calendars.Substring(i, 1) == ",")
            //    {
            //        skip = 1;
            //        event4 = calendars.Substring(counter, i - counter);
            //        i = calendars.Length;
            //    }
            //}

        }
    }
}