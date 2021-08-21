using System;
using ConsoleGameEngine;
using System.Threading;

namespace ForestForay
{
   class ForestForay
   {
      static void Main(string[] args)
      {
         CGE.Initialize();

         ScreenBuffer sb = new(1, 1, 50, 20);
         Entity e = new Entity(Entity.EntityType.PineTree, ' ');
         sb.AddEntity(e);
         sb.Draw(true);
         MoveTreeRoutine(e);
      }

      static void MoveTreeRoutine(Entity e)
      {
         while (true)
         {
            bool esc = false;
            if (Console.KeyAvailable)
            {
               switch(Console.ReadKey(true).Key)
               {
                  case ConsoleKey.W:
                     e.MyBuffer.Top--;
                     e.MyBuffer.Redraw(true);
                     break;
                  case ConsoleKey.S:
                     e.MyBuffer.Top++;
                     e.MyBuffer.Redraw(true);
                     break;
                  case ConsoleKey.A:
                     e.MyBuffer.Left--;
                     e.MyBuffer.Redraw(true);
                     break;
                  case ConsoleKey.D:
                     e.MyBuffer.Left++;
                     e.MyBuffer.Redraw(true);
                     break;
                  case ConsoleKey.UpArrow:
                     e.Top--;
                     e.Draw();
                     break;
                  case ConsoleKey.DownArrow:
                     e.Top++;
                     e.Draw();
                     break;
                  case ConsoleKey.LeftArrow:
                     e.Left--;
                     e.Draw();
                     break;
                  case ConsoleKey.RightArrow:
                     e.Left++;
                     e.Draw();
                     break;
                  case ConsoleKey.Escape:
                     esc = true;
                     break;
               }
               while(Console.KeyAvailable)
               {
                  Console.ReadKey(true); //consume additional characters.
               }
            }
            if (esc)
            {
               break;
            }
            Thread.Sleep(100);
         }
      }
   }
}