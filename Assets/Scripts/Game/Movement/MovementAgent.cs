using UnityEngine;

namespace Balls.Game.Movement
{
   public abstract class MovementAgent
   {
      protected MovementAgent(float speed, Vector3 startPosition, Quaternion rotation)
      {
         CurrentTime = 0f;
         Speed = speed;
         Rotation = rotation;
         StartPosition = startPosition;

         Update(0);
      }

      public void Update(float dTime)
      {
         CurrentTime += dTime;
         CurrentPosition = GetPosition(CurrentTime);
      }

      public Vector3 StartPosition { get; private set; }

      public float Speed { get; private set; }
      
      public Vector3 CurrentPosition { get; private set; } 
      
      public Quaternion Rotation { get; private set; }
      
      public float CurrentTime { get; private set; }

      public abstract MovementType MovementType { get; }
      
      public abstract Vector3 GetPosition(float time);
   }
}

