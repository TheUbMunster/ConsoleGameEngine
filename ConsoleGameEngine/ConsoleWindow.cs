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
         TextMode = 1,
         EntityMode = 2,
         WindowMode = 4
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
         bool anyOutOfBounds = false;
         char[,] chars = new char[Height, Width];
         for (int i = 0; i < chars.GetLength(0); i++)
            for (int j = 0; j < chars.GetLength(1); j++)
               chars[i, j] = ' ';
         int[,] colorCodes = new int[Height, Width];
         HashSet<int> takenColorCodesLookupKeys = new HashSet<int>() { 0 };
         Dictionary<int, string> colorCodesLookup = new Dictionary<int, string>()
         { { 0, ConsoleUtil.GetColorANSIPrefix(255, 255, 255) } };
         //our sprites
         if ((DrawType & WindowDrawType.EntityMode) != WindowDrawType.Disabled)
         {
            //key: PersistenSpriteId, value: (key: old color code, value: new color code)
            Dictionary<int, Dictionary<int, int>> spritesColorCodesRemappings = new();
            foreach (Entity e in Entities.OrderBy(x => x.ZOrder)) //small to big, so that big gets rendered last
            {
               bool fullyOutOfBounds = e.Left >= Width || e.Left + e.BackingSprite.Width <= 0 ||
                  e.Top >= Height || e.Top + e.BackingSprite.Height <= 0;
               anyOutOfBounds |= fullyOutOfBounds;
               if (!fullyOutOfBounds)
               {
                  if (e.BackingSprite.PersistentSpriteId.HasValue)
                  {
                     if (!spritesColorCodesRemappings.ContainsKey(e.BackingSprite.PersistentSpriteId.Value))
                        spritesColorCodesRemappings.Add(e.BackingSprite.PersistentSpriteId.Value, new());
                     for (int oldKeyIndex = 0; oldKeyIndex < e.BackingSprite.ColorCodesLookup.Count; oldKeyIndex++)
                     {
                        int newKey = 0;
                        while (takenColorCodesLookupKeys.Contains(newKey))
                           newKey++;
                        takenColorCodesLookupKeys.Add(newKey);
                        spritesColorCodesRemappings[e.BackingSprite.PersistentSpriteId.Value].Add(e.BackingSprite.ColorCodesLookup.ElementAt(oldKeyIndex).Key, newKey);
                     }
                  }
                  else
                  {
                     throw new NotImplementedException();
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
                              colorCodes[worldy, worldx] = spritesColorCodesRemappings[e.BackingSprite.PersistentSpriteId.Value][e.BackingSprite.ColorCodes[e.AnimationFrameIndex][x, y]];
                           }
                        }
                     }
                  }
               }
            }
            for (int spriteRemaps = 0; spriteRemaps < spritesColorCodesRemappings.Count; spriteRemaps++)
            {
               var e = spritesColorCodesRemappings.ElementAt(spriteRemaps);
               foreach (KeyValuePair<int, int> kvp in e.Value)
               {
                  colorCodesLookup.Add(kvp.Value, Sprite.PersistentSpriteTemplates[e.Key].ColorCodesLookup[kvp.Key]);
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
            //Meta = $"anyOutOfBounds: {anyOutOfBounds}"
         };
      }
   }
}
