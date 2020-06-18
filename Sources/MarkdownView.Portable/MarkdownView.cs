namespace Xam.Forms.Markdown
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Extensions;
    using Markdig;
    using Markdig.Syntax;
    using Markdig.Syntax.Inlines;
    using Xamarin.Forms;

    public class MarkdownView : ContentView
    {
        public Action<string> NavigateToLink { get; set; } = (s) => Device.OpenUri(new Uri(s));

        public static MarkdownTheme Global = new LightMarkdownTheme();

        public string Markdown
        {
            get => (string)GetValue(MarkdownProperty);
            set => SetValue(MarkdownProperty, value);
        }

        public static readonly BindableProperty MarkdownProperty = BindableProperty.Create(nameof(Markdown), typeof(string), typeof(MarkdownView), null, propertyChanged: OnMarkdownChanged);

        public string RelativeUrlHost
        {
            get => (string)GetValue(RelativeUrlHostProperty);
            set => SetValue(RelativeUrlHostProperty, value);
        }

        public static readonly BindableProperty RelativeUrlHostProperty = BindableProperty.Create(nameof(RelativeUrlHost), typeof(string), typeof(MarkdownView), null, propertyChanged: OnMarkdownChanged);

        public MarkdownTheme Theme
        {
            get => (MarkdownTheme)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }

        public static readonly BindableProperty ThemeProperty = BindableProperty.Create(nameof(Theme), typeof(MarkdownTheme), typeof(MarkdownView), Global, propertyChanged: OnMarkdownChanged);

        private bool isQuoted;

        private readonly List<View> queuedViews = new List<View>();

        private static void OnMarkdownChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MarkdownView view = bindable as MarkdownView;
            view.RenderMarkdown();
        }

        private StackLayout stack;

        private List<KeyValuePair<string, string>> links = new List<KeyValuePair<string, string>>();

        private void RenderMarkdown()
        {
            stack = new StackLayout()
            {
                Spacing = Theme.VerticalSpacing,
            };

            Padding = Theme.ControlPadding;

            BackgroundColor = Theme.BackgroundColor;

            if (!string.IsNullOrEmpty(Markdown))
            {
                MarkdownDocument parsed = Markdig.Markdown.Parse(Markdown, s_pipeline);
                Render(parsed.AsEnumerable());
            }

            Content = stack;
        }

        private static readonly MarkdownPipeline s_pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        private void Render(IEnumerable<Block> blocks)
        {
            foreach (Block block in blocks)
            {
                Render(block);
            }
        }

        private void AttachLinks(View view)
        {
            if (links.Any())
            {
                List<KeyValuePair<string, string>> blockLinks = links;
                view.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        try
                        {
                            if (blockLinks.Count > 1)
                            {
                                string result = await Application.Current.MainPage.DisplayActionSheet("Open link", "Cancel", null, blockLinks.Select(x => x.Key).ToArray());
                                KeyValuePair<string, string> link = blockLinks.FirstOrDefault(x => x.Key == result);
                                NavigateToLink(link.Value);
                            }
                            else
                            {
                                NavigateToLink(blockLinks.First().Value);
                            }
                        }
                        catch (Exception) { }
                    }),
                });

                links = new List<KeyValuePair<string, string>>();
            }
        }

        #region Rendering blocks

        private void Render(Block block)
        {
            switch (block)
            {
                case HeadingBlock heading:
                    Render(heading);
                    break;

                case ParagraphBlock paragraph:
                    Render(paragraph);
                    break;

                case QuoteBlock quote:
                    Render(quote);
                    break;

                case CodeBlock code:
                    Render(code);
                    break;

                case ListBlock list:
                    Render(list);
                    break;

                case ThematicBreakBlock thematicBreak:
                    Render(thematicBreak);
                    break;

                case HtmlBlock html:
                    Render(html);
                    break;

                default:
                    Debug.WriteLine($"Can't render {block.GetType()} blocks.");
                    break;
            }

            if (queuedViews.Any())
            {
                foreach (View view in queuedViews)
                {
                    stack.Children.Add(view);
                }
                queuedViews.Clear();
            }
        }

        private int listScope;

        private void Render(ThematicBreakBlock block)
        {
            MarkdownStyle style = Theme.Separator;

            if (style.BorderSize > 0)
            {
                stack.Children.Add(new BoxView
                {
                    HeightRequest = style.BorderSize,
                    BackgroundColor = style.BorderColor,
                });
            }
        }

        private void Render(ListBlock block)
        {
            listScope++;

            for (int i = 0; i < block.Count(); i++)
            {
                Block item = block.ElementAt(i);

                if (item is ListItemBlock itemBlock)
                {
                    Render(block, i + 1, itemBlock);
                }
            }

            listScope--;
        }

        private void Render(ListBlock parent, int index, ListItemBlock block)
        {
            StackLayout initialStack = stack;

            stack = new StackLayout()
            {
                Spacing = Theme.List.VerticalSpacing,
            };

            Render(block.AsEnumerable());

            StackLayout horizontalStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(listScope * Theme.List.Indentation, 0, 0, 0),
            };

            View bullet;

            if (parent.IsOrdered)
            {
                bullet = new Label
                {
                    Text = $"{index}.",
                    FontSize = Theme.Paragraph.FontSize,
                    TextColor = Theme.Paragraph.ForegroundColor,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.End,
                };
            }
            else
            {
                bullet = new BoxView
                {
                    WidthRequest = 4,
                    HeightRequest = 4,
                    Margin = new Thickness(0, 6, 0, 0),
                    BackgroundColor = Theme.Paragraph.ForegroundColor,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Center,
                };
            }

            horizontalStack.Children.Add(bullet);

            horizontalStack.Children.Add(stack);
            initialStack.Children.Add(horizontalStack);

            stack = initialStack;
        }

        private void Render(HeadingBlock block)
        {
            MarkdownStyle style;

            switch (block.Level)
            {
                case 1:
                    style = Theme.Heading1;
                    break;

                case 2:
                    style = Theme.Heading2;
                    break;

                case 3:
                    style = Theme.Heading3;
                    break;

                case 4:
                    style = Theme.Heading4;
                    break;

                case 5:
                    style = Theme.Heading5;
                    break;

                default:
                    style = Theme.Heading6;
                    break;
            }

            Color foregroundColor = isQuoted ? Theme.Quote.ForegroundColor : style.ForegroundColor;

            Label label = new Label
            {
                FormattedText = CreateFormatted(block.Inline, style.FontFamily, style.Attributes, foregroundColor, style.BackgroundColor, style.FontSize),
            };

            AttachLinks(label);

            if (style.BorderSize > 0)
            {
                StackLayout headingStack = new StackLayout();
                headingStack.Children.Add(label);
                headingStack.Children.Add(new BoxView
                {
                    HeightRequest = style.BorderSize,
                    BackgroundColor = style.BorderColor,
                });
                stack.Children.Add(headingStack);
            }
            else
            {
                stack.Children.Add(label);
            }
        }

        private void Render(ParagraphBlock block)
        {
            MarkdownStyle style = Theme.Paragraph;
            Color foregroundColor = isQuoted ? Theme.Quote.ForegroundColor : style.ForegroundColor;
            Label label = new Label
            {
                FormattedText = CreateFormatted(block.Inline, style.FontFamily, style.Attributes, foregroundColor, style.BackgroundColor, style.FontSize),
            };
            AttachLinks(label);
            stack.Children.Add(label);
        }

        private void Render(HtmlBlock block)
        {
            // ?
        }

        private void Render(QuoteBlock block)
        {
            bool initialIsQuoted = isQuoted;
            StackLayout initialStack = stack;

            isQuoted = true;
            stack = new StackLayout()
            {
                Spacing = Theme.VerticalSpacing,
            };

            MarkdownStyle style = Theme.Quote;

            if (style.BorderSize > 0)
            {
                StackLayout horizontalStack = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    BackgroundColor = Theme.Quote.BackgroundColor,
                };

                horizontalStack.Children.Add(new BoxView()
                {
                    WidthRequest = style.BorderSize,
                    BackgroundColor = style.BorderColor,
                });

                horizontalStack.Children.Add(stack);
                initialStack.Children.Add(horizontalStack);
            }
            else
            {
                stack.BackgroundColor = Theme.Quote.BackgroundColor;
                initialStack.Children.Add(stack);
            }

            Render(block.AsEnumerable());

            isQuoted = initialIsQuoted;
            stack = initialStack;
        }

        private void Render(CodeBlock block)
        {
            MarkdownStyle style = Theme.Code;
            Label label = new Label
            {
                TextColor = style.ForegroundColor,
                FontAttributes = style.Attributes,
                FontFamily = style.FontFamily,
                FontSize = style.FontSize,
                Text = string.Join(Environment.NewLine, block.Lines),
            };
            stack.Children.Add(new Frame()
            {
                CornerRadius = 3,
                HasShadow = false,
                Padding = style.Padding,
                BackgroundColor = style.BackgroundColor,
                Content = label
            });
        }

        private FormattedString CreateFormatted(ContainerInline inlines, string family, FontAttributes attributes, Color foregroundColor, Color backgroundColor, float size)
        {
            FormattedString fs = new FormattedString();

            foreach (Inline inline in inlines)
            {
                Span[] spans = CreateSpans(inline, family, attributes, foregroundColor, backgroundColor, size);
                if (spans != null)
                {
                    foreach (Span span in spans)
                    {
                        fs.Spans.Add(span);
                    }
                }
            }

            return fs;
        }

        private Span[] CreateSpans(Inline inline, string family, FontAttributes attributes, Color foregroundColor, Color backgroundColor, float size)
        {
            switch (inline)
            {
                case LiteralInline literal:
                    return new[]
                    {
                        new Span
                        {
                            Text = literal.Content.Text.Substring(literal.Content.Start, literal.Content.Length),
                            FontAttributes = attributes,
                            ForegroundColor = foregroundColor,
                            BackgroundColor = backgroundColor,
                            FontSize = size,
                            FontFamily = family,
                        }
                    };

                case EmphasisInline emphasis:
                    FontAttributes childAttributes = attributes | (emphasis.IsDouble ? FontAttributes.Bold : FontAttributes.Italic);
                    return emphasis.SelectMany(x => CreateSpans(x, family, childAttributes, foregroundColor, backgroundColor, size)).ToArray();

                case LineBreakInline breakline:
                    return new[] { new Span { Text = "\n" } };

                case LinkInline link:

                    string url = link.Url;

                    if (!(url.StartsWith("http://") || url.StartsWith("https://")))
                    {
                        url = $"{RelativeUrlHost?.TrimEnd('/')}/{url.TrimStart('/')}";
                    }

                    if (link.IsImage)
                    {
                        Image image = new Image();

                        if (Path.GetExtension(url) == ".svg")
                        {
                            image.RenderSvg(url);
                        }
                        else
                        {
                            image.Source = url;
                        }

                        queuedViews.Add(image);
                        return new Span[0];
                    }
                    else
                    {
                        Span[] spans = link.SelectMany(x => CreateSpans(x, Theme.Link.FontFamily ?? family, Theme.Link.Attributes, Theme.Link.ForegroundColor, Theme.Link.BackgroundColor, size)).ToArray();
                        links.Add(new KeyValuePair<string, string>(string.Join("", spans.Select(x => x.Text)), url));
                        return spans;
                    }

                case CodeInline code:
                    return new[]
                    {
                        new Span()
                        {
                            Text="\u2002",
                            FontSize = size,
                            FontFamily = Theme.Code.FontFamily,
                            ForegroundColor = Theme.Code.ForegroundColor,
                            BackgroundColor = Theme.Code.BackgroundColor
                        },
                        new Span
                        {
                            Text = code.Content,
                            FontAttributes = Theme.Code.Attributes,
                            FontSize = size,
                            FontFamily = Theme.Code.FontFamily,
                            ForegroundColor = Theme.Code.ForegroundColor,
                            BackgroundColor = Theme.Code.BackgroundColor
                        },
                        new Span()
                        {
                            Text="\u2002",
                            FontSize = size,
                            FontFamily = Theme.Code.FontFamily,
                            ForegroundColor = Theme.Code.ForegroundColor,
                            BackgroundColor = Theme.Code.BackgroundColor
                        },
                    };

                default:
                    Debug.WriteLine($"Can't render {inline.GetType()} inlines.");
                    return null;
            }
        }

        #endregion Rendering blocks
    }
}