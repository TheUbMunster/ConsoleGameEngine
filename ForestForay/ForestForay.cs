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

         ConsoleWindow cw = new(40, 20);
         cw.DrawType |= ConsoleWindow.WindowDrawType.EntityMode;
         Renderer rend = new Renderer();
         rend.SetRootConsoleWindow(cw);
         Entity tree = new Entity() { BackingSprite = Sprite.PersistentSpriteTemplates[SpriteFactory.Tree] };
         cw.Entities.Add(tree);
         
         while (true)
         {
            rend.Draw();
            bool esc = false;
            while (Console.KeyAvailable)
            {
               switch (Console.ReadKey(true).Key)
               {
                  case ConsoleKey.W:
                     tree.Top--;
                     break;
                  case ConsoleKey.S:
                     tree.Top++;
                     break;
                  case ConsoleKey.A:
                     tree.Left--;
                     break;
                  case ConsoleKey.D:
                     tree.Left++;
                     break;
                  case ConsoleKey.Escape:
                     esc = true;
                     break;
               }
            }
            if (esc)
            {
               break;
            }
            Thread.Sleep(50);
         }
      }
   }
}