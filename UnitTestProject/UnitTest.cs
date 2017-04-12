using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using TwitterToDB;
using Newtonsoft.Json;
using OAuthTwitterWrapper.JsonTypes;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestTwitterAuthentication()
        {
            var oAuthTwitterWrapper = new OAuthTwitterWrapper.OAuthTwitterWrapper();
            var result = JsonConvert.DeserializeObject<Search>(oAuthTwitterWrapper.GetSearch());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestSearchTweets()
        {
            var oAuthTwitterWrapper = new OAuthTwitterWrapper.OAuthTwitterWrapper();
            var result = JsonConvert.DeserializeObject<Search>(oAuthTwitterWrapper.GetSearch());

            Assert.IsTrue(result.Results.Count > 0);
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

        [TestMethod]
        public void TestPositiveSentiment()
        {
            string sampleText = "Demonitization is an good move, however its difficult to implement";
            var sentiment = SentimentIdentifier.GetSentiment(sampleText);

            Assert.AreEqual(sentiment.Type, "Positive");
        }

        [TestMethod]
        public void TestNegativeSentiment()
        {
            string sampleText = "Demonitization is an bad move, we should abolish it soon";
            var sentiment = SentimentIdentifier.GetSentiment(sampleText);

            Assert.AreEqual(sentiment.Type, "Negative");
        }
    }
}
