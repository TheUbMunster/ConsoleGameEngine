using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   class ScreenBuffer
   {
      #region Fields
      private SortedSet<Entity> entities;
      #endregion

      #region Ctor
      public ScreenBuffer()
      {
         entities = new SortedSet<Entity>(new Entity.EntityZComparer());
      }
      #endregion

      #region Utility
      public void Draw()
      {

      }

      public void Redraw()
      {

      }
      #endregion

      #region QOL
#if DEBUG
      /// <summary>
      /// Takes the given ScreenBuffer, renders it, bakes the appearance and saves it in a file for deveopment purposes
      /// </summary>
      /// <returns>The filepath to the temp file containing the baked data</returns>
      public static string Bake(ScreenBuffer sb)
      {
         throw new NotImplementedException();
      }
#endif
      #endregion
   }
}