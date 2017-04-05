using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterToDB
{
    class TwitterToDBClass
    {
        static void Main(string[] args)
        {
            Auth.SetUserCredentials("Ur3PBOoqVk51myW4EllCbLDq7", "b1vmUfCu3GRoXblMEyjPOuijpECDpvUpsYX8hymFGMhjt5LcHk", "262095897-zJ1P75AIw87di5BjtcLrq3MjlgFwhv0qonHFpDJy", "FQHxPPYwQmnhynZ4fLAIa9IM8S0pDf4gOlZtOT358ndiB");

            var authenticatedUser = User.GetAuthenticatedUser();
            var matchingTweets = Search.SearchTweets("Demonitization");
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

            foreach (var tweet in tweets)
            {
                using (SqlCommand com = new SqlCommand("insert into Tweets(TwitterID,FullText,SentimentType,SentimentCount,CreatedDate,Language,IsReweet) values(@TwitterID,@FullText,@SentimentType,@SentimentCount,@CreatedDate,@Language,@IsReweet)", connection))
                {
                    Sentiment sentiment = GetSentiment(tweet.FullText);

                    com.Parameters.AddWithValue("@TwitterID", tweet.Id);
                    com.Parameters.AddWithValue("@FullText", RemoveStopwords(tweet.FullText));
                    com.Parameters.AddWithValue("@SentimentType", sentiment.Type);
                    com.Parameters.AddWithValue("@SentimentCount", sentiment.Count);
                    com.Parameters.AddWithValue("@CreatedDate", tweet.CreatedAt.ToShortDateString());
                    com.Parameters.AddWithValue("@Language", tweet.Language.ToString());
                    com.Parameters.AddWithValue("@IsReweet", tweet.IsRetweet.ToString());

                    com.ExecuteNonQuery();
                }
            }

            connection.Close();
        }

        

        /// <summary>
        /// Remove stopwords from string.
        /// </summary>
        private static string RemoveStopwords(string input)
        {
            // 1
            // Split parameter into words
            var words = input.Split(Constants.DELIMITERS,
                StringSplitOptions.RemoveEmptyEntries);
            // 2
            // Allocate new dictionary to store found words
            var found = new Dictionary<string, bool>();
            // 3
            // Store results in this StringBuilder
            StringBuilder builder = new StringBuilder();
            // 4
            // Loop through all words
            foreach (string currentWord in words)
            {
                // 5
                // Convert to lowercase
                string lowerWord = currentWord.ToLower();
                // 6
                // If this is a usable word, add it
                if (!Constants.STOP_WORDS.ContainsKey(lowerWord) &&
                    !found.ContainsKey(lowerWord))
                {
                    builder.Append(currentWord).Append(' ');
                    found.Add(lowerWord, true);
                }
            }
            // 7
            // Return string with words removed
            return builder.ToString().Trim();
        }

        private static Sentiment GetSentiment(string input)
        {
            // 1
            // Split parameter into words
            var words = input.Split(Constants.DELIMITERS, StringSplitOptions.RemoveEmptyEntries);
            int positiveCount = 0;
            int negativeCount = 0;
            int neutralCount = 0;
            Sentiment sentiment = new Sentiment();

            // Loop through all words
            foreach (string currentWord in words)
            {
                if (Constants.NEGATIVE_WORDS.Contains(currentWord.ToLower()))
                    negativeCount++;

                if (Constants.POSITIVE_WORDS.Contains(currentWord.ToLower()))
                    positiveCount++;
            }

            if (positiveCount == neutralCount)
            {
                sentiment.Type = "Neutral";
                sentiment.Count = 0;
            }

            if (positiveCount > neutralCount)
            {
                sentiment.Type = "Positive";
                sentiment.Count = positiveCount;
            }

            if (positiveCount < negativeCount)
            {
                sentiment.Type = "Negative";
                sentiment.Count = negativeCount;
            }

            return sentiment;
        }

        internal class Sentiment
        {
            public int Count { get; set; }
            public string Type { get; set; }
        }
    }
}
