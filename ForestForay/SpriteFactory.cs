using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestForay
{
   public static class SpriteFactory
   {
      public static readonly int Tree = Sprite.CreatePersistentSpriteTemplate(
         new List<NDLockableCollection<char>> { new NDLockableCollection<char>(
               @"      /\      ".Concat(
               @"     /\\\     ").Concat(
               @"    /\/\\\    ").Concat(
               @"    //\/\\    ").Concat(
               @"   /\//\\\\   ").Concat(
               @"  //\///\/\\  ").Concat(
               @"  //\//\//\\  ").Concat(
               @" //\///\\/\\\ ").Concat(
               @"/\///\/\\\\\/\").Concat(
               @"      ||      "),
            14, 10) },
         new List<NDLockableCollection<int>> { new NDLockableCollection<int>(
               new List<int> {
               0,0,0,0,0,0,1,2,0,0,0,0,0,0,
               0,0,0,0,0,1,2,2,2,0,0,0,0,0,
               0,0,0,0,1,2,1,2,2,2,0,0,0,0,
               0,0,0,0,1,1,2,1,2,2,0,0,0,0,
               0,0,0,1,2,1,1,2,2,2,2,0,0,0,
               0,0,1,1,2,1,1,1,2,1,2,2,0,0,
               0,0,1,1,2,1,1,2,1,1,2,2,0,0,
               0,1,1,2,1,1,1,2,2,1,2,2,2,0,
               1,2,1,1,1,2,1,2,2,2,2,2,1,2,
               0,0,0,0,0,0,3,3,0,0,0,0,0,0, },
            14, 10) },
         new List<NDLockableCollection<bool>> { new NDLockableCollection<bool>(
               new List<bool> { 
               false,false,false,false,false,false,true, true, false,false,false,false,false,false,
               false,false,false,false,false,true, true, true, true, false,false,false,false,false,
               false,false,false,false,true, true, true, true, true, true, false,false,false,false,
               false,false,false,false,true, true, true, true, true, true, false,false,false,false,
               false,false,false,true, true, true, true, true, true, true, true, false,false,false,
               false,false,true, true, true, true, true, true, true, true, true, true, false,false,
               false,false,true, true, true, true, true, true, true, true, true, true, false,false,
               false,true, true, true, true, true, true, true, true, true, true, true, true, false,
               true, true, true, true, true, true, true, true, true, true, true, true, true, true, 
               false,false,false,false,false,false,true, true, false,false,false,false,false,false, },
            14, 10) },
         new Dictionary<int, string>
         {
            { 0, "error" }, //the mask should prevent "0" from ever being accessed.
            { 1, CGEUtility.GetColorANSIPrefix(42, 138, 37) },
            { 2, CGEUtility.GetColorANSIPrefix(18, 82, 15) },
            { 3, CGEUtility.GetColorANSIPrefix(82, 47, 15) },
         });
   }
}
