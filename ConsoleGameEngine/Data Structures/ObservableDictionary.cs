using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Data_Structures
{
   public class ObservableDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
   {
      private Dictionary<TKey, TValue> internalDictionary = new();
      public event NotifyCollectionChangedEventHandler CollectionChanged;

      public void Add(TKey key, TValue value)
      {
         internalDictionary.Add(key, value);
         CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
      }

      public bool Remove(TKey key, out TValue value)
      {
         bool res = internalDictionary.Remove(key, out value);
         CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value));
         return res;
      }

      public bool Remove(TKey key) => Remove(key, out _);

      public void Clear()
      {
         internalDictionary.Clear();
         CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }

      public TValue this[TKey key]
      {
         get { return internalDictionary[key]; }
         set 
         {
            TValue oldv = internalDictionary[key];
            TValue newv = internalDictionary[key] = value;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newv, oldv));
         }
      }
      public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
      {
         return ((IEnumerable<KeyValuePair<TKey, TValue>>)internalDictionary).GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return ((IEnumerable)internalDictionary).GetEnumerator();
      }
   }
}