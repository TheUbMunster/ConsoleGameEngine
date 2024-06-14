using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public static class ConsoleUtil
   {
      #region Typedefs
      public enum DWInputMode : uint
      {
         ENABLE_ECHO_INPUT = 0x0004, //enabled by default
         ENABLE_INSERT_MODE = 0x0020, //enabled by default
         ENABLE_LINE_INPUT = 0x0002, //enabled by default
         ENABLE_MOUSE_INPUT = 0x0010, //enabled by default
         ENABLE_PROCESSED_INPUT = 0x0001, //enabled by default
         ENABLE_QUICK_EDIT_MODE = 0x0040, //enabled by default
         ENABLE_WINDOW_INPUT = 0x0008, //disabled by default
         ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200 //enabled by default
      }

      public enum DWBufferMode : uint
      {
         ENABLE_PROCESSED_OUTPUT = 0x0001, //enabled by default
         ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002, //enabled by default
         ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004, //disabled by default
         DISABLE_NEWLINE_AUTO_RETURN = 0x0008, //disabled by default
         ENABLE_LVB_GRID_WORLDWIDE = 0x0010 //disabled by default
      }
      #endregion

      #region Consts
      //-============================-
      //- BORDER CHARACTER CONSTANTS -
      //-============================-
      public const char charHorizontalBorder = '\x2550';
      public const char charVerticalBorder = '\x2551';
      public const char charBottomLeftCornerBorder = '\x255A';
      public const char charTopLeftCornerBorder = '\x2554';
      public const char charBottomRightCornerBorder = '\x255D';
      public const char charTopRightCornerBorder = '\x2557';
      public const char charNotTopTBorder = '\x2566';
      public const char charNotLeftTBorder = '\x2560';
      public const char charNotBottomTBorder = '\x2569';
      public const char charNotRightTBorder = '\x2563';
      public const char charXBorder = '\x256C';
      #endregion

      #region Ctor
      static ConsoleUtil()
      {
         EventHandler onExit = (_, _) => { OnApplicationQuit?.Invoke(); };
         AppDomain.CurrentDomain.ProcessExit += onExit;
         OnApplicationStart?.Invoke();
      }
      #endregion

      #region Fields
      public static PlatformID Platform { get; private set; } = Environment.OSVersion.Platform;

      #region EngineEvents
      public static event Action OnApplicationStart;
      public static event Action OnApplicationQuit;
      #endregion
      #endregion

      #region Initialize
      public static void Initialize()
      {
         CGEUtility.DisableConsoleMode(CGEUtility.DWInputMode.ENABLE_QUICK_EDIT_MODE, out _);
         CGEUtility.OrConsoleMode(CGEUtility.DWBufferMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING, out _);
         Console.CursorVisible = false;
      }
      #endregion

      #region Extern Mac
      /// <summary>
      /// MAC ONLY
      /// </summary>
      [DllImport("libc")]
      private static extern int system(string exec);
      #endregion

      #region Extern Windows
      /// <summary>
      /// WINDOWS ONLY
      /// </summary>
      [DllImport("kernel32.dll", SetLastError = true)]
      private static extern IntPtr GetStdHandle(int nStdHandle);

      /// <summary>
      /// WINDOWS ONLY
      /// </summary>
      [DllImport("kernel32.dll")]
      private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

      /// <summary>
      /// WINDOWS ONLY
      /// </summary>
      [DllImport("kernel32.dll")]
      private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
      #endregion

      #region Windows
      /// <summary>
      /// Sets the console modes. See <see href="https://docs.microsoft.com/en-us/windows/console/setconsolemode">here</see> for more info. (Top region of page).
      /// Default mode is 0x0277.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="modes">Bitwise-ord flags of the modes you want set.</param>
      /// <param name="error">True if an error occured, false otherwise.</param>
      public static void SetConsoleMode(DWInputMode modes, out bool error)
      {
         const int STD_INPUT_HANDLE = -10;
         error = false;
         if (Platform == PlatformID.Win32NT)
         {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode = (uint)modes;
            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
            }
         }
      }

      /// <summary>
      /// Or's the current console mode with the provided modes. See <see href="https://docs.microsoft.com/en-us/windows/console/setconsolemode">here</see> for more info. (Top region of page).
      /// Default mode is 0x0277.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="modes">Bitwise-ord flags of the modes you want to or into the current modes.</param>
      /// <param name="error">True if an error occured, false otherwise.</param>
      public static void OrConsoleMode(DWInputMode modes, out bool error)
      {
         const int STD_INPUT_HANDLE = -10;
         error = false;
         if (Platform == PlatformID.Win32NT)
         {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode;
            if (!GetConsoleMode(consoleHandle, out consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
               goto end; //don't attempt to assign.
            }
            consoleMode |= (uint)modes;
            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
            }
         end:;
         }
      }

      /// <summary>
      /// Nots and ands the provided modes with the current console modes. See <see href="https://docs.microsoft.com/en-us/windows/console/setconsolemode">here</see> for more info. (Top region of page).
      /// Default mode is 0x0277.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="modes">Bitwise-ord flags of the modes you want to not/and into the current modes.</param>
      /// <param name="error">True if an error occured, false otherwise.</param>
      public static void DisableConsoleMode(DWInputMode modes, out bool error)
      {
         const int STD_INPUT_HANDLE = -10;
         error = false;
         if (Platform == PlatformID.Win32NT)
         {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode;
            if (!GetConsoleMode(consoleHandle, out consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
               goto end; //don't attempt to assign.
            }
            consoleMode &= ~((uint)modes);
            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
            }
         end:;
         }
      }

      /// <summary>
      /// Sets the console modes. See <see href="https://docs.microsoft.com/en-us/windows/console/setconsolemode">here</see> for more info. (Bottom region of page).
      /// Default mode is 0x0003.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="modes">Bitwise-ord flags of the modes you want set.</param>
      /// <param name="error">True if an error occured, false otherwise.</param>
      public static void SetConsoleMode(DWBufferMode modes, out bool error)
      {
         const int STD_INPUT_HANDLE = -11;
         error = false;
         if (Platform == PlatformID.Win32NT)
         {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode = (uint)modes;
            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
            }
         }
      }

      /// <summary>
      /// Or's the current console mode with the provided modes. See <see href="https://docs.microsoft.com/en-us/windows/console/setconsolemode">here</see> for more info. (Bottom region of page).
      /// Default mode is 0x0003.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="modes">Bitwise-ord flags of the modes you want to or into the current modes.</param>
      /// <param name="error">True if an error occured, false otherwise.</param>
      public static void OrConsoleMode(DWBufferMode modes, out bool error)
      {
         const int STD_INPUT_HANDLE = -11;
         error = false;
         if (Platform == PlatformID.Win32NT)
         {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode;
            if (!GetConsoleMode(consoleHandle, out consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
               goto end; //don't attempt to assign.
            }
            consoleMode |= (uint)modes;
            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
            }
         end:;
         }
      }

      /// <summary>
      /// Nots and ands the provided modes with the current console modes. See <see href="https://docs.microsoft.com/en-us/windows/console/setconsolemode">here</see> for more info. (Bottom region of page).
      /// Default mode is 0x0003.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="modes">Bitwise-ord flags of the modes you want to not/and into the current modes.</param>
      /// <param name="error">True if an error occured, false otherwise.</param>
      public static void DisableConsoleMode(DWBufferMode modes, out bool error)
      {
         const int STD_INPUT_HANDLE = -11;
         error = false;
         if (Platform == PlatformID.Win32NT)
         {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode;
            if (!GetConsoleMode(consoleHandle, out consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
               goto end; //don't attempt to assign.
            }
            consoleMode &= ~((uint)modes);
            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
               // Error. See GetLastError for more error info.
               error = true;
            }
         end:;
         }
      }
      #endregion

      #region Mac

      #endregion

      #region Linux

      #endregion

      #region Mixed
      /// <summary>
      /// Resizes the console/terminal on the screen.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="width">The new console/terminal width</param>
      /// <param name="height">The new console/terminal height</param>
      public static void ResizeConsole(int width, int height)
      {
         switch (Platform)
         {
            case PlatformID.Win32NT:
               Console.SetWindowSize(width, height);
               Console.SetBufferSize(width, height); //this may or may not need to be removed or put above the line above.
#pragma warning disable CA1416
#pragma warning restore CA1416
               break;
            case PlatformID.Unix:
               system(@"printf '\e[8;" + height + @";" + width + @"t'");
               //system($"printf \'\\e[8;{height};{width}t\\33c\\e[3J\'");
               break;
            case PlatformID.Other:
               break;
         }
      }
      #endregion

      #region Util
      //TODO: fallback on ConsoleColor

      /// <summary>
      /// ENABLE_VIRTUAL_TERMINAL_PROCESSING must be enabled for this to function, compatible with Windows 10 version 1607 or greater.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="r">Red value (0-255)</param>
      /// <param name="g">Green value (0-255)</param>
      /// <param name="b">Blue value (0-255)</param>
      /// <param name="foreground">True if foreground color, false if background.</param>
      /// <returns>ANSI code for coloring content in the console.</returns>
      public static string GetColorANSIPrefix(byte r, byte g, byte b, bool foreground = true)
      {
         return $"\x1b[{(foreground ? 38 : 48)};2;{r};{g};{b}m";
      }

      /// <summary>
      /// ENABLE_VIRTUAL_TERMINAL_PROCESSING must be enabled for this to function, compatible with Windows 10 version 1607 or greater.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="r">Red value (0d-1d)</param>
      /// <param name="g">Green value (0d-1d)</param>
      /// <param name="b">Blue value (0d-1d)</param>
      /// <param name="foreground">True if foreground color, false if background.</param>
      /// <returns>ANSI code for coloring content in the console.</returns>
      public static string GetColorANSIPrefix(double r, double g, double b, bool foreground = true)
      {
         return $"\x1b[{(foreground ? 38 : 48)};2;{(byte)(r * 255)};{(byte)(g * 255)};{(byte)(b * 255)}m";
      }

      /// <summary>
      /// ENABLE_VIRTUAL_TERMINAL_PROCESSING must be enabled for this to function, compatible with Windows 10 version 1607 or greater.<br/><br/>
      /// <b>Author: TheUbMunster</b>
      /// </summary>
      /// <param name="r">Red value (0f-1f)</param>
      /// <param name="g">Green value (0f-1f)</param>
      /// <param name="b">Blue value (0f-1f)</param>
      /// <param name="foreground">True if foreground color, false if background.</param>
      /// <returns>ANSI code for coloring content in the console.</returns>
      public static string GetColorANSIPrefix(float r, float g, float b, bool foreground = true)
      {
         return $"\x1b[{(foreground ? 38 : 48)};2;{(byte)(r * 255)};{(byte)(g * 255)};{(byte)(b * 255)}m";
      }
      #endregion
   }
}
