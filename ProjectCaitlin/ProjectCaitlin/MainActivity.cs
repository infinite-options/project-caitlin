using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Xamarin.Essentials;
using static Android.Bluetooth.BluetoothClass;
using System;
using Android.Provider;
using System.IO;

namespace ProjectCaitlin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button bedroom = FindViewById<Button>(Resource.Id.bedroom);
            Button restroom = FindViewById<Button>(Resource.Id.restroom);
            Button kitchen = FindViewById<Button>(Resource.Id.kitchen);
            Button living = FindViewById<Button>(Resource.Id.LivingRoom);
            ImageButton callMom = FindViewById<ImageButton>(Resource.Id.callMom);
            ImageButton callDad = FindViewById<ImageButton>(Resource.Id.callDad);
            ImageButton callBro = FindViewById<ImageButton>(Resource.Id.callBro);
            Button longTerm = FindViewById<Button>(Resource.Id.LongTerm);
            Button takePic = FindViewById<Button>(Resource.Id.takePic);

            takePic.Click += (s, e) =>
            {
                Intent i = new Intent(MediaStore.ActionImageCapture);
                StartActivityForResult(i, 0);
            };

            callMom.Click += (s, e) =>
            {
                Intent i = new Intent(Intent.ActionDial);
                i.SetData(Android.Net.Uri.Parse("tel:15555555555"));
                StartActivity(i);
                //Device.OpenUri(new Uri("tel://xxxxxxxxxx"));
                //PhoneDialer.Open("15555555555");
            };

            callDad.Click += (s, e) =>
            {
                PhoneDialer.Open("15555555555");
            };

            callBro.Click += (s, e) =>
            {
                PhoneDialer.Open("15555555555");

            };

            longTerm.Click += (s, e) =>
            {
                Intent intent = new Intent(this, typeof(LongTerm));
                StartActivity(intent);
            };

            bedroom.Click += (s, e) =>
            {
                Intent intent = new Intent(this, typeof(BedroomActivities));
                StartActivity(intent);

            };
            restroom.Click += (s, e) =>
            {
                Intent intent = new Intent(this, typeof(RestroomActivity));
                StartActivity(intent);

            };
            kitchen.Click += (s, e) =>
            {
                Intent intent = new Intent(this, typeof(KitchenActivity));
                StartActivity(intent);
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}