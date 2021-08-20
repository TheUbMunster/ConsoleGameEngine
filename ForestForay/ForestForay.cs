using System;
using ConsoleGameEngine;

namespace ForestForay
{
   class ForestForay
   {
      static void Main(string[] args)
      {
         CGE.Initialize();
         Console.WriteLine(CGEUtility.GetColorANSIPrefix(160, 20, 80) + "Hello World!");
         Console.SetCursorPosition(2, 0);
         Console.Write(CGEUtility.GetColorANSIPrefix(20, 20, 240) + "ll");
         Console.Read();
         //for (int i = 0; i < 255; i += 4)
         //{
         //   for (int j = 0; j < 255; j += 4)
         //   {
         //      for (int k = 0; k < 255; k += 4)
         //      {
         //         Console.WriteLine(CGEUtility.GetColorANSIPrefix(i, j, k) + "Hello World!");
         //      }
         //   }
         //}
      }
   }
}