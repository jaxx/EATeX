using System.Collections.Generic;

namespace EATeX.Html
{
    public struct HtmlTag
    {
        public HtmlTagType Type { get; set; }
        public List<HtmlTagAttribute> Attributes { get; set; }
    }

    public enum HtmlTagType
    {
        Bold,
        Italic,
        Underline,
        Font,
        Hyperlink,
        OrderedList,
        UnorderedList,
        ListItem,
        Superscript,
        Subscript
    }
}