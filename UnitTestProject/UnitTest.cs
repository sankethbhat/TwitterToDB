using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using System.Collections.Generic;
using Tweetinvi.Models;
using System.Data.SqlClient;
using TwitterToDB;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestTwitterAuthentication()
        {
            Auth.SetUserCredentials("Ur3PBOoqVk51myW4EllCbLDq7", "b1vmUfCu3GRoXblMEyjPOuijpECDpvUpsYX8hymFGMhjt5LcHk", "262095897-zJ1P75AIw87di5BjtcLrq3MjlgFwhv0qonHFpDJy", "FQHxPPYwQmnhynZ4fLAIa9IM8S0pDf4gOlZtOT358ndiB");

            var authenticatedUser = User.GetAuthenticatedUser();

            Assert.IsNotNull(authenticatedUser);
        }

        [TestMethod]
        public void TestSearchTweets()
        {
            Auth.SetUserCredentials("Ur3PBOoqVk51myW4EllCbLDq7", "b1vmUfCu3GRoXblMEyjPOuijpECDpvUpsYX8hymFGMhjt5LcHk", "262095897-zJ1P75AIw87di5BjtcLrq3MjlgFwhv0qonHFpDJy", "FQHxPPYwQmnhynZ4fLAIa9IM8S0pDf4gOlZtOT358ndiB");

            var authenticatedUser = User.GetAuthenticatedUser();
            IEnumerable<ITweet> tweets = Search.SearchTweets("Demonitization");

            Assert.IsNotNull(tweets);
        }

        [TestMethod]
        public void TestSQLConnection()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Twitter;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            connection.Open();
            connection.Close();

            Assert.IsNotNull(connection);
        }

        [TestMethod]
        public void TestStopWordsRemoval()
        {
            string sampleText = "Demonitization is an good move, however its difficult to implement";
            string afterStopWordRemoval = StopWordRemover.RemoveStopwords(sampleText);

            Assert.AreNotEqual(sampleText, afterStopWordRemoval);
        }
    }
}
