using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public class Entity
   {
      /// <summary>
      /// Animation frames will increment every <see cref="AnimationRate"/> seconds. If null, the animation is paused.
      /// </summary>
      public float? AnimationRate { get; set; } = null;
      public int AnimationFrameIndex { get; set; } = 0;
      public int[,] CollisionMask { get; init; } //dont animate this, one frame could be fine, but then another frame's collision data could cause this object to be suddenly clipping in another object.
      Sprite DrawableData { get; init; }
      private Entity() { }
   }
}
