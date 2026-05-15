using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.VisualTree;
using Markdig;
using Markdig.SyntaxHighlighting;

namespace Barkdown.Editor;

public partial class MainWindow : Window
{
    private string ThemeColours { get; set; } = "body {background-color: white}";

    public MainWindow()
    {
        InitializeComponent();

        VersionText.Text = AppConsts.VERSION;

        var settings = MdRender.GetPlatformSettings();
        var colors = settings?.GetColorValues();

        // Check current theme
        if (colors?.ThemeVariant == PlatformThemeVariant.Dark)
        {
            ThemeColours = "body {background-color: black; color: white;}";
        }


        var blank = $"""
                      <!DOCTYPE html>
                      <html>
                      <head><style>{ThemeColours}</style></head>
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
            .UseSoftlineBreakAsHardlineBreak()
            .Build();

        var input = MdInput.Text ?? string.Empty;
        var html = Markdown.ToHtml(input, pipeline);
        var content = $"""
                        <!DOCTYPE html>
                        <html>
                        <head><style>{ThemeColours}</style></head>
                        <body><p>{html}</p></body>
                        </html>
                        """;
        MdRender.NavigateToString(content);
    }
}
