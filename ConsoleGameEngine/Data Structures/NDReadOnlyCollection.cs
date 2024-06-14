using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.DataStructures
{
   public class NDLockableCollection<T>
   {
      //TODO: n-dimension read only collection.
      private T[] flatData;
      private int[] lengths;
      public bool Locked { get; private set; } = false;
      public NDLockableCollection(IEnumerable<T> flattenedData, params int[] lengths) : this(flattenedData.ToList(), lengths) { }
      public NDLockableCollection(IReadOnlyList<T> flattenedData, params int[] lengths) 
      {
         flatData = flattenedData.ToArray();
         this.lengths = lengths;
      }
      public T this[params int[] indeces]
      {
         get => flatData[FlattenIndex(indeces)];
         set
         {
            if (!Locked)
               flatData[FlattenIndex(indeces)] = value;
            else
               throw new InvalidOperationException("Tried to modify a locked NDLockableCollection.");
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
      public void Lock() => Locked = true;
   }
}
