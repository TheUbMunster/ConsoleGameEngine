using ConsoleGameEngine.DataStructures;
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
      public string Meta { get; init; } = string.Empty;
      public int Width { get => Chars.GetLength(0); }
      public int Height { get => Chars.GetLength(1); }
      public NDLockableCollection<char> Chars { get; init; }
      public NDLockableCollection<int> ColorCodes { get; init; }
      public IReadOnlyDictionary<int, string> ColorCodesLookup { get; init; }
   }
}
