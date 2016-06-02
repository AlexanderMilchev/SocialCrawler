using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Twitter.Models;

namespace Twitter
{
    public class Twitter
    {
        //plp[lll[
        public string OAuthConsumerSecret { get; set; }
        public string OAuthConsumerKey { get; set; }

        public async Task<IEnumerable<string>> GetTwitts(string userName, int count, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();
            }


            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}&trim_user=1&exclude_replies=1", count, userName));

            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = await httpClient.SendAsync(requestUserTimeline);
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(await responseUserTimeLine.Content.ReadAsStringAsync());
            var enumerableTwitts = (json as IEnumerable<dynamic>);

            if (enumerableTwitts == null)
            {
                return null;
            }
            return enumerableTwitts.Select(t => (string)(t["text"].ToString()));
        }

        public async Task<List<Status>> SearchTwittsByKeyWord(string keyword, int count = 15, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();
            }

            //To search with the twiter api we need to pas someting like this:
            // https://api.twitter.com/1.1/search/tweets.json?q=%40twitterapi
            //base URL for searches: https://api.twitter.com/1.1/search/tweets.json 
            //search query: "q=%40twitterapi" - can be taken from https://twitter.com/search
            //https://api.twitter.com/1.1/search/tweets.json?q=бойко%20борисов&src=typd

            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}&src=typd&count=15", keyword));
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(request);


            var jsonString = await response.Content.ReadAsStringAsync();
            System.IO.File.WriteAllText(@"D:\TwitterResult.txt", jsonString);
            TwitterSearchResult twitterSearchResults = JsonConvert.DeserializeObject<TwitterSearchResult>(jsonString);
            List<Status> results = new List<Status>();

            foreach (var result in twitterSearchResults.statuses)
            {
                results.Add(result);
            }

            return results;
        }

        public TwitterSearchResult CreateTwitterSearchResult(dynamic obj)
        {
            TwitterSearchResult result = new TwitterSearchResult();
            //try
            //{
            //    result.id = obj.id;
            //    result.text = obj.text;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}
            return result;
        }

        public async Task<string> GetAccessToken()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            string json = await response.Content.ReadAsStringAsync();
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(json);
            return item["access_token"];
        }
    }
}