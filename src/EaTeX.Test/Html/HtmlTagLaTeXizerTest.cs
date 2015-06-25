using EATeX.Html;
using NUnit.Framework;

namespace EaTeX.Test.Html
{
    public class HtmlTagLaTeXizerTest
    {
        [Test]
        public void EmptyStringTest()
        {
            const string text = "";
            Assert.AreEqual(text, HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void EmptyStringWithWhitespaceTest()
        {
            const string text = " ";
            Assert.AreEqual(text, HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void NoTagsTest()
        {
            const string text = "Text without tags.";
            Assert.AreEqual(text, HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void OneSimpleTagTest()
        {
            var text = "<b>Bold</b> text.";
            Assert.AreEqual(@"\textbf{Bold} text.", HtmlTagLaTeXizer.Latexize(text));

            text = "Text is <b>bold</b>.";
            Assert.AreEqual(@"Text is \textbf{bold}.", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void ManySimpleTagsTest()
        {
            var text = "<b><u>Bold underlined</u></b> and <i>italic</i> text.";
            Assert.AreEqual(@"\textbf{\underline{Bold underlined}} and \textit{italic} text.", HtmlTagLaTeXizer.Latexize(text));

            text = "Text is <b><u><i>bold, underlined and italic</i></u></b>.";
            Assert.AreEqual(@"Text is \textbf{\underline{\textit{bold, underlined and italic}}}.", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void SimpleListTest()
        {
            var text = "<ol><li>Item1</li><li>Item2</li><li>Item3</li></ol>";
            Assert.AreEqual(@"\begin{enumerate}\item Item1\item Item2\item Item3\end{enumerate}", HtmlTagLaTeXizer.Latexize(text));

            text = "<ul><li>Item1</li><li>Item2</li><li>Item3</li></ul>";
            Assert.AreEqual(@"\begin{itemize}\item Item1\item Item2\item Item3\end{itemize}", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void BoldListTest()
        {
            var text = "<b><ol><li>Item1</li><li>Item2</li><li>Item3</li></ol></b>";
            Assert.AreEqual(@"\textbf{\begin{enumerate}\item Item1\item Item2\item Item3\end{enumerate}}", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void ListWithOneBoldItemTest()
        {
            var text = "<ol><li>Item1</li><li><b>Item2</b></li><li>Item3</li></ol>";
            Assert.AreEqual(@"\begin{enumerate}\item Item1\item \textbf{Item2}\item Item3\end{enumerate}", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void SubscriptTest()
        {
            var text = "10<sub>2</sub>";
            Assert.AreEqual(@"10$_{2}$", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void SuperscriptTest()
        {
            var text = "10<sup>2</sup>";
            Assert.AreEqual(@"10$^{2}$", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void EmptyTagTest()
        {
            var text = "<b></b>";
            Assert.AreEqual("", HtmlTagLaTeXizer.Latexize(text));

            text = "Text is <b><u><i></i></u></b>normal.";
            Assert.AreEqual("Text is normal.", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void TagWithWhitespacesTest()
        {
            const string text = "<b>   </b>";
            Assert.AreEqual("   ", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test]
        public void ALotOfTagsTest()
        {
            const string text = @"<b><u>I.</u></b><b><u>E-Toimikust:</u></b><b><u></u></b><b><i><u>&lt;E-Toimik</u></i></b><i><u> on</u></i><u> &lt;/teenusepõhine&gt;</u> infosüsteem.";
            Assert.AreEqual(@"\textbf{\underline{I.}}\textbf{\underline{E-Toimikust:}}\textbf{\textit{\underline{&lt;E-Toimik}}}\textit{\underline{ on}}\underline{ &lt;/teenusepõhine&gt;} infosüsteem.", HtmlTagLaTeXizer.Latexize(text));
        }

        [Test, ExpectedException(ExpectedMessage = "Missing closing tags", MatchType = MessageMatch.Contains)]
        public void TagWithoutClosingTagTest()
        {
            const string text = "<b>Something wrong";
            HtmlTagLaTeXizer.Latexize(text);
        }

        [Test, ExpectedException(ExpectedMessage = "Missing opening tags", MatchType = MessageMatch.Contains)]
        public void TagWithoutOpeningTagTest()
        {
            const string text = "Something wrong</b>";
            HtmlTagLaTeXizer.Latexize(text);
        }

        [Test, ExpectedException(ExpectedMessage = "Order of open and close tags does not match", MatchType = MessageMatch.Contains)]
        public void WrongOrderOfTagsTest()
        {
            const string text = "<b><u>Bold text</b></u>";
            HtmlTagLaTeXizer.Latexize(text);
        }
    }
}