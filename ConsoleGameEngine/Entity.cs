using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public class Entity
   {
      #region Typedefs
      public enum EntityType
      {
         Player = 0,
         PineTree,
         Brush,
         LargePineTree,
         SmallPineTree,
         PineSapling,
      }

      public class EntityZComparer : IComparer<Entity>
      {
         public int Compare(Entity x, Entity y)
         {
            return y.Z - x.Z;
         }
      }
      #endregion

      public int Width { get => Sprites.sprites[(int)myType][0, 0].Length; }
      public int Height { get => Sprites.sprites[(int)myType].GetLength(1); }
      private int left;
      public int Left
      {
         get => left;
         set
         {
            if (value != left)
            {
               //erase trail, notify others that get dirty
               left = value;
               Dirty = true;
            }
         }
      }
      private int top;
      public int Top
      {
         get => top;
         set
         {
            if (value != top)
            {
               //erase trail, notify others that get dirty
               top = value;
               Dirty = true;
            }
         }
      }
      /// <summary>
      /// The lower the z value, the earlier it is drawn.
      /// </summary>
      public int Z { get; set; }
      /// <summary>
      /// A list of all of the characters that are considered to be "transparent" for drawing purposes.
      /// </summary>
      public bool Dirty { get; set; } = true;
      public ScreenBuffer MyBuffer { get; set; }

      public List<char> transparencyCharacters = new List<char>();
      private EntityType myType;

      public Entity(EntityType type, params char[] transparents)
      {
         myType = type;
         transparencyCharacters.AddRange(transparents);
      }

      public void Draw()
      {
         const bool drawOrigin = true;
         if (Dirty)
         {
            //draw
            //0 visual, 1 color, 2 coll, 3 text
            //origin top left
            Console.Write(CGEUtility.GetColorANSIPrefix(0, 0, 0, false));
            StringBuilder buff = new StringBuilder();
            for (int v = 0; v < Height; v++)
            {
               char establishedColor = ' ';
               if (Top + v >= MyBuffer.Top - 1 && Top + v < MyBuffer.Top + MyBuffer.Height - 1)
               {
                  int sk = 0;
                  for (int w = 0; w < Width; w++)
                  {
                     //if (Left - (Width - 1) + w < MyBuffer.Left && Left - (Width - 1) + w >= MyBuffer.Left + (MyBuffer.Width - 1))
                     if (Left + w < MyBuffer.Left - 1)
                     {
                        sk++;
                        continue;
                     }
                     else if (Left + w >= MyBuffer.Left + MyBuffer.Width - 1)
                     {
                        continue;
                     }
                     if (drawOrigin)
                     {
                        if (v == 0 && w == 0)
                        {
                           buff.Append(CGEUtility.GetColorANSIPrefix(255, 20, 20, false) + CGEUtility.GetColorANSIPrefix(20, 255, 255) + 'O' + CGEUtility.GetColorANSIPrefix(0, 0, 0, false));
                           continue;
                        }
                     }
                     //TODO determine in the loop if we need to clip because we're outside of the screenbuffer.
                     if (establishedColor == Sprites.sprites[(int)myType][1, v][w])
                     {
                        buff.Append(Sprites.sprites[(int)myType][0, v][w]);
                     }
                     else
                     {
                        //append the ansi code of the new color
                        establishedColor = Sprites.sprites[(int)myType][1, v][w];
                        if (establishedColor != ' ')
                        {
                           buff.Append(Sprites.colorAliases[(int)myType][establishedColor]);
                        }
                        buff.Append(Sprites.sprites[(int)myType][0, v][w]);
                     }
                  }
                  Console.SetCursorPosition(Left + MyBuffer.Left + sk, Top + MyBuffer.Top + v);
                  Console.Write(buff.ToString());
                  buff.Clear();
               }
            }
            Dirty = false;
         }
      }
   }
}