using Balls.Game.Movement;
using UnityEngine;

namespace Balls.Game.Utils
{
   public static class GeometryUtils
   {
      public static bool IsRenderedFrom(this GameObject obj, Camera camera)
      {
         if(camera == null)
            return false;

         return obj.IsRenderedFrom(GeometryUtility.CalculateFrustumPlanes(camera));
      }
      public static bool IsRenderedFrom(this GameObject obj, Plane[] planes)
      {
         if(obj.renderer == null || planes == null)
            return false;
         return GeometryUtility.TestPlanesAABB(planes, obj.renderer.bounds);
      }

      public static float PredictCollisionBySimulation(MovementAgent aMovement, MovementAgent bMovement, float minDistance, float timeStep)
      {
         if(aMovement == null || bMovement == null)
            return -1f;

         float time = 0f;
         Vector3 posA;
         Vector3 posB;
         var upperBound = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)) + Vector3.up;
         var downBound = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)) + Vector3.down;
         do
         {
            posA = aMovement.GetPosition(aMovement.CurrentTime + time);
            posB = bMovement.GetPosition(bMovement.CurrentTime + time);

            if(Mathf.Abs(Vector3.Distance(posA, posB)) <= minDistance)
               return time;
            
            time += timeStep;
         }while(posA.y > downBound.y && posA.y < upperBound.y && posB.y > downBound.y && posB.y < upperBound.y);

         return -1f;
      }

      public static float PredictLineCollision(Vector3 aPos, Quaternion aRotation, float aSpeed, Vector3 bPos, Quaternion bRotation, float bSpeed, float minDistance)
      {
         var diplacement = aPos - bPos;
         var velocity = aRotation * Vector3.left * aSpeed - bRotation * Vector3.left * bSpeed;
         var a = diplacement.x;
         var b = velocity.x;
         var x = diplacement.y;
         var y = velocity.y;
         float t1, t2;
         if(!SolveQuadratic(b*b + y*y, 2*(a*b + x*y), a*a + x*x - minDistance*minDistance, out t1, out t2))
            return -1;
         var time = t1;
         if(t2 < t1 && t2 >= 0)
            time = t2;
         return time;
      }

      public static float PredictLineCollision(MovementAgent aMovement, MovementAgent bMovement, float minDistance)
      {
         if(aMovement == null || bMovement == null)
            return -1;

         return PredictLineCollision(aMovement.CurrentPosition,
                                     aMovement.Rotation,
                                     aMovement.Speed,
                                     bMovement.CurrentPosition,
                                     bMovement.Rotation,
                                     bMovement.Speed,
                                     minDistance);
      }
      
      public static bool SolveQuadratic(float a, float b, float c, out float t1, out float t2)
      {
         t1 = -1;
         t2 = -1;
         float sqrtpart = b * b - 4 * a * c;
         float x, x1, x2, img;
         if (sqrtpart > 0)
         {
            x1 = (float)(-b + System.Math.Sqrt(sqrtpart)) / (2 * a);
            x2 = (float)(-b - System.Math.Sqrt(sqrtpart)) / (2 * a);
            t1 = x1;
            t2 = x2;
         }
         else if (sqrtpart < 0)
         {
            sqrtpart = -sqrtpart;
            x = -b / (2 * a);
            img = (float)System.Math.Sqrt(sqrtpart) / (2 * a);

            return false;
         }
         else
         {
            x = (float)(-b + System.Math.Sqrt(sqrtpart)) / (2 * a);
            t1 = x;
         }
         return true;
      }

      public static  Quaternion FindRotation(Vector3 screenSpawnPoint, float minX, float maxX, bool spawnFromTop)
      {
         float minAngle;
         float maxAngle;

         if(!spawnFromTop)
         {
            minAngle = Mathf.Atan2(screenSpawnPoint.y - Screen.height, screenSpawnPoint.x - minX) * Mathf.Rad2Deg;
            maxAngle = Mathf.Atan2(screenSpawnPoint.y - Screen.height,  screenSpawnPoint.x - maxX) * Mathf.Rad2Deg;
         }
         else
         {
            minAngle = Mathf.Atan2(screenSpawnPoint.y, screenSpawnPoint.x - minX) * Mathf.Rad2Deg;
            maxAngle = Mathf.Atan2(screenSpawnPoint.y, screenSpawnPoint.x - maxX) * Mathf.Rad2Deg;
         }
         float angle = UnityEngine.Random.Range(minAngle, maxAngle);

         return Quaternion.Euler(0, 0, angle);
      }
   }
}