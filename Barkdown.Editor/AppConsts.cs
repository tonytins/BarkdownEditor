namespace Barkdown.Editor;

public struct AppConsts
{
    // Ignore VSCode if it complains about "ThisAssembly" not being found.
    public const string VERSION =
        $"{ThisAssembly.Git.SemVer.Major}.{ThisAssembly.Git.SemVer.Minor}.{ThisAssembly.Git.SemVer.Patch}";
}
