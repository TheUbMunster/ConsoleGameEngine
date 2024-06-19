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

         ConsoleWindow mainCw = new(80, 25);
         mainCw.DrawType |= ConsoleWindow.WindowDrawType.EntityMode | ConsoleWindow.WindowDrawType.WindowMode | ConsoleWindow.WindowDrawType.RawMode;

         ConsoleWindow subCw = new ConsoleWindow(12, 5);
         subCw.DrawType |= ConsoleWindow.WindowDrawType.EntityMode;
         subCw.Left = 3; subCw.Top = 4;
         mainCw.ChildConsoleWindows.Add(subCw);
         
         Renderer rend = new Renderer();
         rend.SetRootConsoleWindow(mainCw);

         Entity tree = new Entity() { BackingSprite = Sprite.PersistentSpriteTemplates[SpriteFactory.Tree] };
         mainCw.Entities.Add(tree);
         Entity shrub = new Entity() { BackingSprite = Sprite.PersistentSpriteTemplates[SpriteFactory.Shrub], Left = 10, AnimationRate = 1f };
         mainCw.Entities.Add(shrub);

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
                     shrub.Top--;
                     mainCw.IsDirty = true;
                     break;
                  case ConsoleKey.S:
                     tree.Top++;
                     shrub.Top++;
                     mainCw.IsDirty = true;
                     break;
                  case ConsoleKey.A:
                     tree.Left--;
                     shrub.Left--;
                     mainCw.IsDirty = true;
                     break;
                  case ConsoleKey.D:
                     tree.Left++;
                     shrub.Left++;
                     mainCw.IsDirty = true;
                     break;

                  case ConsoleKey.UpArrow:
                     player.Top--;
                     subCw.IsDirty = true;
                     break;
                  case ConsoleKey.DownArrow:
                     player.Top++;
                     subCw.IsDirty = true;
                     break;
                  case ConsoleKey.LeftArrow:
                     player.Left--;
                     subCw.IsDirty = true;
                     break;
                  case ConsoleKey.RightArrow:
                     player.Left++;
                     subCw.IsDirty = true;
                     break;

                  case ConsoleKey.Spacebar:
                     mainCw.RawColorCodesLookup[1] = ConsoleUtil.GetColorANSIPrefix(220, 20, 220);
                     mainCw.RawColorCodes[0, 0] = 1;
                     mainCw.RawChars[0, 0] = 'F';
                     mainCw.RawDisplayMask[0, 0] = true;
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