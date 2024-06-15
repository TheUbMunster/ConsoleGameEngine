using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public class Renderer
   {
      private ConsoleWindow rootWindow;
      private FrameInfo lastFrameInfo;
      public Renderer() { }
      public void SetRootConsoleWindow(ConsoleWindow cw)
      {
         rootWindow = cw;
         //set width + height of terminal
         ConsoleUtil.ResizeConsole(cw.Width + 2, cw.Height + 2);
      }
      public void Draw()
      {
         FrameInfo info = rootWindow.Draw(); //this needs to return some "info"
         if (lastFrameInfo == info)
            return;
         //then we draw that info to the console
         //make it so that "info" is only the "diff" between the previous screen and this one so that we don't waste compute
         //especially since old-style terminals seek() functions are so freaking slow.

         //ideas for speed:
         //quadtree diff?


         int top = (Console.BufferHeight - rootWindow.Height) / 2, left = (Console.BufferWidth - rootWindow.Width) / 2;
         for (int x = 0; x < info.Width; x++)
         {
            for (int y = 0; y < info.Height; y++)
            {
               int screenx = left + x, screeny = top + y;
               Console.SetCursorPosition(screenx, screeny);
               Console.Write(info.ColorCodesLookup[info.ColorCodes[x, y]] + info.Chars[x, y]);
            }
         }
         string white = ConsoleUtil.GetColorANSIPrefix(255, 255, 255);
         for (int x = 0, bx = info.Width + 2; x < bx; x++)
         {
            for (int y = 0, by = info.Height + 2; y < by; y++)
            {
               int screenx = x + left - 1, screeny = y + top - 1;
               Console.SetCursorPosition(screenx, screeny);
               if (x == 0 && y == 0) //top left corner
                  Console.Write(white + ConsoleUtil.charTopLeftCornerBorder);
               else if (x == 0 && y == by - 1) //bottom left corner
                  Console.Write(white + ConsoleUtil.charBottomLeftCornerBorder);
               else if (x == bx - 1 && y == 0) //top right corner
                  Console.Write(white + ConsoleUtil.charTopRightCornerBorder);
               else if (x == bx - 1 && y == by - 1) //bottom right corner
                  Console.Write(white + ConsoleUtil.charBottomRightCornerBorder);
               else if ((y == 0 || y == by - 1) && (x > 0 && x < bx)) //horiz edges
               {
                  if (x - 1 < info.Meta.Length)
                     Console.Write(white + info.Meta[x - 1]); //- 1 because the very first horiz edge starts at x=1
                  else
                     Console.Write(white + ConsoleUtil.charHorizontalBorder);
               }
               else if ((x == 0 || x == bx - 1) && (y > 0 && y < by)) //vert edges
                  Console.Write(white + ConsoleUtil.charVerticalBorder);
            }
         }
         lastFrameInfo = info;
      }
   }
}
