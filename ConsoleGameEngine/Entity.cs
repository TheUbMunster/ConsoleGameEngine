using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   class Entity
   {
      public enum EntityType
      {
         PineTree = 0,
         LargePineTree,
         SmallPineTree,
         PineSapling,
         Brush
      }

      public class EntityZComparer : IComparer<Entity>
      {
         public int Compare(Entity x, Entity y)
         {
            return y.Z - x.Z;
         }
      }
      public int Left { get; private set; }
      public int Top { get; private set; }
      /// <summary>
      /// The lower the z value, the earlier it is drawn.
      /// </summary>
      public int Z { get; private set; }
      /// <summary>
      /// A list of all of the characters that are considered to be "transparent" for drawing purposes.
      /// </summary>
      public List<char> transparencyCharacters = new List<char>();
      private string[,] entityData; //TODO: SYNC INDEX WITH SPRITE DATA

      public Entity(EntityType type, params char[] transparents)
      {
         entityData = Sprites.sprites[(int)type]; //TODO: SYNC INDEX WITH SPRITE DATA
         transparencyCharacters.AddRange(transparents);
      }
   }
}