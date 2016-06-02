using System;
using System.Collections.Generic;
using Twitter.Models;
using Twitter.Repositories;


using System.Collections;

using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


namespace Twitter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var twitter = new Twitter
                {
                    OAuthConsumerKey = "0Rj1w3s0d2EHcM6H6qYcCQ2y8",
                    OAuthConsumerSecret = "AnqKjJJ1SXgFo9UXphm1o0Afc4ChT9fCOJaL80KgiPjMdnfiLc"
                };
                //IEnumerable<string> twitts = twitter.GetTwitts("karpach96", 10).Result;
                //IEnumerable<string> twitts = twitter.GetTwitts("Sashko_Sashkov", 10).Result;


                List<string> keys = new List<string>();
                keys.Add("бойко%20борисов");
                keys.Add("forbes");


                foreach (var key in keys)
                {
                    List<Status> twitts = twitter.SearchTwittsByKeyWord(key).Result;

                    TwitteSearchResultsRepository repo = new TwitteSearchResultsRepository();
                    repo.Add(twitts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
