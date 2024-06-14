using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.old
{
    public class Entity
    {
        #region Typedefs
        public enum EntityType
        {
            Player = 0,
            PineTree,
            Brush,
            LargePineTree,
            SmallPineTree,
            PineSapling,
        }

        public class EntityZComparer : IComparer<Entity>
        {
            public int Compare(Entity x, Entity y)
            {
                return y.Z - x.Z;
            }
        }
        #endregion

        private CGEUtility.VectorInt2 position;
        public CGEUtility.VectorInt2 Position
        {
            get => position;
            set
            {
                //erase trail, notify others that get dirty
                if (value != position) //don't do anything if the value being assigned is the same value.
                {
                    CGEUtility.VectorInt2 temp = position;
                    position = value;
                    //erase (N/A if not in buffer)
                    if (MyBuffer != null)
                    {
                        Console.Write(CGEUtility.GetColorANSIPrefix(0, 0, 0, false)); //bg (erase) color
                        for (int v = 0; v < Height; v++)
                        {
                            for (int w = 0; w < Width; w++)
                            {
                                Console.SetCursorPosition(temp.Left + MyBuffer.Left, temp.Top + MyBuffer.Top + v);
                                Console.Write(new string(' ', Width));
                            }
                        }
                        Dirty = true;
                    }
                    //TODO: NOTIFY OTHERS THAT GET DIRTY
                }
            }
        }

        public int Width { get => Sprites.sprites[(int)myType][0, 0].Length; }
        public int Height { get => Sprites.sprites[(int)myType].GetLength(1); }
        public int Left
        {
            get => Position.Left;
            set
            {
                if (value != Position.Left)
                {
                    Position = new CGEUtility.VectorInt2(value, Position.Top);
                }
            }
        }
        public int Top
        {
            get => Position.Top;
            set
            {
                if (value != Position.Top)
                {
                    Position = new CGEUtility.VectorInt2(Position.Left, value);
                }
            }
        }
        /// <summary>
        /// The lower the z value, the earlier it is drawn.
        /// </summary>
        public int Z { get; set; }
        /// <summary>
        /// A list of all of the characters that are considered to be "transparent" for drawing purposes.
        /// </summary>
        public bool Dirty { get; set; } = true;
        public ScreenBuffer MyBuffer { get; set; }

        public List<char> transparencyCharacters = new List<char>();
        private EntityType myType;

        public Entity(EntityType type, params char[] transparents)
        {
            myType = type;
            transparencyCharacters.AddRange(transparents);
        }

        public void Draw()
        {
            const bool drawOrigin = false;
            if (Dirty)
            {
                //draw
                //0 visual, 1 color, 2 coll, 3 text
                //origin top left
                Console.Write(CGEUtility.GetColorANSIPrefix(0, 0, 0, false));
                StringBuilder buff = new StringBuilder();
                for (int v = 0; v < Height; v++)
                {
                    char establishedColor = ' ';
                    if (Top + v >= 0 && Top + v < MyBuffer.Height)
                    {
                        int sk = 0;
                        for (int w = 0; w < Width; w++)
                        {
                            //if (Left - (Width - 1) + w < MyBuffer.Left && Left - (Width - 1) + w >= MyBuffer.Left + (MyBuffer.Width - 1))
                            if (Left + w < 0)
                            {
                                sk++;
                                continue;
                            }
                            else if (Left + w >= MyBuffer.Width)
                            {
                                continue;
                            }
#if DEBUG
                            if (drawOrigin)
                            {
                                if (v == 0 && w == 0)
                                {
                                    buff.Append(CGEUtility.GetColorANSIPrefix(255, 20, 20, false) + CGEUtility.GetColorANSIPrefix(20, 255, 255) + 'O' + CGEUtility.GetColorANSIPrefix(0, 0, 0, false));
                                    continue;
                                }
                            }
#endif
                            //TODO determine in the loop if we need to clip because we're outside of the screenbuffer.
                            if (establishedColor == Sprites.sprites[(int)myType][1, v][w])
                            {
                                buff.Append(Sprites.sprites[(int)myType][0, v][w]);
                            }
                            else
                            {
                                //append the ansi code of the new color
                                establishedColor = Sprites.sprites[(int)myType][1, v][w];
                                if (establishedColor != ' ')
                                {
                                    buff.Append(Sprites.colorAliases[(int)myType][establishedColor]);
                                }
                                buff.Append(Sprites.sprites[(int)myType][0, v][w]);
                            }
                        }
                        Console.SetCursorPosition(Left + MyBuffer.Left + sk, Top + MyBuffer.Top + v);
                        Console.Write(buff.ToString());
                        buff.Clear();
                    }
                }
                Dirty = false;
            }
        }
    }
}