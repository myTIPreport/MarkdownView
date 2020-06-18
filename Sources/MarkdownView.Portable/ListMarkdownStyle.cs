namespace Xam.Forms.Markdown
{
    public class ListMarkdownStyle : MarkdownStyle
    {
        public float VerticalSpacing { get; set; } = 10;

        /// <summary>
        /// The amount of indentation each list item gets. Note, for sub-lists, the indentation
        /// is multiplied by the level of child.
        /// </summary>
        public float Indentation { get; set; } = 10;
    }
}