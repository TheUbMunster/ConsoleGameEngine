using ConsoleGameEngine.Data_Structures;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.old;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
         //MenuMode = 4,
         RawMode = 8, //"raw" control
      }
      #endregion

      #region Fields
      #region Parent Window Stuff
      private int left;
      /// <summary>
      /// How many columns to the right is this window relative to the left edge of the parent window.
      /// </summary>
      public int Left 
      { 
         get => left;
         set
         {
            if (left != value)
            {
               if (ParentWindow != null && (ParentWindow.DrawType & WindowDrawType.WindowMode) != WindowDrawType.Disabled) 
                  ParentWindow.IsDirty |= true;
               left = value;
            }
         }
      }
      private int top;
      /// <summary>
      /// How many rows to the bottom is this window relative to the top edge of the parent window.
      /// </summary>
      public int Top 
      {
         get => top;
         set
         {
            if (top != value)
            {
               if (ParentWindow != null && (ParentWindow.DrawType & WindowDrawType.WindowMode) != WindowDrawType.Disabled)
                  ParentWindow.IsDirty |= true;
               top = value;
            }
         }
      }
      private int zOrder;
      /// <summary>
      /// What order this drawable element is drawn in. High values get drawn on top, low values on bottom.
      /// </summary>
      public int ZOrder 
      {
         get => zOrder;
         set
         {
            if (zOrder != value)
            {
               if (ParentWindow != null && (ParentWindow.DrawType & WindowDrawType.WindowMode) != WindowDrawType.Disabled)
                  ParentWindow.IsDirty |= true;
               zOrder = value;
            }
         }
      }
      #endregion

      #region EntityStuff
      public ObservableCollection<Entity> Entities { get; } = new ObservableCollection<Entity>(); //when add/remove set this window dirty if WindowDrawType is EntityMode
      #endregion

      #region Child Window Stuff
      /// <summary>
      /// All child console windows are implicitly drawn on top of this one. ZOrder is only compared between children to determine draw order.
      /// </summary>
      public ObservableCollection<ConsoleWindow> ChildConsoleWindows { get; } = new ObservableCollection<ConsoleWindow>(); //when add/remove set this window dirty if WindowDrawType is WindowMode
      #endregion

      #region TextMode
      public NDCollection<char> RawChars { get; }
      public NDCollection<int> RawColorCodes { get; }
      public NDCollection<bool> RawDisplayMask { get; }
      //TODO: ObservableDictionary
      public ObservableDictionary<int, string> RawColorCodesLookup { get; } //when modify set this window dirty if WindowDrawType is textmode
      #endregion

      #region General
      private WindowDrawType drawType;
      /// <summary>
      /// Flag field for what elements this window should display.
      /// 
      /// If no flags are set, this window will be entirely ignored during rendering (this window will be fully transparent).
      /// 
      /// Note: TextMode content will always be drawn on top.
      /// </summary>
      public WindowDrawType DrawType
      {
         get => drawType;
         set
         {
            if (drawType != value)
            {
               IsDirty |= true;
               drawType = value;
            }
         }
      }
      private bool isDirty = true;
      public bool IsDirty 
      {
         get => isDirty;
         set
         {
            if (value)
            {
               if (ParentWindow != null)
                  ParentWindow.IsDirty = true;
            }
            isDirty = value;
         }
      }
      public int Width { get; }
      public int Height { get; }
      public ConsoleWindow ParentWindow { get; private set; } = null;
      private FrameInfo lastFrameInfo = null;
      #endregion
      #endregion

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
         RawChars = new NDCollection<char>(Enumerable.Repeat(' ', width * height), width, height);
         RawColorCodes = new NDCollection<int>(width, height);
         RawDisplayMask = new NDCollection<bool>(width, height);
         RawColorCodesLookup = new ObservableDictionary<int, string>()
         { { 0, ConsoleUtil.GetColorANSIPrefix(255, 255, 255) } };
         void OnNDCollectionModify<T>(int[] indeces, T oldv, T newv)
         {
            if (!oldv.Equals(newv) && (DrawType & WindowDrawType.RawMode) != WindowDrawType.Disabled)
               IsDirty = true;
         }
         RawChars.OnContentsChanged += OnNDCollectionModify<char>;
         RawColorCodes.OnContentsChanged += OnNDCollectionModify<int>;
         RawDisplayMask.OnContentsChanged += OnNDCollectionModify<bool>;
         RawColorCodesLookup.CollectionChanged += (obj, args) =>
         {
            switch (args.Action)
            {
               case NotifyCollectionChangedAction.Add:
               case NotifyCollectionChangedAction.Remove:
               case NotifyCollectionChangedAction.Reset:
                  if ((DrawType & WindowDrawType.RawMode) != WindowDrawType.Disabled)
                     IsDirty = true;
                  break;
               case NotifyCollectionChangedAction.Replace:
                  if (!args.OldItems[0].Equals(args.NewItems[0]) && (DrawType & WindowDrawType.RawMode) != WindowDrawType.Disabled)
                     IsDirty = true;
                  break;
               case NotifyCollectionChangedAction.Move: //do nothing?
                  break;
               default:
                  throw new NotImplementedException("Unrecognized ObservableDictionary event");
            }
         };
         Entities.CollectionChanged += (obj, args) =>
         {
            switch (args.Action)
            {
               case NotifyCollectionChangedAction.Add:
                  foreach (Entity item in args.NewItems)
                  {
                     if (item.ParentWindow != null)
                        throw new Exception("Error: ConsoleWindow was assigned to be a child of another ConsoleWindow, but it already was a child of a ConsoleWindow. Remove it as a child from the other ConsoleWindow before adding it to this one!");
                     item.ParentWindow = this;
                  }
                  break;
               case NotifyCollectionChangedAction.Reset: // I think this is just .Clear()
               case NotifyCollectionChangedAction.Remove:
                  foreach (Entity item in args.NewItems)
                  {
                     if (item.ParentWindow == null)
                        throw new Exception("Error: a ConsoleWindow was removed as a child of this ConsoleWindow, but for some reason it had an internal null parent!");
                     else if (item.ParentWindow != this)
                        throw new Exception("Error: a ConsoleWindow was removed as a child of this ConsoleWindow, but for some reason it had a different internal parent!");
                     item.ParentWindow = null;
                  }
                  break;
               case NotifyCollectionChangedAction.Replace: //when: coll[5] = newObj;
                  if (!(args.OldItems.Count == 1 && args.NewItems.Count == 1))
                     throw new NotImplementedException("Error: What function did you even call to cause this exception?");
                  Entity oldw = (args.OldItems[0] as Entity), neww = (args.NewItems[0] as Entity);
                  if (oldw.ParentWindow == null)
                     throw new Exception("Error: a ConsoleWindow was removed as a child of this ConsoleWindow, but for some reason it had an internal null parent!");
                  else if (oldw.ParentWindow != this)
                     throw new Exception("Error: a ConsoleWindow was removed as a child of this ConsoleWindow, but for some reason it had a different internal parent!");
                  oldw.ParentWindow = null;
                  if (neww.ParentWindow != null)
                     throw new Exception("Error: ConsoleWindow was assigned to be a child of another ConsoleWindow, but it already was a child of a ConsoleWindow. Remove it as a child from the other ConsoleWindow before adding it to this one!");
                  neww.ParentWindow = this;
                  break;
               case NotifyCollectionChangedAction.Move:
                  break; //shouldn't need to do anything hypothetically
               default:
                  throw new NotImplementedException("Unrecognized ObservableCollection event");
            }
         };
         ChildConsoleWindows.CollectionChanged += (obj, args) =>
         {
            switch (args.Action)
            {
               case NotifyCollectionChangedAction.Add:
                  foreach (ConsoleWindow item in args.NewItems)
                  {
                     if (item.ParentWindow != null)
                        throw new Exception("Error: ConsoleWindow was assigned to be a child of another ConsoleWindow, but it already was a child of a ConsoleWindow. Remove it as a child from the other ConsoleWindow before adding it to this one!");
                     item.ParentWindow = this;
                  }
                  break;
               case NotifyCollectionChangedAction.Reset: // I think this is just .Clear()
               case NotifyCollectionChangedAction.Remove:
                  foreach (ConsoleWindow item in args.NewItems)
                  {
                     if (item.ParentWindow == null)
                        throw new Exception("Error: a ConsoleWindow was removed as a child of this ConsoleWindow, but for some reason it had an internal null parent!");
                     else if (item.ParentWindow != this)
                        throw new Exception("Error: a ConsoleWindow was removed as a child of this ConsoleWindow, but for some reason it had a different internal parent!");
                     item.ParentWindow = null;
                  }
                  break;
               case NotifyCollectionChangedAction.Replace: //when: coll[5] = newObj;
                  if (!(args.OldItems.Count == 1 && args.NewItems.Count == 1))
                     throw new NotImplementedException("Error: What function did you even call to cause this exception?");
                  ConsoleWindow oldw = (args.OldItems[0] as ConsoleWindow), neww = (args.NewItems[0] as ConsoleWindow);
                  if (oldw.ParentWindow == null)
                     throw new Exception("Error: a ConsoleWindow was removed as a child of this ConsoleWindow, but for some reason it had an internal null parent!");
                  else if (oldw.ParentWindow != this)
                     throw new Exception("Error: a ConsoleWindow was removed as a child of this ConsoleWindow, but for some reason it had a different internal parent!");
                  oldw.ParentWindow = null;
                  if (neww.ParentWindow != null)
                     throw new Exception("Error: ConsoleWindow was assigned to be a child of another ConsoleWindow, but it already was a child of a ConsoleWindow. Remove it as a child from the other ConsoleWindow before adding it to this one!");
                  neww.ParentWindow = this;
                  break;
               case NotifyCollectionChangedAction.Move:
                  break; //shouldn't need to do anything hypothetically
               default:
                  throw new NotImplementedException("Unrecognized ObservableCollection event");
            }
         };
      }

      public FrameInfo Draw()
      {
         if (!IsDirty && lastFrameInfo != null)
            return lastFrameInfo;
         int cullCount = 0;
         char[,] chars = new char[Height, Width];
         for (int i = 0; i < chars.GetLength(0); i++)
            for (int j = 0; j < chars.GetLength(1); j++)
               chars[i, j] = ' '; //replace nulls with space
         int[,] colorCodes = new int[Height, Width];
         Dictionary<int, string> colorCodesLookup = new Dictionary<int, string>()
         { { 0, ConsoleUtil.GetColorANSIPrefix(255, 255, 255) } };
         //entity mode
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
         //raw text mode
         if ((DrawType & WindowDrawType.RawMode) != WindowDrawType.Disabled)
         {
            int newKey = colorCodesLookup.Count;
            //key: old color code, value: new color code
            Dictionary<int, int> colorCodesRemappings = new();
            foreach (KeyValuePair<int, string> lookup in RawColorCodesLookup)
            {
               colorCodesRemappings.Add(lookup.Key, newKey);
               colorCodesLookup.Add(newKey, lookup.Value);
               newKey++;
            }
            for (int x = 0; x < Width; x++)
            {
               for (int y = 0; y < Height; y++)
               {
                  if (RawDisplayMask[x, y])
                  {
                     chars[y, x] = RawChars[x, y];
                     colorCodes[y, x] = colorCodesRemappings[RawColorCodes[x, y]];
                  }
               }
            }
         }
         IsDirty = false;
         lastFrameInfo = new FrameInfo()
         {
            Chars = new NDCollection<char>(chars.Cast<char>(), Width, Height),
            ColorCodes = new NDCollection<int>(colorCodes.Cast<int>(), Width, Height),
            ColorCodesLookup = colorCodesLookup,
            Meta = $"cull: {cullCount} d: {false}" //*need* to fix this not updating or else the meta feature won't work
         };
         return new FrameInfo()
         {
            Chars = new NDCollection<char>(chars.Cast<char>(), Width, Height),
            ColorCodes = new NDCollection<int>(colorCodes.Cast<int>(), Width, Height),
            ColorCodesLookup = colorCodesLookup,
            Meta = $"cull: {cullCount} d: {true}"
         };
      }
   }
}
