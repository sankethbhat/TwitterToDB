using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterToDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Auth.SetUserCredentials("Ur3PBOoqVk51myW4EllCbLDq7", "b1vmUfCu3GRoXblMEyjPOuijpECDpvUpsYX8hymFGMhjt5LcHk", "262095897-zJ1P75AIw87di5BjtcLrq3MjlgFwhv0qonHFpDJy", "FQHxPPYwQmnhynZ4fLAIa9IM8S0pDf4gOlZtOT358ndiB");

            var authenticatedUser = User.GetAuthenticatedUser();

            // Get my Home Timeline
            //var tweets = Timeline.GetHomeTimeline();

            var matchingTweets = Search.SearchTweets("Demonitization");

            var searchParameter = new SearchTweetsParameters("Demonitization") {
                MaximumNumberOfResults = 100,
                Until = new DateTime(2017, 06, 02)
            };

            var tweets = Search.SearchTweets(searchParameter);
        }
    }
}
