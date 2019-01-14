using UnityEngine;

namespace Balls.Game.Movement
{
   public class SinMovementAgent: MovementAgent
   {
      private readonly float frequency;
      private readonly float amplitude;

      public SinMovementAgent(float speed, Vector3 startPosition, Quaternion rotation, float frequency = 10f, float amplitude = 0.1f): 
         base(speed, startPosition, rotation)
      {
         this.frequency = frequency;
         this.amplitude = amplitude;
      }

      public override Vector3 GetPosition(float time)
      {
          var distance = time * Speed;
          Vector3 vSin = new Vector3(0, amplitude * Mathf.Sin(distance * frequency), 0);
          return StartPosition + Rotation * (Vector3.left * distance + vSin);
      }

      public override MovementType MovementType{ get { return MovementType.Sinus; } }
   }
}