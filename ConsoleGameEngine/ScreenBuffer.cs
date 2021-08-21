using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public class ScreenBuffer
   {
      #region Fields
      private List<Entity> entities;
      public int Top { get; set; }
      public int Left { get; set; }
      public int Width { get; private set; }
      public int Height { get; private set; }
      #endregion

      #region Ctor
      public ScreenBuffer(int top, int left, int width, int height)
      {
         entities = new List<Entity>();
         Top = top;
         Left = left;
         Width = width;
         Height = height;
      }
      #endregion

      #region Utility
      public void Draw(bool drawBorder = false)
      {
         string borderForeColor = CGEUtility.GetColorANSIPrefix(255, 255, 255);
         string borderBackColor = CGEUtility.GetColorANSIPrefix(0, 0, 0, false);
         entities.Sort(new Entity.EntityZComparer());
         foreach (Entity e in entities)
         {
            e.Draw();
         }
         if (drawBorder)
         {
            Console.Write(borderForeColor + borderBackColor);
            for (int i = -1; i < Height + 1; i++)
            {
               Console.SetCursorPosition(Left - 1, Top + i);
               if (i == -1)
               {
                  Console.Write(WindowBuilder.charTopLeftCornerBorder
                     + new string(WindowBuilder.charHorizontalBorder, Width)
                     + WindowBuilder.charTopRightCornerBorder);
               }
               else if (i == Height)
               {
                  Console.Write(WindowBuilder.charBottomLeftCornerBorder
                     + new string(WindowBuilder.charHorizontalBorder, Width)
                     + WindowBuilder.charBottomRightCornerBorder);
               }
               else
               {
                  Console.Write(WindowBuilder.charVerticalBorder);
                  Console.SetCursorPosition(Left + Width, Top + i);
                  Console.Write(WindowBuilder.charVerticalBorder);
               }
            }
         }
      }

      public void Redraw(bool drawBorder = false)
      {
         foreach (Entity e in entities)
         {
            e.Dirty = true;
         }
         Draw(drawBorder);
      }

      public void AddEntity(Entity e)
      {
         //if (e.MyBuffer != null) TODO: DO THIS
         //{
         //   if (e.MyBuffer.entities.Contains(e))
         //   {
         //      //this entity is already in another buffer. Make sure to break before make.
         //   }
         //   else
         //   {
         //      //This entity's buffer isn't null, but it doesn't belong to that buffer. Something odd is going on.
         //   }
         //}
         e.MyBuffer = this;
         entities.Add(e);
      }

      public void RemoveEntity(Entity e)
      {
         e.MyBuffer = null;
         entities.Remove(e);
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