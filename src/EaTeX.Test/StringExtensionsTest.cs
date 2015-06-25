using EATeX;
using NUnit.Framework;

namespace EaTeX.Test
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void EmptyStringTest()
        {
            const string text = "";
            Assert.AreEqual(text, text.ReplaceTags());
        }

        [Test]
        public void EmptyStringWithWhitespaceTest()
        {
            const string text = " ";
            Assert.AreEqual(text, text.ReplaceTags());
        }

        [Test]
        public void NoTagsTest()
        {
            const string text = "Text without tags.";
            Assert.AreEqual(text, text.ReplaceTags());
        }

        [Test]
        public void OneTagTest()
        {
            const string text = "This text is <u>underlined</u>.";
            Assert.AreEqual(@"This text is \underline{underlined}.", text.ReplaceTags());
        }

        [Test]
        public void ManyTagsTest()
        {
            const string text = "This text is <b><u>bold and underlined</u></b>.";
            Assert.AreEqual(@"This text is \textbf{\underline{bold and underlined}}.", text.ReplaceTags());
        }

        [Test]
        public void TagsWithoutContentTest()
        {
            const string text = "<b></b>";
            Assert.AreEqual(string.Empty, text.ReplaceTags());
        }

        [Test]
        public void TagsWithWhitespaceTest()
        {
            const string text = "<b>   </b>";
            Assert.AreEqual("   ", text.ReplaceTags());
        }

        [Test]
        public void ALotOfTagsTest()
        {
            const string text = @"<b><u>I.</u></b><b><u>E-Toimikust:</u></b><b><u></u></b><b><i><u>&lt;E-Toimik</u></i></b><i><u> on</u></i><u> &lt;/teenusepõhine&gt;</u> infosüsteem.";
            Assert.AreEqual(@"\textbf{\underline{I.}}\textbf{\underline{E-Toimikust:}}\textbf{\textit{\underline{&lt;E-Toimik}}}\textit{\underline{ on}}\underline{ &lt;/teenusepõhine&gt;} infosüsteem.", text.ReplaceTags());
        }

        [Test]
        [ExpectedException(ExpectedMessage = "Missing closing tags", MatchType = MessageMatch.Contains)]
        public void OpenTagWithoutClosingTagTest()
        {
            const string text = "<b>Something wrong";
            text.ReplaceTags();
        }

        [Test]
        [ExpectedException(ExpectedMessage = "Missing opening tags", MatchType = MessageMatch.Contains)]
        public void CloseTagWithoutOpenTagTest()
        {
            const string text = "Something wrong</b>";
            text.ReplaceTags();
        }

        [Test]
        [ExpectedException(ExpectedMessage = "Invalid tag - unexpected end", MatchType = MessageMatch.Contains)]
        public void InvalidTagUnexpectedEndTest()
        {
            const string text = "<bBold text</b>";
            text.ReplaceTags();
        }

        [Test]
        [ExpectedException(ExpectedMessage = "Invalid tag - unexpected start", MatchType = MessageMatch.Contains)]
        public void InvalidTagUnexpectedStartTest()
        {
            const string text = "b>Bold text</b>";
            text.ReplaceTags();
        }

        [Test]
        [ExpectedException(ExpectedMessage = "Order of open and close tags does not match", MatchType = MessageMatch.Contains)]
        public void WrongOrderOfTagsTest()
        {
            const string text = "<b><u>Bold text</b></u>";
            text.ReplaceTags();
        }
    }
}