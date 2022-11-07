using System.Text.RegularExpressions;
using IO = System.IO;

namespace IOHelper;

public static class Directory
{
  /// <summary>
  /// Type of IO item.
  /// </summary>
  public enum IOType
  {
    File,
    Folder,
    Both,
  }

  /// <summary>
  /// Rename contents of a folder.
  /// </summary>
  /// <param name="source"></param>
  /// <param name="searchPattern"></param>
  /// <param name="replaceWith"></param>
  /// <param name="replaceType"></param>
  /// <param name="recursive"></param>
  public static void RenameItems(this DirectoryInfo source, string searchPattern, string replaceWith, IOType replaceType = IOType.File, bool recursive = false)
  {
    if (!source.Exists)
    {
      Console.WriteLine($"Folder \"{source.FullName}\" does not exist.");

      return;
    }

    if (recursive || replaceType == IOType.Folder || replaceType == IOType.Both)
    {
      foreach (DirectoryInfo subdirInfo in source.GetDirectories("*", SearchOption.TopDirectoryOnly))
      {
        if (recursive)
        {
          RenameItems(subdirInfo, searchPattern, replaceWith, replaceType, recursive);
        }

        if (
          (replaceType == IOType.Folder || replaceType == IOType.Both) &&
          Regex.IsMatch(subdirInfo.Name, searchPattern))
        {
          // Try-catch to avoid an abort, e.g. in case the replacement already matches the output.
          try
          {
            string newFolderName = Regex.Replace(subdirInfo.Name, searchPattern, replaceWith);

            subdirInfo.MoveTo(IO.Path.Combine(source.FullName, newFolderName));
          }
          catch (Exception) { }
        }
      }
    }

    if (replaceType == IOType.File || replaceType == IOType.Both)
    {
      foreach (FileInfo fileInfo in source.GetFiles("*", SearchOption.TopDirectoryOnly))
      {
        if (Regex.IsMatch(fileInfo.Name, searchPattern))
        {
          // Try-catch to avoid an abort, e.g. in case the replacement already matches the output.
          try
          {
            string newFileName = Regex.Replace(fileInfo.Name, searchPattern, replaceWith);

            fileInfo.MoveTo(IO.Path.Combine(source.FullName, newFileName));
          }
          catch (Exception) { }
        }
      }
    }
  }

  /// <summary>
  /// Rename contents of a folder.
  /// </summary>
  /// <param name="source"></param>
  /// <param name="searchPattern"></param>
  /// <param name="replaceWith"></param>
  /// <param name="replaceType"></param>
  /// <param name="recursive"></param>
  public static void RenameItems(string source, string searchPattern, string replaceWith, IOType replaceType = IOType.File, bool recursive = false)
  {
    RenameItems(new DirectoryInfo(source), searchPattern, replaceWith, replaceType, recursive);
  }

