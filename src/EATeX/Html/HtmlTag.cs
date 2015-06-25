using System.Collections.Generic;

namespace EATeX.Html
{
    public class HtmlTag
    {
        public HtmlTagType Type { get; set; }
        public List<HtmlTagAttribute> Attributes { get; set; }

        public HtmlTag()
        {
            Attributes = new List<HtmlTagAttribute>();
        }

        public bool IsList
        {
            get { return Type == HtmlTagType.OrderedList || Type == HtmlTagType.UnorderedList; }
        }

        public bool IsOrderedList
        {
            get { return Type == HtmlTagType.OrderedList; }
        }

        public bool IsUnorderedList
        {
            get { return Type == HtmlTagType.UnorderedList; }
        }

        public bool IsListItem
        {
            get { return Type == HtmlTagType.ListItem; }
        }
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