using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EATeX.TempLaTex
{
    public class LatexTemplateReader
    {
        private FileStream stream;

        private byte[] placeholderStart = Encoding.UTF8.GetBytes("%<");
        private byte[] placeholderEnd = Encoding.UTF8.GetBytes(">%");

        private Dictionary<string, string> placeholders = new Dictionary<string, string>();

        public LatexTemplateReader(string templatePath)
        {
            stream = new FileStream(templatePath, FileMode.Open);
        }
    }
}