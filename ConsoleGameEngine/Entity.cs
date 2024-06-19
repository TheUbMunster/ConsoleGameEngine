using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleGameEngine.ConsoleWindow;

namespace ConsoleGameEngine
{
   public class Entity
   {
      /// <summary>
      /// For indeces i and j, whenever an entity of physics layer i collides with an entity of physics layer j, the
      /// "OnCollision" Event will be raised if physicsLayersInteractions[i, j] is true.
      /// 
      /// Within the "OnCollision" event, you can check e.g., if either entity is the "player", and if so, roll back their
      /// movement so they're no longer colliding with what they hit.
      /// </summary>
      public static bool[,] physicsLayersInteractions = new bool[13, 13]; //n x n
      public static event Action<Entity, Entity> OnCollision;




      private long ctorTime = DateTime.Now.Ticks;
      private int animationFrameOffset = 0;
      private float? animationRate = null;
      /// <summary>
      /// Animation frames will increment every <see cref="AnimationRate"/> seconds. If null, the animation is paused.
      /// </summary>
      public float? AnimationRate 
      {
         get => animationRate;
         set
         {
            //if (value == null && animationRate != null)
            //{
            //   animationRate = value;
            //   ParentWindow.SubmemberAnimatedCount--;
            //}
            //else if (value != null && animationRate == null)
            //{
            //   animationRate = value;
            //   ParentWindow.SubmemberAnimatedCount++;
            //}
            //else if (value != null && animationRate != null && animationRate != value)
               animationRate = value; //this is already in submemberanimatedcount
         }
      }
      //private int animationFrameIndex = 0;
      public int AnimationFrameIndex //when changed via time or manually set parent window dirty
      {
         get
         {
            int timeVal = 0;
            if (AnimationRate.HasValue)
            {
               if (AnimationRate != null)
               {
                  timeVal = (int)(((DateTime.Now.Ticks - ctorTime) / TimeSpan.TicksPerSecond) / AnimationRate.Value);
               }
            }
            return (timeVal + animationFrameOffset) % BackingSprite.Frames;
         }
         set
         {
            int cur = AnimationFrameIndex;
            animationFrameOffset = Math.Abs(cur - value);
            //animationFrameIndex = (value % BackingSprite.Chars.Count);
         }
      }
      /// <summary>
      /// Note that not all of the collision mask must belong to the same physics layer. Some of it may belong
      /// to a solid collision layer, while another portion of it may be a "trigger" physics layer to (e.g., open a door).
      /// </summary>
      public int[,] CollisionMask { get; init; } //dont animate this, one frame could be fine, but then another frame's collision data could cause this object to be suddenly clipping in another object.
      public Sprite BackingSprite { get; init; }
      private int left;
      /// <summary>
      /// How many columns to the right is this window relative to the left edge of the parent window.
      /// </summary>
      public int Left 
      {
         get => left;
         set
         {
            if (left != value)
            {
               if (ParentWindow != null && (ParentWindow.DrawType & WindowDrawType.EntityMode) != WindowDrawType.Disabled)
                  ParentWindow.IsDirty |= true;
               left = value;
            }
         }
      }
      private int top;
      /// <summary>
      /// How many rows to the bottom is this window relative to the top edge of the parent window.
      /// </summary>
      public int Top 
      {
         get => top;
         set
         {
            if (top != value)
            {
               if (ParentWindow != null && (ParentWindow.DrawType & WindowDrawType.EntityMode) != WindowDrawType.Disabled)
                  ParentWindow.IsDirty |= true;
               top = value;
            }
         }
      }
      private int zOrder;
      /// <summary>
      /// What order this drawable element is drawn in. High values get drawn on top, low values on bottom.
      /// </summary>
      public int ZOrder 
      {
         get => zOrder;
         set
         {
            if (zOrder != value)
            {
               if (ParentWindow != null && (ParentWindow.DrawType & WindowDrawType.EntityMode) != WindowDrawType.Disabled)
                  ParentWindow.IsDirty |= true;
               zOrder = value;
            }
         }
      }
      public ConsoleWindow ParentWindow { get; internal set; }
      public Entity() { } //todo: ctor verify drawabledata matches collisionmask dimension
   }
}
