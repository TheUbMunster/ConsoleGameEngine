using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ConsoleGameEngine
{
   public static partial class CGEUtility
   {
      public abstract class ConsoleSaveableData
      {
         #region Fields
         private Dictionary<string, string> stringData;
         private Dictionary<string, char> charData;
         private Dictionary<string, bool> boolData;
         private Dictionary<string, byte> byteData;
         private Dictionary<string, short> shortData;
         private Dictionary<string, int> intData;
         private Dictionary<string, long> longData;
         private Dictionary<string, float> floatData;
         private Dictionary<string, double> doubleData;
         private Dictionary<string, decimal> decimalData;

         public const uint version = 0; //not sure if this is usable with the json serializer limitations
         #endregion

         #region Default Ctor
         public ConsoleSaveableData()
         {
            stringData = new();
            charData = new();
            boolData = new();
            byteData = new();
            shortData = new();
            intData = new();
            longData = new();
            floatData = new();
            doubleData = new();
            decimalData = new();
         }
         #endregion

         #region Data Interaction
         public bool ContainsString(string key) => stringData.ContainsKey(key);
         public bool ContainsChar(string key) => charData.ContainsKey(key);
         public bool ContainsBool(string key) => boolData.ContainsKey(key);
         public bool ContainsByte(string key) => byteData.ContainsKey(key);
         public bool ContainsShort(string key) => shortData.ContainsKey(key);
         public bool ContainsInt(string key) => intData.ContainsKey(key);
         public bool ContainsLong(string key) => longData.ContainsKey(key);
         public bool ContainsFloat(string key) => floatData.ContainsKey(key);
         public bool ContainsDouble(string key) => doubleData.ContainsKey(key);
         public bool ContainsDecimal(string key) => decimalData.ContainsKey(key);

         public string GetString(string key) => stringData[key];
         public char GetChar(string key) => charData[key];
         public bool GetBool(string key) => boolData[key];
         public byte GetByte(string key) => byteData[key];
         public short GetShort(string key) => shortData[key];
         public int GetInt(string key) => intData[key];
         public long GetLong(string key) => longData[key];
         public float GetFloat(string key) => floatData[key];
         public double GetDouble(string key) => doubleData[key];
         public decimal GetDecimal(string key) => decimalData[key];

         public void SetString(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         public void SetChar(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         public void SetBool(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         public void SetByte(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         public void SetShort(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         public void SetInt(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         public void SetLong(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         public void SetFloat(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         public void SetDouble(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         public void SetDecimal(string key, string value)
         {
            if (!stringData.TryAdd(key, value))
            {
               stringData[key] = value;
            }
         }
         #endregion

         #region Data Serialization/Deserialization
         public virtual string Serialize()
         {
            return JsonSerializer.Serialize(this, this.GetType());
         }

         public static T Deserialize<T>(string json) where T : ConsoleSaveableData
         {
            return JsonSerializer.Deserialize<T>(json);
         }
         #endregion
      }

      //public class D : ConsoleSaveableData
      //{
      //   public new const uint version = 1; //see above issue on the version numbers
      //}

      public static void SaveDataObject<T>(T data) where T : ConsoleSaveableData
      {
         string json = data.Serialize();
         //save the json in a file somewhere where it can be loaded again.
      }

      //public static uint GetVersionNumber(string json)
      //{
      //   JsonDocument.par
      //}

      public static T LoadDataObject<T>() where T : ConsoleSaveableData, new()
      {
         //read all the json from the saved location.
         string json = string.Empty;
         return ConsoleSaveableData.Deserialize<T>(json);
      }
   }
}