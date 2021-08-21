using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public class Entity
   {
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
      public int Left { get; set; }
      public int Top { get; set; }
      /// <summary>
      /// The lower the z value, the earlier it is drawn.
      /// </summary>
      public int Z { get; set; }
      /// <summary>
      /// A list of all of the characters that are considered to be "transparent" for drawing purposes.
      /// </summary>
      public List<char> transparencyCharacters = new List<char>();
      public int Width { get => Sprites.sprites[(int)myType][0, 0].Length; }
      public int Height { get => Sprites.sprites[(int)myType].GetLength(1); }
      public bool Dirty { get; set; } = true;

      private EntityType myType;

      public Entity(EntityType type, params char[] transparents)
      {
         myType = type;
         transparencyCharacters.AddRange(transparents);
      }

      public void Draw()
      {
         if (Dirty)
         {
            //draw
            //0 visual, 1 color, 2 coll, 3 text
            StringBuilder buff = new StringBuilder();
            for (int v = 0; v < Height; v++)
            {
               char establishedColor = ' ';
               for (int w = 0; w < Width; w++)
               {
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
               Console.SetCursorPosition(Left - (Width - 1), (Top - (Height - 1)) + v);
               Console.Write(buff.ToString());
               buff.Clear();
            }
            Dirty = false;
         }
      }
   }
}