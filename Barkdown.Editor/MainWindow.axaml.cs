using Markdig;
using Markdig.SyntaxHighlighting;

namespace Barkdown.Editor;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

#if DEBUG
        VersionText.Text = $"{AppConsts.VERSION}-{ThisAssembly.Git.Commit}";
#else
        VersionText.Text = AppConsts.VERSION;
#endif

        TPawImage.Source = IsDarkTheme switch
        {
            true => ImageHelper.LoadFromResource(new Uri("avares://Barkdown.Editor/Assets/tpaw-white.png")),
            _ => ImageHelper.LoadFromResource(new Uri("avares://Barkdown.Editor/Assets/tpaw.png"))
        };

        var blank = $"""
                     <!DOCTYPE html>
                     <html>
                     {HtmlHeader}
                     <body></body>
                     </html>
                     """;

        MdRender.NavigateToString(blank);
    }

    private string HtmlHeader => $"<head><style>{HtmlStyle()}</style></head>";

    private bool IsDarkTheme
    {
        get
        {
            var settings = MdWindow.GetPlatformSettings();
            var colors = settings?.GetColorValues();

            return colors?.ThemeVariant == PlatformThemeVariant.Dark;
        }
    }

    private string HtmlStyle()
    {
        const string darkTheme = """
                                 body {background-color: black; color: white;}
                                 a {color: cyan;}
                                 """;

        const string lightTheme = @"body {background-color: white;}";

        // Check current theme
        return IsDarkTheme ? darkTheme : lightTheme;
    }

    private void Markdown_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseSyntaxHighlighting()
            .UseEmojiAndSmiley()
            .UseYamlFrontMatter()
            .UseSoftlineBreakAsHardlineBreak()
            .Build();

        var input = MdInput.Text ?? string.Empty;
        var html = Markdown.ToHtml(input, pipeline);
        var content = $"""
                       <!DOCTYPE html>
                       <html>
                       {HtmlHeader}
                       <body><div>{html}</div></body>
                       </html>
                       """;

        if (!string.IsNullOrEmpty(input))
        {
            MdRender.NavigateToString(content);
        }
        else
        {
            MdRender.NavigateToString(string.Empty);
            MdRender.IsEnabled = false;
        }
    }
}
