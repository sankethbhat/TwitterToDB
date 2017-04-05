using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterToDB
{
    public class SentimentIdentifier
    {
        public static Sentiment GetSentiment(string input)
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
    }

    public class Sentiment
    {
        public int Count { get; set; }
        public string Type { get; set; }
    }
}
