using UnityEngine;

namespace Balls.Game.Movement
{
   public class CircleMovementAgent: MovementAgent
   {
      private readonly float radius;

      public CircleMovementAgent(float speed, Vector3 startPosition, Quaternion rotation, float radius = 6f): 
         base(speed, startPosition, rotation)
      {
         this.radius = radius;
      }

      public override Vector3 GetPosition(float time)
      {
         var distance = time * Speed;
         return StartPosition + Rotation * new Vector3(Mathf.Cos(distance), Mathf.Sin(distance), 0) * radius;
      }
      
      public override MovementType MovementType{ get { return MovementType.Circle; } }
   }
}