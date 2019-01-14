using UnityEngine;

namespace Balls.Game.Hit
{
   public delegate void OnHitHandler(GameObject owner, GameObject other);
   public interface IHit
   {
      event OnHitHandler OnHit;
   }
}