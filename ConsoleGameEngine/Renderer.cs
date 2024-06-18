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
         if (lastFrameInfo == info) //instead of this, do dirty flags
            return;
         //then we draw that info to the console
         //make it so that "info" is only the "diff" between the previous screen and this one so that we don't waste compute
         //especially since old-style terminals seek() functions are so freaking slow.

         //ideas for speed:
         //quadtree diff?


         int top = (Console.BufferHeight - rootWindow.Height) / 2, left = (Console.BufferWidth - rootWindow.Width) / 2;
         //old inefficent way?:
         /*
          50% of CPU time is spent PURELY on Console.SetCursorPosition
          25% of CPU time is spent PURELY on Console.Write
          */
         //for (int x = 0; x < info.Width; x++)
         //{
         //   for (int y = 0; y < info.Height; y++)
         //   {
         //      int screenx = left + x, screeny = top + y;
         //      Console.SetCursorPosition(screenx, screeny);
         //      Console.Write(info.ColorCodesLookup[info.ColorCodes[x, y]] + info.Chars[x, y]);
         //   }
         //}
         //new way
         /*
          70% of CPU time is spent PURELY on Console.SetCursorPosition
          10% of CPU time is spent PURELY on Console.Write

          Keep in minds these values are the portions of the total times taken. Overall, I believe this is faster. Needs more investigating.
          */
         for (int y = 0; y < info.Height; y++)
         {
            int screeny = top + y;
            Console.SetCursorPosition(left, screeny);
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < info.Width; x++)
               sb.Append(info.ColorCodesLookup[info.ColorCodes[x, y]] + info.Chars[x, y]);
            Console.Write(sb.ToString());
         }
         string white = ConsoleUtil.GetColorANSIPrefix(255, 255, 255);
         /*
          With this double for loop only setting the cursor position when absolutely necessary:

          22% of CPU time is spent PURELY on Console.SetCursorPosition
          18% of CPU time is spent PURELY on Console.Write

          These percentages include the time spent on SetCursorPosition and Write from the above code too.
          */
         for (int x = 0, bx = info.Width + 2; x < bx; x++)
         {
            for (int y = 0, by = info.Height + 2; y < by; y++)
            {
               int screenx = x + left - 1, screeny = y + top - 1;
               if (x == 0 && y == 0) //top left corner
               {
                  Console.SetCursorPosition(screenx, screeny); //this only needs to execute *if* we draw.
                  Console.Write(white + ConsoleUtil.charTopLeftCornerBorder);
               }
               else if (x == 0 && y == by - 1) //bottom left corner
               {
                  Console.SetCursorPosition(screenx, screeny); //this only needs to execute *if* we draw.
                  Console.Write(white + ConsoleUtil.charBottomLeftCornerBorder);
               }
               else if (x == bx - 1 && y == 0) //top right corner
               {
                  Console.SetCursorPosition(screenx, screeny); //this only needs to execute *if* we draw.
                  Console.Write(white + ConsoleUtil.charTopRightCornerBorder);
               }
               else if (x == bx - 1 && y == by - 1) //bottom right corner
               {
                  Console.SetCursorPosition(screenx, screeny); //this only needs to execute *if* we draw.
                  Console.Write(white + ConsoleUtil.charBottomRightCornerBorder);
               }
               else if ((y == 0 || y == by - 1) && (x > 0 && x < bx)) //horiz edges
               {
                  Console.SetCursorPosition(screenx, screeny); //this only needs to execute *if* we draw.
                  if (x - 1 < info.Meta.Length)
                     Console.Write(white + info.Meta[x - 1]); //- 1 because the very first horiz edge starts at x=1
                  else
                     Console.Write(white + ConsoleUtil.charHorizontalBorder);
               }
               else if ((x == 0 || x == bx - 1) && (y > 0 && y < by)) //vert edges
               {
                  Console.SetCursorPosition(screenx, screeny); //this only needs to execute *if* we draw.
                  Console.Write(white + ConsoleUtil.charVerticalBorder);
               }
            }
         }
         lastFrameInfo = info;
      }
   }
}
