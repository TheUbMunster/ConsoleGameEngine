using System;
using System.Threading;
using ConsoleGameEngine;

namespace ForestForay
{
   class ForestForay
   {
      static void Main(string[] args)
      {
         ConsoleUtil.Initialize();

         ConsoleWindow cw = new(20, 20);
         cw.DrawType |= ConsoleWindow.WindowDrawType.EntityMode;
         Renderer rend = new Renderer();
         rend.SetRootConsoleWindow(cw);
         cw.Entities.Add(new Entity() { BackingSprite = Sprite.PersistentSpriteTemplates[SpriteFactory.Tree] });
         rend.Draw();
         //CGE.Initialize();

         //ScreenBuffer sb = new(6, 8, 50, 20);
         //Entity e1 = new Entity(Entity.EntityType.PineTree, ' ');
         //e1.Left = e1.Width;
         //Entity e2 = new Entity(Entity.EntityType.Brush, ' ');
         //e2.Top = e2.Height * 2;
         //Entity e3 = new Entity(Entity.EntityType.Player, ' ');
         //sb.AddEntity(e1);
         //sb.AddEntity(e2);
         //sb.AddEntity(e3);
         //sb.Draw(true);
         //MoveTreeRoutine(e3);
      }

      //static void MoveTreeRoutine(Entity e)
      //{
      //   while (true)
      //   {
      //      bool esc = false;
      //      if (Console.KeyAvailable)
      //      {
      //         switch (Console.ReadKey(true).Key)
      //         {
      //            case ConsoleKey.W:
      //               e.MyBuffer.Top--;
      //               e.MyBuffer.Redraw(true);
      //               break;
      //            case ConsoleKey.S:
      //               e.MyBuffer.Top++;
      //               e.MyBuffer.Redraw(true);
      //               break;
      //            case ConsoleKey.A:
      //               e.MyBuffer.Left--;
      //               e.MyBuffer.Redraw(true);
      //               break;
      //            case ConsoleKey.D:
      //               e.MyBuffer.Left++;
      //               e.MyBuffer.Redraw(true);
      //               break;
      //            case ConsoleKey.UpArrow:
      //               e.Top--;
      //               e.Draw();
      //               break;
      //            case ConsoleKey.DownArrow:
      //               e.Top++;
      //               e.Draw();
      //               break;
      //            case ConsoleKey.LeftArrow:
      //               e.Left--;
      //               e.Draw();
      //               break;
      //            case ConsoleKey.RightArrow:
      //               e.Left++;
      //               e.Draw();
      //               break;
      //            case ConsoleKey.Escape:
      //               esc = true;
      //               break;
      //         }
      //         while (Console.KeyAvailable)
      //         {
      //            Console.ReadKey(true); //consume additional characters.
      //         }
      //      }
      //      if (esc)
      //      {
      //         break;
      //      }
      //      Thread.Sleep(100);
      //   }
      //}
   }
}