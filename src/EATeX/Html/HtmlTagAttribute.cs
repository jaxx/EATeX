namespace EATeX.Html
{
    public struct HtmlTagAttribute
    {
        public HtmlTagAttributeType Type { get; set; }
        public string Value { get; set; }
    }

    public enum HtmlTagAttributeType
    {
        Color
    }
}