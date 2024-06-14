using ConsoleGameEngine.old;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   /// <summary>
   /// Represents an instance of a console window.
   /// 
   /// Intended to be "drawn" to the screen
   /// Can contain sprites, text (ui) or other windows
   /// </summary>
   public class ConsoleWindow
   {
      public enum WindowDrawType
      {
         Disabled = 0,
         TextMode = 1,
         SpriteMode = 2,
         WindowMode = 4
      }
      /// <summary>
      /// Flag field for what elements this window should display.
      /// 
      /// If no flags are set, this window will be entirely ignored during rendering (this window will be fully transparent).
      /// 
      /// Note: TextMode content will always be drawn on top.
      /// </summary>
      public WindowDrawType DrawType { get; set; } 
      /// <summary>
      /// How many columns to the right is this window relative to the left edge of the parent window.
      /// </summary>
      public int Left { get; set; }
      /// <summary>
      /// How many rows to the bottom is this window relative to the top edge of the parent window.
      /// </summary>
      public int Top { get; set; }
      /// <summary>
      /// What order this drawable element is drawn in. High values get drawn on top, low values on bottom.
      /// </summary>
      public int ZOrder { get; set; }
      public List<Entity> Sprites { get; } = new List<Entity>();
      public List<ConsoleWindow> ChildConsoleWindows { get; } = new List<ConsoleWindow>();
      public IReadOnlyList<char[,]> Chars { get; init; }
      public IReadOnlyList<int[,]> ColorCodes { get; init; }
      public IReadOnlyDictionary<int, string> ColorCodesLookup { get; init; }
      /// <summary>
      /// Creates a ConsoleWindow with the specified width and height.
      /// 
      /// If you're creating a ConsoleWindow with the intent to place it fully inside another window (like a popup),
      /// it's recommended that the new ConsoleWindow has a width and height at least four less than
      /// the containing window.
      /// </summary>
      public ConsoleWindow(int width, int height)
      {
         
      }

      public void Draw(int left, int top)
      {
         Console.SetCursorPosition(left, top);

      }
   }
}
