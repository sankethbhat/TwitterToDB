using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
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

            var searchParameter = new SearchTweetsParameters("Demonitization")
            {
                MaximumNumberOfResults = 100,
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
                using (SqlCommand com = new SqlCommand("insert into Tweets(TwitterID,FullText) values(@a,@b)", connection))
                {
                    com.Parameters.AddWithValue("@a", tweet.Id);
                    com.Parameters.AddWithValue("@b", tweet.FullText);

                    com.ExecuteNonQuery();
                }
            }

            connection.Close();

            /*
            OleDbConnection my_con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\\sbhat2\\Documents\\Twitter.accdb");
            my_con.Open();
            OleDbCommand o_cmd = new OleDbCommand("insert into Tweets(TwitterID,FullText) values(@a,@b)", my_con);
            o_cmd.Parameters.AddWithValue("@a", 123);
            o_cmd.Parameters.AddWithValue("@b", "sample");
            int i = o_cmd.ExecuteNonQuery();
            my_con.Close();*/

            /*
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\sbhat2\Documents\Twitter.accdb";
            string QueryText = "INSERT INTO Tweets (TwitterID,FullText) values (@TwitterID,@FullText)";
            connect.Open();

            using (OleDbCommand command = new OleDbCommand(QueryText))
            {
                try
                {

                    foreach (var tweet in tweets)
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter("INSERT INTO Tweets", connect);

                        command.Parameters.AddWithValue("@TwitterID", tweet.Id);
                        command.Parameters.AddWithValue("@FullText", tweet.FullText);

                        command.ExecuteNonQuery();
                    }


                    connect.Close();
                }
                catch (Exception ex)
                {
                    connect.Close();
                }
            }
            
            try
            {
                using (var connection1 = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\sbhat2\Documents\Twitter.accdb"))
                {
                    OleDbDataAdapter cmd = new OleDbDataAdapter();

                    foreach (var tweet in tweets)
                    {
                        using (var insertCommand = new OleDbCommand("INSERT INTO Tweets (TwitterID,FullText) values (@TwitterID,@FullText)"))
                        {
                            insertCommand.Connection = connection1;
                            cmd.InsertCommand = insertCommand;
                            //.....
                            connection1.Open();

                            insertCommand.Parameters.AddWithValue("@TwitterID", tweet.Id);
                            insertCommand.Parameters.AddWithValue("@FullText", tweet.FullText);

                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //connect.Close();
            }
            */
        }
    }
}
