using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.old
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
         new string [,]
         {
            {
               @" \ | / ",
               @"__\|/__",
            },
            {
               @" t b B ",
               @"bbtbBbb",
            },
            {
               @" 11111 ",
               @" 11111 ",
            },
            {
               @"       ",
               @"       ",
            },
         },
        };

        /// <summary>
        /// Color ANSI string values for the individual sprites
        /// </summary>
        public static Dictionary<char, string>[] colorAliases =
        {
         new Dictionary<char, string>
         {
            { 'r', CGEUtility.GetColorANSIPrefix(232, 68, 46)},
            { 'w', CGEUtility.GetColorANSIPrefix(235, 210, 197)},
            { 'b', CGEUtility.GetColorANSIPrefix(46, 65, 158)}
         },
         new Dictionary<char, string>
         {
            { 'g', CGEUtility.GetColorANSIPrefix(42, 138, 37)},
            { 'd', CGEUtility.GetColorANSIPrefix(18, 82, 15)},
            { 'b', CGEUtility.GetColorANSIPrefix(82, 47, 15)}
         },
         new Dictionary<char, string>
         {
            { 't', CGEUtility.GetColorANSIPrefix(181, 142, 63)},
            { 'b', CGEUtility.GetColorANSIPrefix(135, 102, 36)},
            { 'B', CGEUtility.GetColorANSIPrefix(84, 62, 18)}
         },
      };
    }
}