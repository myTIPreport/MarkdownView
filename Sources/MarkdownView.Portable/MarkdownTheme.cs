namespace Xam.Forms.Markdown
{
    using Xamarin.Forms;

    public class MarkdownTheme
    {
        public MarkdownTheme(
            int baseFontSize = 12)
        {
            Paragraph = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                FontSize = baseFontSize,
            };

            Heading1 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                BorderSize = 1,
                FontSize = baseFontSize + 14,
            };

            Heading2 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                BorderSize = 1,
                FontSize = baseFontSize + 10,
            };

            Heading3 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = baseFontSize + 8,
            };

            Heading4 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = baseFontSize + 6,
            };

            Heading5 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = baseFontSize + 4,
            };

            Heading6 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = baseFontSize + 2,
            };

            Link = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                FontSize = baseFontSize,
            };

            List = new ListMarkdownStyle
            {
                Attributes = FontAttributes.None,
                FontSize = baseFontSize,
            };

            Code = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                FontSize = baseFontSize,
            };

            Quote = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                BorderSize = 4,
                FontSize = baseFontSize,
                BackgroundColor = Color.Gray.MultiplyAlpha(.1),
            };

            Separator = new MarkdownStyle
            {
                BorderSize = 2,
            };

            // Platform specific properties
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Code.FontFamily = "Courier";
                    break;

                case Device.Android:
                    Code.FontFamily = "monospace";
                    break;
            }
        }

        public Color BackgroundColor { get; set; }

        public MarkdownStyle Paragraph { get; set; }

        public ListMarkdownStyle List { get; set; }

        public MarkdownStyle Heading1 { get; set; }

        public MarkdownStyle Heading2 { get; set; }

        public MarkdownStyle Heading3 { get; set; }

        public MarkdownStyle Heading4 { get; set; }

        public MarkdownStyle Heading5 { get; set; }

        public MarkdownStyle Heading6 { get; set; }

        public MarkdownStyle Quote { get; set; }

        public MarkdownStyle Separator { get; set; }

        public MarkdownStyle Link { get; set; }

        public MarkdownStyle Code { get; set; }

        /// <summary>
        /// Vertical spacing between markdown elements
        /// </summary>
        public float VerticalSpacing { get; set; } = 10;

        /// <summary>
        /// Padding for the Markdown container.
        /// </summary>
        public Thickness ControlPadding { get; set; } = new Thickness(10);
    }

    public class LightMarkdownTheme : MarkdownTheme
    {
        public LightMarkdownTheme(
            int baseFontSize = 12)
            : base(baseFontSize)
        {
            BackgroundColor = DefaultBackgroundColor;
            Paragraph.ForegroundColor = DefaultTextColor;
            Heading1.ForegroundColor = DefaultTextColor;
            Heading1.BorderColor = DefaultSeparatorColor;
            Heading2.ForegroundColor = DefaultTextColor;
            Heading2.BorderColor = DefaultSeparatorColor;
            Heading3.ForegroundColor = DefaultTextColor;
            Heading4.ForegroundColor = DefaultTextColor;
            Heading5.ForegroundColor = DefaultTextColor;
            Heading6.ForegroundColor = DefaultTextColor;
            Link.ForegroundColor = DefaultAccentColor;
            Code.ForegroundColor = DefaultTextColor;
            Code.BackgroundColor = DefaultCodeBackground;
            Quote.ForegroundColor = DefaultQuoteTextColor;
            Quote.BorderColor = DefaultQuoteBorderColor;
            Separator.BorderColor = DefaultSeparatorColor;
        }

        public static readonly Color DefaultBackgroundColor = Color.FromHex("#ffffff");

        public static readonly Color DefaultAccentColor = Color.FromHex("#0366d6");

        public static readonly Color DefaultTextColor = Color.FromHex("#24292e");

        public static readonly Color DefaultCodeBackground = Color.FromHex("#f6f8fa");

        public static readonly Color DefaultSeparatorColor = Color.FromHex("#eaecef");

        public static readonly Color DefaultQuoteTextColor = Color.FromHex("#6a737d");

        public static readonly Color DefaultQuoteBorderColor = Color.FromHex("#dfe2e5");
    }

    public class DarkMarkdownTheme : MarkdownTheme
    {
        public DarkMarkdownTheme(
            int baseFontSize = 12)
            : base(baseFontSize)
        {
            BackgroundColor = DefaultBackgroundColor;
            Paragraph.ForegroundColor = DefaultTextColor;
            Heading1.ForegroundColor = DefaultTextColor;
            Heading1.BorderColor = DefaultSeparatorColor;
            Heading2.ForegroundColor = DefaultTextColor;
            Heading2.BorderColor = DefaultSeparatorColor;
            Heading3.ForegroundColor = DefaultTextColor;
            Heading4.ForegroundColor = DefaultTextColor;
            Heading5.ForegroundColor = DefaultTextColor;
            Heading6.ForegroundColor = DefaultTextColor;
            Link.ForegroundColor = DefaultAccentColor;
            Code.ForegroundColor = DefaultTextColor;
            Code.BackgroundColor = DefaultCodeBackground;
            Quote.ForegroundColor = DefaultQuoteTextColor;
            Quote.BorderColor = DefaultQuoteBorderColor;
            Separator.BorderColor = DefaultSeparatorColor;
        }

        public static readonly Color DefaultBackgroundColor = Color.FromHex("#2b303b");

        public static readonly Color DefaultAccentColor = Color.FromHex("#d08770");

        public static readonly Color DefaultTextColor = Color.FromHex("#eff1f5");

        public static readonly Color DefaultCodeBackground = Color.FromHex("#4f5b66");

        public static readonly Color DefaultSeparatorColor = Color.FromHex("#65737e");

        public static readonly Color DefaultQuoteTextColor = Color.FromHex("#a7adba");

        public static readonly Color DefaultQuoteBorderColor = Color.FromHex("#a7adba");
    }
}