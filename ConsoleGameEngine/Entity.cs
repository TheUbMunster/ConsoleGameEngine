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
      /// 
      /// What layer is considered to be collision/trigger/whatever is up to the implementer.
      /// 
      /// Negative entries in the CollisionMask are considered to be the "lack of collision".
      /// Values within the dimensions of <see cref="CGE.Physics.physicsLayersInteractions"/> correlate to those physics layers.
      /// 
      /// Instead of using numbers, it's useful to use an enum type to represent your physics layers.
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
               CGE.Physics.RaiseMoveEvent(this);
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
               CGE.Physics.RaiseMoveEvent(this);
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
