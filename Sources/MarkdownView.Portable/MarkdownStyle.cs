﻿namespace Xam.Forms.Markdown
{
    using Xamarin.Forms;

    public class MarkdownStyle
    {
        public FontAttributes Attributes { get; set; } = FontAttributes.None;

        public TextDecorations TextDecorations { get; set; } = TextDecorations.None;

        public float FontSize { get; set; } = 12;

        public Color ForegroundColor { get; set; } = Color.Black;

        public Color BackgroundColor { get; set; } = Color.Transparent;

        public Color BorderColor { get; set; }

        public float BorderSize { get; set; }

        public string FontFamily { get; set; }

        public Thickness Padding { get; set; } = new Thickness(10);
    }
}