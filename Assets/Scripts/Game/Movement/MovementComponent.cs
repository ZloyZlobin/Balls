using UnityEngine;

namespace Balls.Game.Movement
{
   public class MovementComponent : MonoBehaviour
   {
      private Transform trns;

      public MovementAgent MovementAgent;

      void Awake()
      {
         trns = transform;
      }

      void FixedUpdate()
      {
         if(MovementAgent != null)
         {
            MovementAgent.Update(Time.fixedDeltaTime);
            trns.position = MovementAgent.CurrentPosition;
         }
      }
   }
}