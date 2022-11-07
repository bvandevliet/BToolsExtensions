using CommandDotNet;
using IOHelper;
using System.Globalization;

namespace BToolsCLI;

public partial class Commands
{
  [Command(Description = "Creates a new yyyy-MM-dd folder.")]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
  public void NewYMDFolder(
    [Operand(Description = "Path to the parent folder.")]
    string path)
  {
    CultureInfo Culture = new("en-US");

    DateTime DateNow = DateTime.Now;

  setDate:

    Console.Clear();

    Console.Write(
      $"\n" +
      $"\nSet the date using LeftArrow and RightArrow or UpArrow for today, then hit Enter." +
      $"\n" +
      $"\n               {DateNow.ToString("dddd", Culture)}" +
      $"\n>> Foldername: {DateNow:yyyy-MM-dd}"
    );

    ConsoleKeyInfo KeyPressed = Console.ReadKey(true);

    // Modify the date and jump back.
    if (
      KeyPressed.Key != ConsoleKey.Enter &&
      KeyPressed.Key != ConsoleKey.Spacebar)
    {
      try
      {
        switch (KeyPressed.Key)
        {
          case ConsoleKey.UpArrow:
            DateNow = DateTime.Now;
            break;
          case ConsoleKey.DownArrow:
            DateNow = new DateTime(9999, 12, 31);
            break;
          case ConsoleKey.LeftArrow:
            DateNow = DateNow.AddDays(-1);
            break;
          case ConsoleKey.RightArrow:
            DateNow = DateNow.AddDays(+1);
            break;
        }
      }
      catch (Exception) { }

      goto setDate;
    }

    Console.Clear();

    Console.Write(
      $"\n" +
      $"\nType in the folder description, then hit Enter to create the folder." +
      $"\n" +
      $"\n               {DateNow.ToString("dddd", Culture),-10}  Description" +
      $"\n>> Foldername: {DateNow:yyyy-MM-dd}"
    );

    Console.Write("  ");

    // Get the description.
    string description = IOHelper.File.SanitizeFilename(Console.ReadLine() ?? "");

    IOHelper.Path.TryGetMappedDrivePath(ref path);

    DirectoryInfo dir = new(path);

    // If parent folder exists, create the YMD subfolder.
    if (dir.Exists)
    {
      dir.CreateSubdirectory($"{DateNow:yyyy-MM-dd}  {description}");
    }
  }
}