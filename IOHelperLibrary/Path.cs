using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace IOHelper;

public static class Path
{
  /// <summary>
  /// Attempts to get the mapped drive path.
  /// </summary>
  /// <param name="path">A network path.</param>
  /// <returns>Whether a mapped drive path was determined.</returns>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
  public static bool TryGetMappedDrivePath(ref string path)
  {
    // If the path already includes a mapped drive letter, do nothing.
    if (Regex.IsMatch(path, @"^[a-zA-Z]:"))
    {
      return true;
    }

    try
    {
      // Get the Network key from registry.
      using RegistryKey? drivesKey = Registry.CurrentUser.OpenSubKey("Network", false);

      if (drivesKey != null)
      {
        // Loop through the assigned drive letters.
        foreach (string driveLetter in drivesKey.GetSubKeyNames())
        {
          try
          {
            using RegistryKey? driveKey = drivesKey.OpenSubKey(driveLetter, false);

            // If a drive letter is assigned to the given network folder, use it.
            if (driveKey?.GetValue("RemotePath") is string remotePath && path.StartsWith(remotePath))
            {
              path = $@"{driveLetter.ToUpper()}:{path[remotePath.Length..]}";

              return true;
            }
          }
          catch (Exception) { } // dirty next ..
        }
      }
    }
    catch (Exception) { } // dirty catch ..

    return false;
  }
}