using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OAuthTwitterWrapper.JsonTypes;

namespace TwitterToDB
{
    class TwitterToDBClass
    {
        static void Main(string[] args)
        {
            var oAuthTwitterWrapper = new OAuthTwitterWrapper.OAuthTwitterWrapper();
            var result = JsonConvert.DeserializeObject<Search>(oAuthTwitterWrapper.GetSearch());

            SaveTweets(result.Results);
        }

        private static void SaveTweets(List<Status> tweets)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Twitter;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            connection.Open();

            List<TweetClassification> tweetClassifications = new List<TweetClassification>();

            for (int i = 0; i < 100; i++ )
            {
                var tweet = tweets.ElementAt(i);

                using (SqlCommand com = new SqlCommand("insert into Tweets(TwitterID,FullText,SentimentType,SentimentCount,CreatedDate,Language,IsReweet,Location) values(@TwitterID,@FullText,@SentimentType,@SentimentCount,@CreatedDate,@Language,@IsReweet,@Location)", connection))
                {
                    Sentiment sentiment = SentimentIdentifier.GetSentiment(tweet.Text);
                    string sentimentType = i <= 90 ? sentiment.Type : TweetClassifer.FindSentimentType(sentiment.Count, tweet.retweeted, tweet.lang, 
                        tweetClassifications);

                    com.Parameters.AddWithValue("@TwitterID", tweet.id.ToString());
                    com.Parameters.AddWithValue("@FullText", StopWordRemover.RemoveStopwords(tweet.Text));
                    com.Parameters.AddWithValue("@SentimentType", sentimentType);
                    com.Parameters.AddWithValue("@SentimentCount", sentiment.Count);
                    com.Parameters.AddWithValue("@CreatedDate", tweet.CreatedAt);
                    com.Parameters.AddWithValue("@Language", tweet.lang);
                    com.Parameters.AddWithValue("@Location", tweet.User.Location);
                    com.Parameters.AddWithValue("@IsReweet", tweet.retweeted.ToString());

                    tweetClassifications.Add(new TweetClassification { SentimentType = sentiment.Type, SentimentCount = sentiment.Count, IsRetweet = tweet.retweeted, Language = tweet.lang.ToString() });

                    com.ExecuteNonQuery();
                }
            }

            connection.Close();
        }
    }
}
