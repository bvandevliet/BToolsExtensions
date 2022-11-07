using CommandDotNet;

namespace BToolsCLI;

public partial class Commands
{
  [Command(Description = "Deletes all temporary files from a folder.")]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
  public void CleanUp(
    [Operand(Description = "Path to the folder.")]
    string path,
    [Option('r', Description = "Also cleanup all its subdirectories?", BooleanMode = BooleanMode.Implicit)]
    bool recursive)
  {
    IOHelper.Path.TryGetMappedDrivePath(ref path);

    IOHelper.Directory.DelTemp(path, recursive);

    Console.WriteLine("\nDone"); Console.ReadKey(true);
  }
}