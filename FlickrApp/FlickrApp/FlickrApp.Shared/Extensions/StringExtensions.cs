﻿namespace FlickrApp.Extensions
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    internal static class StringExtensions
    {
        public static string WrapAtSpace(this string text, int maxLength, int maxLine = 0)
        {
            const char space = ' ';

            var lines = WrapAtChar(text, maxLength, space);

            if (maxLine <= 0) // no limit
                return string.Join(Environment.NewLine, lines);

            var theLines = lines as IList<string> ?? lines.ToList();
            var str = string.Join(Environment.NewLine, theLines.Take(maxLine));

            if (theLines.Count > maxLine)
                str += " …";

            return str;
        }

        public static IEnumerable<string> WrapAtChar(this string text, int maxLength, char chr)
        {
            if (string.IsNullOrEmpty(text))
                yield break;

            var tokens = text.Split(new[] {chr}, StringSplitOptions.RemoveEmptyEntries);
            var currentLine = string.Empty;

            foreach (var token in tokens)
            {
                if ((currentLine.Length > maxLength) ||
                    ((currentLine.Length + token.Length) > maxLength))

                {
                    yield return currentLine;
                    currentLine = string.Empty;
                }

                if (currentLine.Length > 0)
                    currentLine += (chr + token);
                else
                    currentLine += token;
            }

            if (currentLine.Length > 0)
                yield return currentLine;
        }
    }
}