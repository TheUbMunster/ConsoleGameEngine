using System;

namespace ConsoleGameEngine
{
   public static partial class CGEUtility
   {
      #region Utility Methods
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