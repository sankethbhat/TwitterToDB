using ml.Attributes;
using ml.Supervised.NaiveBayes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterToDB
{
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

    internal class TweetClassifer
    {
        public static string FindSentimentType(int sentimentCount, bool isRetweet, string language, List<TweetClassification> tweetClassificationList)
        {
            var model = new NaiveBayesModel<TweetClassification>();
            var predictor = model.Generate(tweetClassificationList);

            var result = predictor.Predict(new TweetClassification { SentimentCount = sentimentCount, Language = language, IsRetweet = isRetweet });

            return result.SentimentType;
        }
    }
}
