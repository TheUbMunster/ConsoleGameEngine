using System;
using System.Runtime.InteropServices;
using static ConsoleGameEngine.CGE;

namespace ConsoleGameEngine
{
   public static partial class CGEUtility
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
      /// <b>Author: Samuel Gardner</b>
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
      /// <b>Author: Samuel Gardner</b>
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
      /// <b>Author: Samuel Gardner</b>
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
      /// <b>Author: Samuel Gardner</b>
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
      /// <b>Author: Samuel Gardner</b>
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
      /// <b>Author: Samuel Gardner</b>
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
      /// <b>Author: Samuel Gardner</b>
      /// </summary>
      /// <param name="width">The new console/terminal width</param>
      /// <param name="height">The new console/terminal height</param>
      private static void ResizeConsole(int width, int height)
      {
         switch (Platform)
         {
            case PlatformID.Win32NT:
#pragma warning disable CA1416
               Console.SetWindowSize(width, height);
               Console.SetBufferSize(width, height); //this may or may not need to be removed or put above the line above.
#pragma warning restore CA1416
               break;
            case PlatformID.Unix:
               system(@"printf '\e[8;" + width + @";" + height + @"t'");
               //system($"printf \'\\e[8;{height};{width}t\\33c\\e[3J\'");
               break;
            case PlatformID.Other:
               break;
         }
      }
      #endregion
   }
}