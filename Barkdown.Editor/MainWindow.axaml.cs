using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.VisualTree;
using Markdig;
using Markdig.SyntaxHighlighting;

namespace Barkdown.Editor;

public partial class MainWindow : Window
{
    private string ChangeTheme()
    {
        var settings = MdWindow.GetPlatformSettings();
        var colors = settings?.GetColorValues();

        const string darkTheme = """
        body {background-color: black; color: white;}
        a {color: cyan;}
        """;

        const string lightTheme = @"body {background-color: white;}";

        // Check current theme
        return colors?.ThemeVariant == PlatformThemeVariant.Dark ? darkTheme : lightTheme;
    }

    public MainWindow()
    {
        InitializeComponent();

        #if DEBUG
        VersionText.Text = $"{AppConsts.VERSION}-{ThisAssembly.Git.Commit}";
        #else
        VersionText.Text = AppConsts.VERSION;
        #endif

        var blank = $"""
                      <!DOCTYPE html>
                      <html>
                      <head><style>{ChangeTheme()}</style></head>
                      </html>
                      """;

        MdRender.NavigateToString(blank);
    }

    private void Markdown_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseSyntaxHighlighting()
            .UseEmojiAndSmiley()
            .UseYamlFrontMatter()
            .UseBootstrap()
            .UseSoftlineBreakAsHardlineBreak()
            .Build();
        var input = MdInput.Text ?? string.Empty;
        var html = Markdown.ToHtml(input, pipeline);
        var content = $"""
                        <!DOCTYPE html>
                        <html>
                        <head><style>{ChangeTheme()}</style></head>
                        <body><p>{html}</p></body>
                        </html>
                        """;

        MdRender.NavigateToString(content);
    }
}
