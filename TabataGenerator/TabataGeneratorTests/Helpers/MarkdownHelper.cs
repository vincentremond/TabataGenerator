using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;

namespace TabataGeneratorTests.Helpers
{
    public class MarkdownHelper
    {
        public static List<string> GetCodeBlocks(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var doc = new MarkdownDocument();
            doc.Parse(content);
            return doc.Blocks
                .OfType<CodeBlock>()
                .Select(c => c.Text)
                .Select(StringHelper.WindowsToUnixLineBreak)
                .ToList();
        }
    }
}