  /// <summary>
  /// Copy contents of a Directory into another Directory.
  /// </summary>
  /// <param name="source"></param>
  /// <param name="destination"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool CopyTo(this DirectoryInfo source, DirectoryInfo destination, bool recursive = true)
  {
    // If source does not exist, bail early.
    if (!source.Exists)
    {
      Console.WriteLine($"Folder \"{source.FullName}\" does not exist.");

      return false;
    }

    // If source and destination are equal, do nothing.
    if (source.Equals(destination))
    {
      return true;
    }

    // Create destination directory if it doesn't exist.
    if (!destination.Exists)
    {
      destination.Create(); // allow to throw exception
    }

    bool success = true;

    // Copy file contents.
    foreach (FileInfo fileInfo in source.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
    {
      try
      {
        FileInfo newFile = fileInfo.CopyTo(IO.Path.Combine(new[] { destination.FullName, fileInfo.Name }));

        Console.WriteLine($"Copied \"{newFile.FullName}\"");
      }
      catch (Exception Ex)
      {
        Console.WriteLine(Ex.Message);

        success = false;
      }
    }

    // Copy subdirectories recursively.
    if (recursive)
    {
      foreach (DirectoryInfo dirInfo in source.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
      {
        success &= dirInfo.CopyTo(new DirectoryInfo(IO.Path.Combine(new[] { source.FullName, dirInfo.Name })), true);
      }
    }

    return success;
  }

  /// <summary>
  /// Copy contents of a Directory into another Directory.
  /// </summary>
  /// <param name="source"></param>
  /// <param name="destination"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool CopyTo(this DirectoryInfo source, string destination, bool recursive = true)
  {
    return CopyTo(source, new DirectoryInfo(destination), recursive);
  }

  /// <summary>
  /// Copy contents of a Directory into another Directory.
  /// </summary>
  /// <param name="source"></param>
  /// <param name="destination"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool CopyTo(string source, DirectoryInfo destination, bool recursive = true)
  {
    return CopyTo(new DirectoryInfo(source), destination, recursive);
  }

  /// <summary>
  /// Copy contents of a Directory into another Directory.
  /// </summary>
  /// <param name="source"></param>
  /// <param name="destination"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool CopyTo(string source, string destination, bool recursive = true)
  {
    return CopyTo(new DirectoryInfo(source), new DirectoryInfo(destination), recursive);
  }

  /// <summary>
  /// Delete files that match a given Regex.
  /// </summary>
  /// <param name="path"></param>
  /// <param name="searchPattern"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool CleanUp(this DirectoryInfo path, Regex searchPattern, bool recursive = true)
  {
    // If source does not exist, bail early.
    if (!path.Exists)
    {
      Console.WriteLine($"Folder \"{path.FullName}\" does not exist.");

      return false;
    }

    bool success = true;

    // Delete files that match the search pattern.
    foreach (FileInfo fileInfo in path.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
    {
      if (searchPattern.IsMatch(fileInfo.Name))
      {
        try
        {
          fileInfo.Delete();

          Console.WriteLine($"Deleted \"{fileInfo.FullName}\"");
        }
        catch (Exception Ex)
        {
          Console.WriteLine(Ex.Message);

          success = false;
        }
      }
    }

    // Delete files in subdirectories recursively.
    if (recursive)
    {
      foreach (DirectoryInfo dirInfo in path.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
      {
        success &= dirInfo.CleanUp(searchPattern, true);
      }
    }

    return success;
  }

  /// <summary>
  /// Delete files that match a given Regex pattern.
  /// </summary>
  /// <param name="path"></param>
  /// <param name="searchPattern"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool CleanUp(this DirectoryInfo path, string searchPattern, bool recursive = true)
  {
    return CleanUp(path, new Regex(searchPattern), recursive);
  }

  /// <summary>
  /// Delete files that match a given Regex.
  /// </summary>
  /// <param name="path"></param>
  /// <param name="searchPattern"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool CleanUp(string path, Regex searchPattern, bool recursive = true)
  {
    return CleanUp(new DirectoryInfo(path), searchPattern, recursive);
  }

  /// <summary>
  /// Delete files that match a given Regex pattern.
  /// </summary>
  /// <param name="path"></param>
  /// <param name="searchPattern"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool CleanUp(string path, string searchPattern, bool recursive = true)
  {
    return CleanUp(new DirectoryInfo(path), new Regex(searchPattern), recursive);
  }

  /// <summary>
  /// Deletes all temporary files from a folder.
  /// </summary>
  /// <param name="path"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool DelTemp(this DirectoryInfo path, bool recursive = true)
  {
    return CleanUp(path, File.tempExt, recursive);
  }

  /// <summary>
  /// Deletes all temporary files from a folder.
  /// </summary>
  /// <param name="path"></param>
  /// <param name="recursive"></param>
  /// <returns></returns>
  public static bool DelTemp(string path, bool recursive = true)
  {
    return DelTemp(new DirectoryInfo(path), recursive);
  }
}