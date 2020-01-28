using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjectCaitlin.Authentication;
using Xamarin.Auth;
using Xamarin.Forms;

namespace ProjectCaitlin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
        OAuth2Authenticator authenticator;

        public MainPage()
        {
            InitializeComponent();

            credentials creds = new credentials();
            Console.WriteLine(creds.client_id);
            authenticator = new OAuth2Authenticator(
                creds.client_id,
                null,
                "https://www.googleapis.com/auth/calendar",
                new Uri(creds.auth_uri),
                new Uri(creds.redirect_uri),
                new Uri(creds.token_uri),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;

            Console.WriteLine("authenticator: " + authenticator);
            LoadFirestore();
        }

        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
            }
        }

        protected async Task LoadFirestore()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/7R6hAVmDrNutRkG3sVRy");
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var mealsString = await content.ReadAsStringAsync();
                JObject meals = JObject.Parse(mealsString);

                Console.WriteLine("Firebase:" + meals["fields"]["last_name"]["stringValue"].ToString());
            }
        }

        async void LoginClicked(object sender, EventArgs e)
        {
            presenter.Login(authenticator);
        }
    }
}
