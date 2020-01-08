using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace FrontEndSruthi
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        protected async void getActivities()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://phaqvwjbw6.execute-api.us-west-1.amazonaws.com/dev/api/v1/kitchens");
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var activityString = await content.ReadAsStringAsync();
                JObject activity = JObject.Parse(activityString);

                string displayActivity = "";
                var i = 0;
                foreach (var a in activity["result"]) {
                    displayActivity = (string)a["kitchen_name"]["S"];
                    if (i == 0)
                    {
                        activityPopUp.Text = displayActivity;
                    } else if (i == 1)
                    {
                        activityPopUp1.Text = displayActivity;
                    } else if (i == 2)
                    {
                        activityPopUp2.Text = displayActivity;
                    }
                    i = i+1;
                    if (i == 3)
                    {
                        break;
                    }
                }
            }
        }

        public MainPage()
        {
            InitializeComponent();

            getActivities();
        }       
    }
}
