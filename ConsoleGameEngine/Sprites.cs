using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public static partial class Sprites
   {
      /// <summary>
      /// First dimension is the individual sprites type (index of EntityType), second is the components of that sprite data e.g. graphical, color, collision, message, third is the individual rows of the selected data.
      /// </summary>
      public static string[][,] sprites = new string[][,]
      {
         new string [,]
         {
            {
               @"_o_",
               @" | ",
               @"/ \",
            },
            {
               @"rwr",
               @" r ",
               @"b b",
            },
            {
               @"###",
               @" # ",
               @"# #",
            },
            {
               @"   ",
               @"   ",
               @"   ",
            },
         },
         new string [,]
         {
            {
               @"      /\      ",
               @"     /\\\     ",
               @"    /\/\\\    ",
               @"    //\/\\    ",
               @"   /\//\\\\   ",
               @"  //\///\/\\  ",
               @"  //\//\//\\  ",
               @" //\///\\/\\\ ",
               @"/\///\/\\\\\/\",
               @"      ||      ",
               @"              ",
            },
            {
               @"      gd      ",
               @"     gddd     ",
               @"    gdgddd    ",
               @"    ggdgdd    ",
               @"   gdggdddd   ",
               @"  ggdgggdgdd  ",
               @"  ggdggdggdd  ",
               @" ggdgggddgddd ",
               @"gdgggdgdddddgd",
               @"      bb      ",
               @"              ",
            },
            {
               @"              ",
               @"              ",
               @"              ",
               @"              ",
               @"              ",
               @"              ",
               @"              ",
               @"     ####     ",
               @"     ####     ",
               @"     ####     ",
               @"              ",
            },
            {
               @"              ",
               @"              ",
               @"              ",
               @"              ",
               @"              ",
               @"   11111111   ",
               @"   11111111   ",
               @"   11111111   ",
               @"   11111111   ",
               @"   11111111   ",
               @"   11111111   ",
            },
         },
      };
   }
}