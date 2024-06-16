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

         ConsoleWindow mainCw = new(60, 20);
         mainCw.DrawType |= ConsoleWindow.WindowDrawType.EntityMode | ConsoleWindow.WindowDrawType.WindowMode;

         ConsoleWindow subCw = new ConsoleWindow(12, 5);
         subCw.DrawType |= ConsoleWindow.WindowDrawType.EntityMode;
         subCw.Left = 3; subCw.Top = 4;
         mainCw.ChildConsoleWindows.Add(subCw);
         
         Renderer rend = new Renderer();
         rend.SetRootConsoleWindow(mainCw);

         Entity tree = new Entity() { BackingSprite = Sprite.PersistentSpriteTemplates[SpriteFactory.Tree] };
         mainCw.Entities.Add(tree);

         Entity player = new Entity() { BackingSprite = Sprite.PersistentSpriteTemplates[SpriteFactory.Player] };
         subCw.Entities.Add(player);
         
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

                  case ConsoleKey.UpArrow:
                     player.Top--;
                     break;
                  case ConsoleKey.DownArrow:
                     player.Top++;
                     break;
                  case ConsoleKey.LeftArrow:
                     player.Left--;
                     break;
                  case ConsoleKey.RightArrow:
                     player.Left++;
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