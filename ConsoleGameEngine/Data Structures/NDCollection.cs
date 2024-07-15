using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.DataStructures
{
   public class NDCollection<T>
   {
      /// <summary>
      /// Params: indeces, oldValue, newvalue
      /// </summary>
      public event Action<int[], T, T> OnContentsChanged;
      private T[] flatData;
      private int[] lengths;
      public NDCollection(params int[] lengths) : this(Enumerable.Repeat(default(T), lengths.Aggregate(1, (a, b) => a * b)), lengths) { }
      public NDCollection(IReadOnlyList<T> flattenedData, params int[] lengths) : this(flattenedData.AsEnumerable(), lengths) { }
      public NDCollection(IEnumerable<T> flattenedData, params int[] lengths)
      {
         flatData = flattenedData.ToArray();
         this.lengths = lengths;
      }
      public T this[params int[] indeces]
      {
         get => flatData[FlattenIndex(indeces)];
         set
         {
            if (!flatData[FlattenIndex(indeces)].Equals(value))
            {
               int flat = FlattenIndex(indeces);
               T oldv = flatData[flat];
               T newv = value;
               flatData[flat] = value;
               OnContentsChanged?.Invoke(indeces, oldv, newv);
            }
         }
      }
      //generalization of
      //https://stackoverflow.com/questions/7367770/how-to-flatten-or-index-3d-array-in-1d-array
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
