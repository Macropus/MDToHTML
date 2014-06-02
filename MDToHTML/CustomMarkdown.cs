using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDToHTML
{
    class CustomMarkdown : MarkdownDeep.Markdown
    {
        public override void OnPrepareLink(MarkdownDeep.HtmlTag tag)
        {
            base.OnPrepareLink(tag);

            // Append .html to the href
            if (tag.attributes.ContainsKey("href") && !tag.attributes["href"].EndsWith(".html"))
            {
                tag.attributes["href"] = string.Format("{0}.html", tag.attributes["href"]);
            }
        }
    }
}
