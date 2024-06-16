using ConsoleGameEngine.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public class Sprite
   {
      //todo: non-persistent sprites?

      #region Static Fields
      private static int persistenSpriteCreationCounter = -1;
      private static Dictionary<int, Sprite> persistentSpriteTemplates = new();
      public static IReadOnlyDictionary<int, Sprite> PersistentSpriteTemplates { get => persistentSpriteTemplates; }
      #endregion

      #region Fields
      public int? PersistentSpriteId { get; private init; } = null;
      public IReadOnlyList<NDCollection<char>> Chars { get; init; }
      public IReadOnlyList<NDCollection<int>> ColorCodes { get; init; }
      public IReadOnlyList<NDCollection<bool>> DisplayMask { get; init; } //if any value is false, that "pixel" is treated like it's transparent.
      public IReadOnlyDictionary<int, string> ColorCodesLookup { get; init; }
      public int Width { get => Chars[0].GetLength(0); }
      public int Height { get => Chars[0].GetLength(1); }
      #endregion

      private Sprite() { }
      /// <summary>
      /// Any reference objects sent to this function should not be modified after the fact.
      /// </summary>
      /// <returns>A key to the <see cref="PersistentSpriteTemplates"/> collection.</returns>
      public static int CreatePersistentSpriteTemplate(IReadOnlyList<NDCollection<char>> chars, IReadOnlyList<NDCollection<int>> colorCodes, IReadOnlyList<NDCollection<bool>> displayMask, IReadOnlyDictionary<int, string> colorCodesLookup)
      {
         bool invalid = false;
         if (chars.Count != colorCodes.Count || chars.Count != displayMask.Count)
            invalid = true;
         if (invalid)
            throw new ArgumentException("Error: Invalid arguments when creating persistent sprite template. Likely mismatch in dimension of sprite data");
         Sprite s = new Sprite()
         {
            Chars = chars,
            ColorCodes = colorCodes,
            DisplayMask = displayMask,
            ColorCodesLookup = colorCodesLookup,
            PersistentSpriteId = ++persistenSpriteCreationCounter
         };
         persistentSpriteTemplates.Add(s.PersistentSpriteId.Value, s);
         return s.PersistentSpriteId.Value;
      }
   }
}
