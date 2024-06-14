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
      public Renderer() { }
      public void SetRootConsoleWindow(ConsoleWindow cw)
      {
         rootWindow = cw;
         //set width + height of terminal
      }
      public void Draw()
      {
         FrameInfo info = rootWindow.Draw(); //this needs to return some "info"
         //then we draw that info to the console
         //make it so that "info" is only the "diff" between the previous screen and this one so that we don't waste compute
         //especially since old-style terminals seek() functions are so freaking slow.
         Console.SetCursorPosition(0, 0);
         for (int x = 0; x < info.Width; x++)
         {
            for (int y = 0; y < info.Height; y++)
            {
               Console.Write(info.ColorCodesLookup[info.ColorCodes[x, y]] + info.Chars[x, y]);
            }
            if (x != info.Width - 1)
               Console.WriteLine();
         }
      }
   }
}
