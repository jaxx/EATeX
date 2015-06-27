using EATeX.Core;
using NUnit.Framework;

namespace EaTeX.Test
{
    [TestFixture]
    public class HtmlLatexizerTest
    {
        [Test]
        public void EmptyStringTest()
        {
            Assert.AreEqual("", HtmlLatexizer.Latexize(""));
        }

        [Test]
        public void StringWithOnlyWhitespacesTest()
        {
            Assert.AreEqual("   ", HtmlLatexizer.Latexize("   "));
        }

        [Test]
        public void StringWithoutTagsTest()
        {
            Assert.AreEqual("Normal text.", HtmlLatexizer.Latexize("Normal text."));
        }

        [Test]
        public void EmptyTagTest()
        {
            Assert.AreEqual("", HtmlLatexizer.Latexize("<b></b>"));
            Assert.AreEqual("Text is normal.", HtmlLatexizer.Latexize("Text is <b><u><i></i></u></b>normal."));
        }

        [Test]
        public void TagWithWhitespacesTest()
        {
            Assert.AreEqual("   ", HtmlLatexizer.Latexize("<b>   </b>"));
        }

        [Test]
        public void SimpleSingleTagTest()
        {
            Assert.AreEqual(@"\textbf{Bold} text.", HtmlLatexizer.Latexize("<b>Bold</b> text."));
            Assert.AreEqual(@"This text is \underline{underlined}.", HtmlLatexizer.Latexize("This text is <u>underlined</u>."));
            Assert.AreEqual(@"This text is \textit{italic} and \textbf{bold} also.", HtmlLatexizer.Latexize("This text is <i>italic</i> and <b>bold</b> also."));
        }

        [Test]
        public void TagWithWhitespacesAndBoldTextTest()
        {
            Assert.AreEqual(@"\textbf{Bold} text ...   .", HtmlLatexizer.Latexize("<b>Bold</b> text ...<b>   </b>."));
        }

        [Test]
        public void SimpleMultipleTagsTest()
        {
            Assert.AreEqual(@"\textbf{\textit{Bold and italic}} text.", HtmlLatexizer.Latexize("<b><i>Bold and italic</i></b> text."));
            Assert.AreEqual(@"\textbf{\textit{\underline{Bold and italic and underlined}}} text.", HtmlLatexizer.Latexize("<b><i><u>Bold and italic and underlined</u></i></b> text."));
        }

        [Test]
        public void SimpleListTest()
        {
            Assert.AreEqual(@"\begin{enumerate}\item Item1\item Item2\item Item3\end{enumerate}", HtmlLatexizer.Latexize("<ol><li>Item1</li><li>Item2</li><li>Item3</li></ol>"));
            Assert.AreEqual(@"\begin{itemize}\item Item1\item Item2\item Item3\end{itemize}", HtmlLatexizer.Latexize("<ul><li>Item1</li><li>Item2</li><li>Item3</li></ul>"));
        }

        [Test]
        public void BoldListTest()
        {
            Assert.AreEqual(@"\textbf{\begin{itemize}\item Item1\item Item2\item Item3\end{itemize}}", HtmlLatexizer.Latexize("<b><ul><li>Item1</li><li>Item2</li><li>Item3</li></ul></b>"));
        }

        [Test]
        public void BoldListItemTest()
        {
            Assert.AreEqual(@"\begin{itemize}\item Item1\item Item2\item \textbf{Item3}\end{itemize}", HtmlLatexizer.Latexize("<ul><li>Item1</li><li>Item2</li><li><b>Item3</b></li></ul>"));
        }

        [Test]
        public void SubscriptTest()
        {
            Assert.AreEqual(@"10$_{2}$", HtmlLatexizer.Latexize("10<sub>2</sub>"));
        }

        [Test]
        public void SuperscriptTest()
        {
            Assert.AreEqual(@"10$^{2}$", HtmlLatexizer.Latexize("10<sup>2</sup>"));
        }

        [Test]
        public void FontColorTest()
        {
            Assert.AreEqual(@"{\color[HTML]{0000FF}This text is blue.}", HtmlLatexizer.Latexize("<font color=\"#0000FF\">This text is blue.</font>"));
        }

        [Test]
        public void BoldRedFontTest()
        {
            Assert.AreEqual(@"{\color[HTML]{FF0000}\textbf{This text is red.}}", HtmlLatexizer.Latexize("<font color=\"#FF0000\"><b>This text is red.</b></font>"));
        }

        [Test]
        public void ShitloadOfTagsTest()
        {
            Assert.AreEqual(@"\textbf{\underline{I.}}\textbf{\underline{E-Toimikust:}}\textbf{\textit{\underline{&lt;E-Toimik}}}\textit{\underline{ on}}\underline{ &lt;/teenusepõhine&gt;} infosüsteem.", HtmlLatexizer.Latexize("<b><u>I.</u></b><b><u>E-Toimikust:</u></b><b><u></u></b><b><i><u>&lt;E-Toimik</u></i></b><i><u> on</u></i><u> &lt;/teenusepõhine&gt;</u> infosüsteem."));
        }

        [Test]
        public void HyperlinkTest()
        {
            Assert.AreEqual(@"\href{http://www.google.com/}{This text is link.}", HtmlLatexizer.Latexize("<a href=\"http://www.google.com/\">This text is link.</a>"));
        }

        [Test]
        public void RedAndBoldHyperlinkTest()
        {
            Assert.AreEqual(@"{\color[HTML]{FF0000}\textbf{\href{http://www.google.com/}{Bold and red link.}}}", HtmlLatexizer.Latexize("<font color=\"#FF0000\"><b><a href=\"http://www.google.com/\">Bold and red link.</a></b></font>"));
        }
    }
}