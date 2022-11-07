using CommandDotNet;
using IOHelper;

namespace BToolsCLI;

public partial class Commands
{
  [Command(Description = "Rename files using regex replace.")]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
  public void RenameFiles(string source, string? searchPattern = null, string? replaceWith = null, bool recursive = false)
  {
    IOHelper.Path.TryGetMappedDrivePath(ref source);

    DirectoryInfo CurDir = new(source);

    if (searchPattern == null)
    {
      Console.Write(
        $"\nRename files in \"{CurDir.Name}\" using regex pattern." +
        $"\n" +
        $"\n>> Regex pattern to replace: "
      );

      searchPattern = Console.ReadLine() ?? "";
    }

    if (replaceWith == null)
    {
      Console.Write($"\n>> Replace with: ");

      replaceWith = IOHelper.File.SanitizeFilename(Console.ReadLine() ?? "");
    }

    // Summarize .. 
    Console.WriteLine(
      $"\n" +
      $"\nSummary .." +
      $"\n" +
      $"\nIn:      {CurDir.Name}" +
      $"\nReplace: /{searchPattern}/" +
      $"\nWith:    {replaceWith}" +
      $"\n" +
      $"\n{(recursive ? "recursively" : "in this folder only")}" +
      $"\n" +
      $"\n>> Hit any key to proceed .."
    );
    Console.ReadKey(true);

    CurDir.RenameItems(searchPattern, replaceWith, IOHelper.Directory.IOType.File, recursive);
  }

  [Command(Description = "Rename directories using regex replace.")]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
  public void RenameDirectories(
    string path,
    string? searchPattern = null,
    string? replaceWith = null,
    bool recursive = false)
  {
    IOHelper.Path.TryGetMappedDrivePath(ref path);

    DirectoryInfo CurDir = new(path);

    if (searchPattern == null)
    {
      Console.Write(
        $"\n" +
        $"\nRename directories in \"{CurDir.Name}\" using regex pattern." +
        $"\n" +
        $"\n>> Regex pattern to replace: "
      );

      searchPattern = Console.ReadLine() ?? "";
    }

    if (replaceWith == null)
    {
      Console.Write($"\n>> Replace with: ");

      replaceWith = IOHelper.File.SanitizeFilename(Console.ReadLine() ?? "");
    }

    // Summarize .. 
    Console.WriteLine(
      $"\n" +
      $"\nSummary .." +
      $"\n" +
      $"\nIn:      {CurDir.Name}" +
      $"\nReplace: /{searchPattern}/" +
      $"\nWith:    {replaceWith}" +
      $"\n" +
      $"\n{(recursive ? "recursively" : "in this folder only")}" +
      $"\n" +
      $"\n>> Hit any key to proceed .."
    );
    Console.ReadKey(true);

    CurDir.RenameItems(searchPattern, replaceWith, IOHelper.Directory.IOType.Folder, recursive);
  }
}