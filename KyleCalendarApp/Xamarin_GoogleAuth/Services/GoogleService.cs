using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin_GoogleAuth;
using Xamarin_GoogleAuth.Authentication;

namespace Xamarin_GoogleAuth.Services
{
    public class GoogleService
    {

        public async Task<string> GetCalendars()
        {

            //Make HTTP Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://www.googleapis.com/calendar/v3/users/me/calendarList");
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", GoogleAuthenticator.superToken);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept" , "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();

            //Deserialize JSON Result
            var result = JsonConvert.DeserializeObject<Methods.GetCalendarsMethod>(json);

            //Create itemList
            var itemList = new List<string>();

            //Try to add "Summary" Items to list from JSON. If null, redirect to Login prompt.
            try
            {
                foreach (var sum in result.Items)
                {
                    itemList.Add(sum.Summary);
                }
            }
            catch(NullReferenceException e)
            {
                return null;
            }

            //Compile these values in to a string list and return to be displayed
            string itemListString = String.Join(", ", itemList);

            return itemListString;
        }

        public async Task<string> GetEventsList()
        {

            //Make HTTP Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events?timeMax=2020-01-01T04%3A36%3A05.000Z&timeMin=2019-12-26T15%3A37%3A21.716Z");
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", GoogleAuthenticator.superToken);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();

            //Deserialize JSON Result
            var result = JsonConvert.DeserializeObject<Methods.GetEventsListMethod>(json);

            //Create itemList
            var itemList = new List<string>();

            //Try to add "Summary" Items to list from JSON. If null, redirect to Login prompt.
            try
            {
                foreach (var events in result.Items)
                {
                    itemList.Add(events.EventName);
                }
            }
            catch (NullReferenceException e)
            {
                return null;
            }

            //Compile these values in to a string list and return to be displayed
            string itemListString = String.Join(", ", itemList);

            return itemListString;
        }
    }
}
