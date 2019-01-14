using UnityEngine;

namespace Balls.Game.Hit
{
   [RequireComponent(typeof(SphereCollider))]
   public class SphereHit: MonoBehaviour, IHit
   {
      private OnHitHandler onHit;

      #region IHit implementation

      public event OnHitHandler OnHit
      {
         add
         {
            onHit += value;
         }
         remove
         {
            onHit -= value;
         }
      }

      #endregion

      void OnCollisionEnter(Collision collision)
      {
         if(onHit != null)
            onHit(gameObject, collision.gameObject);
      }
   }
}