namespace FlickrApp.Extensions
{
    #region Imports

    using System;
    using System.Collections.Generic;

    #endregion

    internal static class StringExtensions
    {
        /// <summary>
        /// Returns a list of strings no larger than the max length sent in.
        /// Source: http://bryan.reynoldslive.com/post/Wrapping-string-data.aspx
        /// </summary>
        /// <remarks>useful function used to wrap string text for reporting.</remarks>
        /// <param name="text">Text to be wrapped into of List of Strings</param>
        /// <param name="maxLength">Max length you want each line to be.</param>
        /// <returns>List of Strings</returns>
        public static string Wrap(this string text, int maxLength)
        {
            // Return empty list of strings if the text was empty

            if (string.IsNullOrEmpty(text))
                return string.Empty;


            var lines = new List<string>();
            var words = text.Split(' ');
            var currentLine = String.Empty;

            foreach (var currentWord in words)
            {
                if ((currentLine.Length > maxLength) ||
                    ((currentLine.Length + currentWord.Length) > maxLength))

                {
                    lines.Add(currentLine);
                    currentLine = "";
                }

                if (currentLine.Length > 0)
                    currentLine += " " + currentWord;
                else
                    currentLine += currentWord;
            }

            if (currentLine.Length > 0)
                lines.Add(currentLine);


            return string.Join(Environment.NewLine, lines);
        }
    }
}