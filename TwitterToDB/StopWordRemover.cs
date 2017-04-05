using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterToDB
{
    public class StopWordRemover
    {
        /// <summary>
        /// Remove stopwords from string.
        /// </summary>
        public static string RemoveStopwords(string input)
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
    }
}
