using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public static partial class CGEUtility
   {
      public struct VectorInt2
      {
         public int Left { get; init; }
         public int Top { get; init; }

         public VectorInt2(int left, int top)
         {
            Left = left;
            Top = top;
         }

         public static bool operator==(VectorInt2 left, VectorInt2 right)
         {
            return left.Left == right.Left && left.Top == right.Top;
         }
         public static bool operator !=(VectorInt2 left, VectorInt2 right)
         {
            return left.Left != right.Left || left.Top != right.Top;
         }

         public override string ToString()
         {
            return $"Left: {Left}, Top: {Top}";
         }
      }
   }
}
