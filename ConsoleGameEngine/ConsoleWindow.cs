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
      #region Typedefs
      public enum WindowDrawType
      {
         Disabled = 0,
         EntityMode = 1,
         WindowMode = 2,
         MenuMode = 4,
         TextMode = 8, //"raw" control
      }
      #endregion

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
         int cullCount = 0;
         char[,] chars = new char[Height, Width];
         for (int i = 0; i < chars.GetLength(0); i++)
            for (int j = 0; j < chars.GetLength(1); j++)
               chars[i, j] = ' ';
         int[,] colorCodes = new int[Height, Width];
         //HashSet<int> takenColorCodesLookupKeys = new HashSet<int>() { 0 };
         Dictionary<int, string> colorCodesLookup = new Dictionary<int, string>()
         { { 0, ConsoleUtil.GetColorANSIPrefix(255, 255, 255) } };
         //our sprites
         if ((DrawType & WindowDrawType.EntityMode) != WindowDrawType.Disabled)
         {
            foreach (Entity e in Entities.OrderBy(x => x.ZOrder)) //small to big, so that big gets rendered last
            {
               bool fullyOutOfBounds = e.Left >= Width || e.Left + e.BackingSprite.Width <= 0 ||
                  e.Top >= Height || e.Top + e.BackingSprite.Height <= 0;
               cullCount += fullyOutOfBounds ? 1 : 0;
               if (!fullyOutOfBounds)
               {
                  int newKey = colorCodesLookup.Count;
                  //key: old color code, value: new color code
                  Dictionary<int, int> colorCodesRemappings = new();
                  foreach (KeyValuePair<int, string> lookup in e.BackingSprite.ColorCodesLookup)
                  {
                     colorCodesRemappings.Add(lookup.Key, newKey);
                     colorCodesLookup.Add(newKey, lookup.Value);
                     newKey++;
                  }
                  for (int x = 0; x < e.BackingSprite.Width; x++)
                  {
                     for (int y = 0; y < e.BackingSprite.Height; y++)
                     {
                        int worldx = x + e.Left, worldy = y + e.Top;
                        if (worldx >= 0 && worldy >= 0 && worldx < Width && worldy < Height)
                        {
                           if (e.BackingSprite.DisplayMask[e.AnimationFrameIndex][x, y])
                           {
                              chars[worldy, worldx] = e.BackingSprite.Chars[e.AnimationFrameIndex][x, y];
                              colorCodes[worldy, worldx] = colorCodesRemappings[e.BackingSprite.ColorCodes[e.AnimationFrameIndex][x, y]];
                           }
                        }
                     }
                  }
               }
            }
         }
         //our child console windows
         if ((DrawType & WindowDrawType.WindowMode) != WindowDrawType.Disabled)
         {
            foreach (ConsoleWindow ccw in ChildConsoleWindows.OrderBy(x => x.ZOrder))
            {
               bool fullyOutOfBounds = ccw.Left >= Width || ccw.Left + ccw.Width <= 0 ||
                  ccw.Top >= Height || ccw.Top + ccw.Height <= 0;
               cullCount += fullyOutOfBounds ? 1 : 0;
               if (!fullyOutOfBounds)
               {
                  FrameInfo ccfi = ccw.Draw();
                  int newKey = colorCodesLookup.Count;
                  //key: old color code, value: new color code
                  Dictionary<int, int> colorCodesRemappings = new();
                  foreach (KeyValuePair<int, string> lookup in ccfi.ColorCodesLookup)
                  {
                     colorCodesRemappings.Add(lookup.Key, newKey);
                     colorCodesLookup.Add(newKey, lookup.Value);
                     newKey++;
                  }
                  for (int x = -1, bx = ccw.Width + 1; x < bx; x++)
                  {
                     for (int y = -1, by = ccw.Height + 1; y < by; y++)
                     {
                        int worldx = x + ccw.Left, worldy = y + ccw.Top;
                        if (worldx >= 0 && worldx < Width && worldy >= 0 && worldy < Height)
                        {
                           colorCodes[worldy, worldx] = 0; //white by default, (should apply to borders).
                           if (x >= 0 && x < ccfi.Width && y >= 0 && y < ccfi.Height)
                           {
                              chars[worldy, worldx] = ccfi.Chars[x, y];
                              colorCodes[worldy, worldx] = colorCodesRemappings[ccfi.ColorCodes[x, y]];
                           }
                           if (x == -1 && y == -1) //top left corner
                              chars[worldy, worldx] = ConsoleUtil.charTopLeftCornerBorder;
                           else if (x == -1 && y == by - 1) //bottom left corner
                              chars[worldy, worldx] = ConsoleUtil.charBottomLeftCornerBorder;
                           else if (x == bx - 1 && y == -1) //top right corner
                              chars[worldy, worldx] = ConsoleUtil.charTopRightCornerBorder;
                           else if (x == bx - 1 && y == by - 1) //bottom right corner
                              chars[worldy, worldx] = ConsoleUtil.charBottomRightCornerBorder;
                           else if ((y == -1 || y == by - 1) && (x > -1 && x < bx - 1)) //horiz edges
                           {
                              if (x < ccfi.Meta.Length)
                                 chars[worldy, worldx] = ccfi.Meta[x]; //- 1 because the very first horiz edge starts at x=1
                              else
                                 chars[worldy, worldx] = ConsoleUtil.charHorizontalBorder;
                           }
                           else if ((x == -1 || x == bx - 1) && (y > -1 && y < by - 1)) //vert edges
                              chars[worldy, worldx] = ConsoleUtil.charVerticalBorder;
                        }
                     }
                  }
               }
            }
         }
         //menu entries mode
         if ((DrawType & WindowDrawType.MenuMode) != WindowDrawType.Disabled)
         {

         }
         //our text
         if ((DrawType & WindowDrawType.TextMode) != WindowDrawType.Disabled)
         {

         }
         return new FrameInfo() 
         {
            Chars = new NDCollection<char>(chars.Cast<char>(), Width, Height),
            ColorCodes = new NDCollection<int>(colorCodes.Cast<int>(), Width, Height),
            ColorCodesLookup = colorCodesLookup,
            Meta = $"cull: {cullCount}"
         };
      }
   }
}
