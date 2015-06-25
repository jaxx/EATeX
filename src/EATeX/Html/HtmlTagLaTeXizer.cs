using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EATeX.Html
{
    public class HtmlTagLaTeXizer
    {
        private static readonly Dictionary<string, HtmlTag> htmlTags = new Dictionary<string, HtmlTag>
        {
            { "<b>", new HtmlTag { Type = HtmlTagType.Bold } },
            { "<u>", new HtmlTag { Type = HtmlTagType.Underline } },
            { "<i>", new HtmlTag { Type = HtmlTagType.Italic } },
            { "<a>", new HtmlTag { Type = HtmlTagType.Hyperlink } },
            { "<ul>", new HtmlTag { Type = HtmlTagType.UnorderedList } },
            { "<ol>", new HtmlTag { Type = HtmlTagType.OrderedList } },
            { "<li>", new HtmlTag { Type = HtmlTagType.ListItem } },
            { "<sup>", new HtmlTag { Type = HtmlTagType.Superscript } },
            { "<sub>", new HtmlTag { Type = HtmlTagType.Subscript } },
            { "<font>", new HtmlTag { Type = HtmlTagType.Font } }
        };

        public static string Latexize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var sr = new StringReader(text);
            var result = new StringBuilder();
            var openTags = new Stack<HtmlTag>();

            while (true)
            {
                var current = sr.Read();

                if (current == -1)
                {
                    if (openTags.Any())
                        throw new Exception("Missing closing tags.");

                    return result.ToString();
                }

                if (current == '<')
                {
                    var isCloseTag = sr.Peek() == '/';
                    var htmlTag = GetHtmlTag(current, sr, isCloseTag);

                    if (!isCloseTag)
                    {
                        AddLaTeXStartCommand(htmlTag, result);
                        openTags.Push(htmlTag);
                    }
                    else
                    {
                        AddLaTeXEndCommand(htmlTag, result);

                        if (!openTags.Any())
                            throw new Exception("Missing opening tags.");

                        var openTag = openTags.Pop();

                        if (openTag.Type != htmlTag.Type)
                            throw new Exception("Order of open and close tags does not match.");
                    }
                }
                else
                {
                    var value = ReadPlainText((char) current, sr);
                    result.Append(value);
                }
            }
        }

        private static void AddLaTeXStartCommand(HtmlTag htmlTag, StringBuilder sb)
        {
            switch (htmlTag.Type)
            {
                case HtmlTagType.Bold:
                    sb.Append(@"\textbf{");
                    break;
                case HtmlTagType.Underline:
                    sb.Append(@"\underline{");
                    break;
                case HtmlTagType.Italic:
                    sb.Append(@"\textit{");
                    break;
                case HtmlTagType.OrderedList:
                    sb.Append(@"\begin{enumerate}");
                    break;
                case HtmlTagType.UnorderedList:
                    sb.Append(@"\begin{itemize}");
                    break;
                case HtmlTagType.ListItem:
                    sb.Append(@"\item ");
                    break;
                case HtmlTagType.Superscript:
                    sb.Append(@"$^{");
                    break;
                case HtmlTagType.Subscript:
                    sb.Append(@"$_{");
                    break;
            }
        }

        private static void AddLaTeXEndCommand(HtmlTag htmlTag, StringBuilder sb)
        {
            switch (htmlTag.Type)
            {
                case HtmlTagType.Bold:
                case HtmlTagType.Underline:
                case HtmlTagType.Italic:
                    sb.Append(@"}");
                    break;
                case HtmlTagType.OrderedList:
                    sb.Append(@"\end{enumerate}");
                    break;
                case HtmlTagType.UnorderedList:
                    sb.Append(@"\end{itemize}");
                    break;
                case HtmlTagType.Superscript:
                case HtmlTagType.Subscript:
                    sb.Append(@"}$");
                    break;
            }
        }

        private static HtmlTag GetHtmlTag(int current, TextReader reader, bool isCloseTag)
        {
            var tagStr = new string((char) current, 1);

            while (current != '>')
            {
                current = reader.Read();
                tagStr += (char) current;
            }

            var tagParts = tagStr.Trim().Split(' ');

            var tag = isCloseTag ? tagParts[0].Replace("/", "") : tagParts[0];
            IEnumerable<string> attributes = null;

            if (tagParts.Length > 1)
            {
                tag = tag + ">";
                attributes = tagParts.Skip(1);
            }

            var htmlTag = htmlTags[tag];

            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    var attributeParts = attribute.Split('=');
                    htmlTag.Attributes.Add(new HtmlTagAttribute { Name = attributeParts[0], Value = attributeParts[1] });
                }
            }

            return htmlTag;
        }

        private static string ReadPlainText(char start, TextReader reader)
        {
            var sb = new StringBuilder().Append(start);

            while (true)
            {
                var current = reader.Peek();

                if (current == -1 || current == '<')
                    return sb.ToString();

                sb.Append((char) current);
                reader.Read();
            }
        }
    }
}