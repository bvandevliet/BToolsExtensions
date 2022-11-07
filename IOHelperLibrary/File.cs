using System.Text.RegularExpressions;

namespace IOHelper;

public static class File
{
  /// <summary>
  /// Regex of file extensions that are considered to belong to temporary files.
  /// </summary>
  public static readonly Regex tempExt = new(@"\.(aso|bac|bak|cache|cfa|clp|csd|csi|cvr|dmp|download|dwl|etl|lo?ck|pa?rt|partial|pnf|rra|te?mp|thumb*|tmt)\$?\d*$");

  /// <summary>
  /// Strips forbidden characters from a given filename.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public static string SanitizeFilename(string name)
  {
    return Regex.Replace(name, "[\\/:*?\"<>]", "");
  }
}