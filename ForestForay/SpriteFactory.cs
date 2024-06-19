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
         new List<NDCollection<char>> { new NDCollection<char>(
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
         new List<NDCollection<int>> { new NDCollection<int>(
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
         new List<NDCollection<bool>> { new NDCollection<bool>(
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
            { 1, ConsoleUtil.GetColorANSIPrefix(42, 138, 37) },
            { 2, ConsoleUtil.GetColorANSIPrefix(18, 82, 15) },
            { 3, ConsoleUtil.GetColorANSIPrefix(82, 47, 15) },
         });
      public static readonly int Player = Sprite.CreatePersistentSpriteTemplate(
         new List<NDCollection<char>> { new NDCollection<char>(
               @"_o_".Concat(
               @" | ").Concat(
               @"/ \"),
            3, 3) },
         new List<NDCollection<int>> { new NDCollection<int>(
               new List<int> {
               1,2,1,
               0,1,0,
               3,0,3, },
            3, 3) },
         new List<NDCollection<bool>> { new NDCollection<bool>(
               new List<bool> { 
               true, true, true,
               false,true, false,
               true, false,true, },
            3, 3) },
         new Dictionary<int, string>
         {
            { 0, "error" }, //the mask should prevent "0" from ever being accessed.
            { 1, ConsoleUtil.GetColorANSIPrefix(232, 68, 46) },
            { 2, ConsoleUtil.GetColorANSIPrefix(235, 210, 197) },
            { 3, ConsoleUtil.GetColorANSIPrefix(46, 65, 158) },
         });
      public static readonly int Shrub = Sprite.CreatePersistentSpriteTemplate(
         new List<NDCollection<char>> 
         {
            new NDCollection<char>(
               @" _   _ ".Concat(
               @"__\|/__"),
            7, 2),
            new NDCollection<char>(
               @" \ | / ".Concat(
               @"__\|/__"),
            7, 2)
         },
         new List<NDCollection<int>> 
         {
            new NDCollection<int>(
               new List<int> {
               0,1,0,0,0,3,0,
               2,2,1,2,3,2,2 },
            7, 2),
            new NDCollection<int>(
               new List<int> {
               0,4,0,5,0,6,0,
               5,5,4,5,6,5,5 },
            7, 2),
         },
         new List<NDCollection<bool>> 
         {
            new NDCollection<bool>(
               new List<bool> {
               false,true, false,false,false,true, false,
               true, true, true, true, true, true, true, },
            7, 2),
            new NDCollection<bool>(
               new List<bool> {
               false,true, false,true, false,true, false,
               true, true, true, true, true, true, true, },
            7, 2),
         },
         new Dictionary<int, string>
         {
            { 0, "error" }, //the mask should prevent "0" from ever being accessed.
            { 1, ConsoleUtil.GetColorANSIPrefix(181, 142, 63) },
            { 2, ConsoleUtil.GetColorANSIPrefix(135, 102, 36) },
            { 3, ConsoleUtil.GetColorANSIPrefix(84, 62, 18) },
            { 4, ConsoleUtil.GetColorANSIPrefix(40, 142, 63) },
            { 5, ConsoleUtil.GetColorANSIPrefix(30, 102, 36) },
            { 6, ConsoleUtil.GetColorANSIPrefix(20, 62, 18) },
         });
   }
}
