using CommandDotNet;

namespace BToolsCLI;

class Program
{
  static int Main(string[] args)
  {
    Console.Title = nameof(BToolsCLI);

    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Yellow;

    // AppRunner<T> where T is the class defining your commands.
    return new AppRunner<Commands>().Run(args);
  }
}