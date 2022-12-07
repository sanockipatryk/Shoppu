using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Shoppu.Domain.Extensions
{

    //using solution from https://www.c-sharpcorner.com/blogs/make-url-slugs-in-asp-net-using-c-sharp2
    public static class StringExtensions
    {
        /// <summary>  
        /// Removes all accents from the input string.  
        /// </summary>  
        /// <param name="text">The input string.</param>  
        /// <returns></returns>  
        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        /// <summary>  
        /// Turn a string into a slug by removing all accents,   
        /// special characters, additional spaces, substituting   
        /// spaces with hyphens & making it lower-case.  
        /// </summary>  
        /// <param name="phrase">The string to turn into a slug.</param>  
        /// <returns></returns>  
        public static string Slugify(this string phrase)
        {
            // Remove all accents and make the string lower case.  
            string output = phrase.RemoveAccents().ToLower();

            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.  
            return output;
        }

        public static string AppendRandomDigits(this string phrase, int numberOfDigits)
        {
            if (numberOfDigits >= 0)
            {
                StringBuilder output = new StringBuilder(phrase);
                Random rnd = new Random();
                string randomNumber = rnd
                    .Next(
                        (int)Math.Pow(10, numberOfDigits - 1),
                        (int)Math.Pow(10, numberOfDigits)
                        )
                    .ToString();
                output.Append($"-{randomNumber}");
                return output.ToString();
            }
            return phrase;
        }
    }
}