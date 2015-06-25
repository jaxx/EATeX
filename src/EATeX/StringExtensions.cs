using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EATeX
{
    public static class StringExtensions
    {
        public static string ReplaceTags(this string text)
        {
            var openTags = new Stack<string>();
            var result = new StringBuilder();
            var sr = new StringReader(text);

            if (sr.Peek() == -1)
                return result.ToString();

            while (true)
            {
                var current = sr.Read();

                if (current == -1)
                {
                    if (openTags.Any())
                        throw new Exception("Missing closing tags.");

                    return result.ToString();
                }

                if (current == '>')
                    throw new Exception("Invalid tag - unexpected start.");

                if (current == '<')
                {
                    var isCloseTag = sr.Peek() == '/';
                    var tag = GetTag((char)current, sr);

                    if (tag == null)
                        throw new Exception("Invalid tag - unexpected end.");

                    if (!isCloseTag)
                        openTags.Push(tag);
                    else
                    {
                        if (openTags.Count == 0)
                            throw new Exception("Missing opening tags.");

                        var tempTag = tag.Replace("/", "");
                        var openTag = openTags.Pop();

                        if (openTag != tempTag)
                            throw new Exception("Order of open and close tags does not match.");
                    }
                }
                else
                {
                    var value = ReadPlainText((char)current, sr);

                    if (openTags.Any() && !string.IsNullOrWhiteSpace(value))
                    {
                        value = AddLaTeXCommands(value, openTags);
                        result.Append(value);
                    }
                    else
                        result.Append(value);
                }
            }
        }

        private static string ReadPlainText(char start, TextReader reader)
        {
            var sb = new StringBuilder().Append(start);

            while (true)
            {
                var current = reader.Peek();

                if (current == -1 || current == '<' || current == '>')
                    return sb.ToString();

                sb.Append((char)current);
                reader.Read();
            }
        }

        private static string GetTag(char start, TextReader reader)
        {
            var sb = new StringBuilder().Append(start);

            while (true)
            {
                var current = reader.Read();

                if (current == -1 || current == '<')
                    return null;

                sb.Append((char)current);

                if (current == '>')
                    return sb.ToString();
            }
        }

        private static string AddLaTeXCommands(string value, Stack<string> openTags)
        {
            var tagsToAdd = openTags.Reverse().ToArray();
            var sb = new StringBuilder();

            foreach (var tag in tagsToAdd)
            {
                switch (tag)
                {
                    case "<u>":
                        sb.Append(@"\underline{");
                        break;
                    case "<b>":
                        sb.Append(@"\textbf{");
                        break;
                    case "<i>":
                        sb.Append(@"\textit{");
                        break;
                }
            }

            sb.Append(value);
            sb.Append(new string('}', tagsToAdd.Length));

            return sb.ToString();
        }
    }
}