using System;
using System.Runtime.InteropServices;
using static System.Console;

public static class Program
{
    public static void Main(string[] args) 
    {
          var defaultMessage = "Hello from .NET!";
          var message = args is object && args.Length > 0 ? string.Join(' ', args): defaultMessage;

          WriteLine();
          WriteLine($"      {message}{GetBot()}");
          
          WriteLine("Environment:");
          WriteLine(RuntimeInformation.FrameworkDescription);
          WriteLine(RuntimeInformation.OSDescription);
    }

    private static string GetBot() 
    {
            
            return @"
      __________________
                        \
                        \
                            ....
                            ....'
                            ....
                          ..........
                      .............'..'..
                  ................'..'.....
                .......'..........'..'..'....
                ........'..........'..'..'.....
              .'....'..'..........'..'.......'.
              .'..................'...   ......
              .  ......'.........         .....
              .                           ......
              ..    .            ..        ......
            ....       .                 .......
            ......  .......          ............
              ................  ......................
              ........................'................
            ......................'..'......    .......
          .........................'..'.....       .......
      ........    ..'.............'..'....      ..........
    ..'..'...      ...............'.......      ..........
    ...'......     ...... ..........  ......         .......
  ...........   .......              ........        ......
  .......        '...'.'.              '.'.'.'         ....
  .......       .....'..               ..'.....
    ..       ..........               ..'........
            ............               ..............
          .............               '..............
          ...........'..              .'.'............
        ...............              .'.'.............
        .............'..               ..'..'...........
        ...............                 .'..............
        .........                        ..............
          .....
  ";
    }
}
