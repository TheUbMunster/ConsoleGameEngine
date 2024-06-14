using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   /// <summary>
   /// Contains all the rasterized information necessary to draw a frame
   /// </summary>
   public class FrameInfo
   {
      public char[,] Chars { get; init; }
      public int[,] ColorCodes { get; init; }
      public IReadOnlyDictionary<int, string> ColorCodesLookup { get; init; }
   }
}
