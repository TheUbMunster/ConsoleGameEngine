using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public static partial class CGEUtility
   {
      /// <summary>
      /// Determines if two rectangles are overlapping (adjacent isn't considered overlapping). If one or both of the rectangles
      /// have a dimension of 0 on one axis (i.e. the rectangle is actually a line) and they intersect with the other, they are considered to overlap.
      /// </summary>
      /// <param name="rect1p1">First point of the first rectangle.</param>
      /// <param name="rect1p2">Second point directly opposing the first point in the first rectangle</param>
      /// <param name="rect2p1">First point of the second rectangle.</param>
      /// <param name="rect2p2">Second point directly opposing the first point in the second rectangle</param>
      /// <returns>True if the rectangles overlap, false otherwise.</returns>
      public static bool IsIntersecting(VectorInt2 rect1p1, VectorInt2 rect1p2, VectorInt2 rect2p1, VectorInt2 rect2p2)
      {
         //VERIFY THIS.
         return !(Math.Min(rect1p1.Top, rect1p2.Top) >= Math.Max(rect2p1.Top, rect2p2.Top) ||
            Math.Min(rect2p1.Top, rect2p2.Top) >= Math.Max(rect1p1.Top, rect1p2.Top) ||
            Math.Min(rect1p1.Left, rect1p2.Left) >= Math.Max(rect2p1.Left, rect2p2.Left) ||
            Math.Min(rect2p1.Left, rect2p2.Left) >= Math.Max(rect1p1.Left, rect1p2.Left));
      }
   }
}