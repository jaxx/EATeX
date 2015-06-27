using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EATeX
{
    public static class HtmlLatexizer
    {
        private static readonly Dictionary<string, string> startTags = new Dictionary<string, string>
        {
            { "<b>", "\\textbf{" },
            { "<i>", "\\textit{" },
            { "<u>", "\\underline{" },
            { "<sup>", "$^{" },
            { "<sub>", "$_{" },
            { "<ul>", "\\begin{itemize}" },
            { "<ol>", "\\begin{enumerate}" },
            { "<li>", "\\item " },
            { "<font>+color", "{\\color[HTML]{0}" },
            { "<a>+href", "\\href{0}{" }
        };

        private static readonly Dictionary<string, string> endTags = new Dictionary<string, string>
        {
            { "</b>", "}" },
            { "</i>", "}" },
            { "</u>", "}" },
            { "</sup>", "}$" },
            { "</sub>", "}$" },
            { "</ul>", "\\end{itemize}" },
            { "</ol>", "\\end{enumerate}" },
            { "</li>", string.Empty },
            { "</font>", "}" },
            { "</a>", "}" }
        };

        public static string Latexize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var latexizedResult = new StringBuilder();
            var tagResult = new StringBuilder();

            var tagsOpen = 0;
            var tempValue = string.Empty;

            using (var reader = new StringReader(text))
            {
                while (true)
                {
                    var current = reader.Read();

                    if (current == -1)
                        break;

                    if (current == '<')
                    {
                        bool isEndTag;
                        ReadTag(current, reader, tagResult, out isEndTag);

                        if (isEndTag)
                            tagsOpen--;
                        else
                            tagsOpen++;

                        if (tagsOpen != 0)
                            continue;

                        if (string.IsNullOrEmpty(tempValue))
                            tagResult.Clear();
                        if (IsWhitespacesOnly(tempValue))
                        {
                            latexizedResult.Append(tempValue);

                            tagResult.Clear();
                            tempValue = "";
                        }
                        else
                        {
                            latexizedResult.Append(tagResult);

                            tagResult.Clear();
                            tempValue = "";
                        }
                    }
                    else
                    {
                        tempValue = ReadText(current, reader);

                        if (tagsOpen > 0)
                            tagResult.Append(tempValue);
                        else
                        {
                            latexizedResult.Append(tempValue);
                            tempValue = "";
                        }
                    }
                }
            }

            return latexizedResult.ToString();
        }

        private static void ReadTag(int current, TextReader reader, StringBuilder result, out bool isEndTag)
        {
            var tag = new string((char)current, 1);

            while (true)
            {
                if (current == '>')
                    break;

                current = reader.Read();
                tag += (char)current;
            }

            isEndTag = tag.Contains("</");

            string[] attributes;
            ReadTagAttributes(ref tag, out attributes);
            ReplaceTagWithLatexCommand(tag, attributes, result, isEndTag);
        }

        private static void ReadTagAttributes(ref string tag, out string[] attributes)
        {
            attributes = null;
            var tagParts = tag.Split(' ');

            if (tagParts.Length == 1)
                return;

            attributes = new string[tagParts.Length - 1];

            tag = tagParts[0] + ">";
            tagParts[tagParts.Length - 1] = tagParts[tagParts.Length - 1].Replace(">", "");

            var attrPos = 0;
            for (var i = 1; i < tagParts.Length; i++)
                attributes[attrPos++] = tagParts[i].Replace("\"", "").Replace("#", "");
        }

        private static string ReadText(int current, TextReader reader)
        {
            var text = new string((char)current, 1);

            while (true)
            {
                var next = reader.Peek();

                if (next == -1 || next == '<')
                    break;

                current = reader.Read();
                text += (char)current;
            }

            return text;
        }

        private static void ReplaceTagWithLatexCommand(string tag, string[] attributes, StringBuilder result, bool isEndTag)
        {
            if (!isEndTag)
            {
                if (attributes == null)
                    result.Append(startTags[tag]);
                else
                {
                    foreach (var attributeParts in attributes.Select(a => a.Split('=')))
                    {
                        result.Append(startTags[string.Format("{0}+{1}", tag, attributeParts[0])])
                            .Replace("{0}", string.Format("{{{0}}}", attributeParts[1]));
                    }
                }
            }
            else
                result.Append(endTags[tag]);
        }

        private static bool IsWhitespacesOnly(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            return text.Trim().Length == 0;
        }
    }
}