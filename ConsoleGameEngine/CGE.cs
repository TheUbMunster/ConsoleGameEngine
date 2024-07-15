using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public static class CGE
   {
      public static void Initialize() { } //force static ctor
      static CGE()
      {
         ConsoleUtil.Initialize();
      }

      public static Renderer Renderer { get; } = new Renderer();

      public static class Animation
      {

      }
      public static class Physics
      {
         public static event Action<Entity, Entity> OnCollision;
         private static bool[,] physicsLayersInteractions = null;
         /// <summary>
         /// For indeces i and j, whenever an entity of physics layer i collides with an entity of physics layer j, the
         /// "OnCollision" Event will be raised if physicsLayersInteractions[i, j] is true. Collisions can only occur between
         /// objects within the same ConsoleWindow.
         /// 
         /// Within the "OnCollision" event, you can check e.g., if either entity is the "player", and if so, roll back their
         /// movement so they're no longer colliding with what they hit.
         /// 
         /// Warning: if collision is enabled for [i, j] and [j, i] individual events will be raised for object i colliding with object j
         /// and vice versa. Order of these OnCollision events determined directly by "what object was moved that caused the collision".
         /// </summary>
         public static void SetPhysicsLayers(bool[,] layers)
         {
            if (physicsLayersInteractions == null)
               physicsLayersInteractions = layers;
            else
               throw new Exception("You've already assigned the physics layers");
         }

         internal static void RaiseMoveEvent(Entity entity) 
         {
            if (entity.ParentWindow != null && entity.CollisionMask != null)
               foreach (Entity e in entity.ParentWindow.Entities)
               {
                  bool wasCollision = false;
                  if (e != entity && e.CollisionMask != null)
                  {
                     for (int ogx = 0; ogx < entity.CollisionMask.GetLength(0); ogx++)
                        for (int ogy = 0; ogy < entity.CollisionMask.GetLength(1); ogy++)
                           for (int nx = 0; nx < e.CollisionMask.GetLength(0); nx++)
                              for (int ny = 0; ny < e.CollisionMask.GetLength(1); ny++)
                              {
                                 int wogx = ogx - e.Left, wogy = ogy - e.Top;
                                 if (wogx >= 0 && wogy >= 0 && 
                                    wogx < entity.CollisionMask.GetLength(0) && wogy < entity.CollisionMask.GetLength(1))
                                 {
                                    if (physicsLayersInteractions[entity.CollisionMask[wogx, wogy], e.CollisionMask[nx, ny]])
                                       wasCollision |= true; //if we wanted which coordinates collided, we could put that here.
                                 }
                              }
                  }
                  if (wasCollision)
                     OnCollision?.Invoke(entity, e);
               }
         }
      }
   }
}
