using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using ml.Supervised.NaiveBayes;
using ml.Attributes;

namespace TwitterToDB
{
    class TwitterToDBClass
    {
        static void Main(string[] args)
        {
            Auth.SetUserCredentials("Ur3PBOoqVk51myW4EllCbLDq7", "b1vmUfCu3GRoXblMEyjPOuijpECDpvUpsYX8hymFGMhjt5LcHk", "262095897-zJ1P75AIw87di5BjtcLrq3MjlgFwhv0qonHFpDJy", "FQHxPPYwQmnhynZ4fLAIa9IM8S0pDf4gOlZtOT358ndiB");

            var authenticatedUser = User.GetAuthenticatedUser();
            var searchParameter = new SearchTweetsParameters("Demonitization")
            {
                MaximumNumberOfResults = 500,
                Until = new DateTime(2017, 06, 02)
            };

            var tweets = Search.SearchTweets(searchParameter);

            SaveTweets(tweets);
        }

        private static void SaveTweets(IEnumerable<ITweet> tweets)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Twitter;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            connection.Open();

            List<TweetClassification> tweetClassifications = new List<TweetClassification>();

            foreach (var tweet in tweets)
            {
                using (SqlCommand com = new SqlCommand("insert into Tweets(TwitterID,FullText,SentimentType,SentimentCount,CreatedDate,Language,IsReweet) values(@TwitterID,@FullText,@SentimentType,@SentimentCount,@CreatedDate,@Language,@IsReweet)", connection))
                {
                    Sentiment sentiment = SentimentIdentifier.GetSentiment(tweet.FullText);

                    com.Parameters.AddWithValue("@TwitterID", tweet.Id);
                    com.Parameters.AddWithValue("@FullText", StopWordRemover.RemoveStopwords(tweet.FullText));
                    com.Parameters.AddWithValue("@SentimentType", sentiment.Type);
                    com.Parameters.AddWithValue("@SentimentCount", sentiment.Count);
                    com.Parameters.AddWithValue("@CreatedDate", tweet.CreatedAt.ToShortDateString());
                    com.Parameters.AddWithValue("@Language", tweet.Language.ToString());
                    com.Parameters.AddWithValue("@IsReweet", tweet.IsRetweet.ToString());

                    tweetClassifications.Add(new TweetClassification { SentimentType = sentiment.Type, SentimentCount = sentiment.Count, IsRetweet = tweet.IsRetweet, Language = tweet.Language.ToString() });

                    com.ExecuteNonQuery();
                }
            }

            var model = new NaiveBayesModel<TweetClassification>();
            var predictor = model.Generate(tweetClassifications.ToArray());

            var result = predictor.Predict(new TweetClassification { SentimentCount = 19, Language = "English", IsRetweet = false });

            connection.Close();
        }

        internal class TweetClassification
        {
            [Label]
            public string SentimentType { get; set; }
            [Feature]
            public int SentimentCount { get; set; }
            [Feature]
            public string Language { get; set; }
            [Feature]
            public bool IsRetweet { get; set; }
        }
    }
}
