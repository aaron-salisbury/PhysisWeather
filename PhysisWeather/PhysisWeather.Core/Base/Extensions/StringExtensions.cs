using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysisWeather.Core.Base.Extensions
{
    public static class StringExtensions
    {
        public static string GetFirst(this string source, int numberOfChars)
        {
            if (source.Length > numberOfChars)
            {
                return source.Substring(0, numberOfChars);
            }

            return source;
        }

        public static string GetLast(this string source, int numberOfChars)
        {
            if (numberOfChars >= source.Length)
            {
                return source;
            }

            return source.Substring(source.Length - numberOfChars);
        }

        public static string GetAfterLastOrEmpty(this string text, string stopAt)
        {
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(stopAt))
            {
                int charLocation = text.LastIndexOf(stopAt) + 1;

                if (charLocation > 0)
                {
                    return text.Substring(charLocation, text.Length - charLocation);
                }
            }

            return string.Empty;
        }

        public static string GetUntilOrEmpty(this string text, string stopAt)
        {
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(stopAt))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///   Takes PascalCase and transforms to Pascal Case. Also does AMACharter to AMA Charter.
        ///   If the preposition is the first or last word, then it should still be capitalized.
        /// </summary>
        public static string ToDisplayName(this string s, string spacer = " ", bool ignorePrepositions = false)
        {
            string eval = s ?? "";
            int words = 0;
            StringBuilder displayName = new StringBuilder(eval.Length);
            int wordStart = 0;
            int wordEnd = 0;

            for (int i = 0; i < eval.Length; i++)
            {
                wordEnd += 1;

                if (i + 1 < eval.Length && eval[i + 1].IsUpper())
                {
                    bool endsWithUpper = eval[i].IsUpper();
                    bool nextCharacterIsLower = i + 2 < eval.Length && eval[i + 2].IsLower();

                    if (!endsWithUpper || nextCharacterIsLower)
                    {
                        string wordString = eval.Substring(wordStart, wordEnd - wordStart);
                        if (words > 0 && (!ignorePrepositions && prepositions.TryGetValue(wordString, out string preposition))) //If the preposition is not the first or last word, then it should be lower case
                        {
                            wordString = preposition;
                        }
                        displayName.Append(wordString);
                        words++;
                        if (eval[i] != '-')// No space after -
                        {
                            displayName.Append(spacer);
                        }
                        wordStart = wordEnd;
                    }
                }
            }

            displayName.Append(eval, wordStart, wordEnd - wordStart);

            return displayName.ToString();
        }

        /// <summary>
        /// A list of prepositions.
        /// </summary>
        private static readonly Dictionary<string, string> prepositions = new string[]
        {
            "a",
            "an",
            "and",
            "aboard",
            "about",
            "above",
            "across",
            "after",
            "against",
            "along",
            "amid",
            "among",
            "anti",
            "around",
            "as",
            "at",
            "before",
            "behind",
            "below",
            "beneath",
            "but",
            "beside",
            "besides",
            "between",
            "beyond",
            "by",
            "concerning",
            "underneath",
            "considering",
            "despite",
            "down",
            "during",
            "except",
            "excepting",
            "excluding",
            "following",
            "for",
            "from",
            "in",
            "inside",
            "into",
            "like",
            "minus",
            "near",
            "of",
            "or",
            "on",
            "onto",
            "opposite",
            "outside",
            "over",
            "past",
            "per",
            "plus",
            "regarding",
            "round",
            "save",
            "since",
            "the",
            "than",
            "through",
            "to",
            "toward",
            "towards",
            "under",
            "unlike",
            "until",
            "up",
            "upon",
            "versus",
            "via",
            "with",
            "within",
            "without",
            "now",
            "that",
            "if",
            "when",
            "because",
            "while",
            "where",
            "unless",
            "so",
            "though",
            "whether",
            "although",
            "nor",
            "once"
        }
        .ToDictionary(prep => prep, prep => prep, StringComparer.OrdinalIgnoreCase);
    }
}
