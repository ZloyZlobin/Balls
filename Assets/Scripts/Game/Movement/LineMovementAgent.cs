using UnityEngine;

namespace Balls.Game.Movement
{
   public class LineMovementAgent: MovementAgent
   {
      private readonly Vector3 direction;

      public LineMovementAgent(float speed, Vector3 startPosition, Quaternion rotation): 
         base(speed, startPosition, rotation)
      {
         direction = Rotation * Vector3.left;
      }

      public override Vector3 GetPosition(float time)
      {
         var distance = time * Speed;
         return StartPosition + direction * distance;
      }

      public override MovementType MovementType{ get { return MovementType.Line; } }
   }
}