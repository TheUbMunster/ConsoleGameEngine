using ConsoleGameEngine.DataStructures;
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
         EntityMode = 2,
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
      public int Width { get; }
      public int Height { get; }
      public List<Entity> Entities { get; } = new List<Entity>();
      /// <summary>
      /// All child console windows are implicitly drawn on top of this one. ZOrder is only compared between children to determine draw order.
      /// </summary>
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
         Width = width;
         Height = height;
      }

      public FrameInfo Draw()
      {
         char[,] chars = new char[Width, Height];
         int[,] colorCodes = new int[Width, Height];
         Dictionary<int, string> colorCodesLookup = new Dictionary<int, string>()
         { { 0, ConsoleUtil.GetColorANSIPrefix(255, 255, 255) } };
         //our sprites
         if ((DrawType & WindowDrawType.EntityMode) != WindowDrawType.Disabled)
         {
            foreach (Entity e in Entities.OrderBy(x => x.ZOrder)) //small to big, so that big gets rendered last
            {
               for (int x = 0; x < e.BackingSprite.Width; x++)
               {
                  for (int y = 0; y < e.BackingSprite.Height; y++)
                  {
                     chars[x + e.Left, y + e.Top] = e.BackingSprite.Chars[e.AnimationFrameIndex][x, y];
                  }
               }
            }
         }
         //our child console windows
         if ((DrawType & WindowDrawType.WindowMode) != WindowDrawType.Disabled)
         {

         }
         //our text
         if ((DrawType & WindowDrawType.TextMode) != WindowDrawType.Disabled)
         {

         }
         return new FrameInfo() 
         {
            Chars = new NDLockableCollection<char>(chars.Cast<char>(), Width, Height),
            ColorCodes = new NDLockableCollection<int>(colorCodes.Cast<int>(), Width, Height),
            ColorCodesLookup = colorCodesLookup,
         };
      }
   }
}
