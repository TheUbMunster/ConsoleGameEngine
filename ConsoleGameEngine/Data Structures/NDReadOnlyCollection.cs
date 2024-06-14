using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Data_Structures
{
   internal class NDReadOnlyCollection<T>
   {
      //TODO: n-dimension read only collection.
      IReadOnlyList<T> flatData;
      int[] lengths;
      public NDReadOnlyCollection(IReadOnlyList<T> flattenedData, params int[] lengths) 
      {
         flatData = flattenedData;
         this.lengths = lengths;
      }
      public T this[params int[] indeces]
      {
         get => flatData[FlattenIndex(indeces)];
      }
      private int FlattenIndex(params int[] indeces)
      {
         int aggr = 0;
         for (int ind = DimensionCount() - 1; ind >= 0; ind--)
         {
            int lenProd = 1;
            for (int len = 0; len < ind; len++)
               lenProd *= GetLength(len);
            aggr += indeces[ind] * lenProd;
         }
         return aggr;
      }
      public int GetLength(int dimension) => lengths[dimension];
      public int DimensionCount() => lengths.Length;
   }
}
