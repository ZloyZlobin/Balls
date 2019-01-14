using Balls.Game.Hit;
using Balls.Game.Movement;
using Balls.Game.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balls.Game
{
   public class ObjectSpawner: MonoBehaviour
   {
      public float speed = 5f;
      public float spawnPerSecond = 1f;
      public float spawnIncrease = 0.5f;
      public int numberOfTries = 50;
      public float collisionDistance = 0.1f;
      public GameObject prototype;
      
      private float startSpawnPerSecond;
      private float spawnTimePeriod;
      private float timeFromSpawn;
      private Camera mainCamera;
      private bool spawnFromTop = true;

      private List<GameObject> objects;
      private ObjectPool<MovementType, GameObject> pool;

      private Vector3 originBound;
      private Vector3 extentsBound;

      #region MonoBehaviour methods

      void Awake()
      {
         objects = new List<GameObject>();
         pool = new ObjectPool<MovementType, GameObject>();

         mainCamera = Camera.main;
         startSpawnPerSecond = spawnPerSecond;
         spawnTimePeriod = 1f / spawnPerSecond;

         StatisticSystem.OnReset += Reset;

          //calc object bounds
         if(prototype != null)
         {
            Bounds bounds = prototype.renderer.bounds;
            originBound  = mainCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
            extentsBound = mainCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));
         }
         else
         {
            originBound = Vector3.zero;
            extentsBound = Vector3.one;
         }

      }

      void OnDestroy()
      {
         StatisticSystem.OnReset -= Reset;
      }

      void Update()
      {
          RemoveOverScreenObjects();

          timeFromSpawn += Time.deltaTime;
          if (timeFromSpawn >= spawnTimePeriod)
          {
              while (timeFromSpawn >= spawnTimePeriod)
              {
                  timeFromSpawn -= spawnTimePeriod;
                  Spawn();
              }
              spawnPerSecond += spawnIncrease;
              spawnTimePeriod = 1f / spawnPerSecond;
          }
      }
      
      #endregion

      void Reset()
      {
         foreach(var obj in objects)
         {
            RemoveObject(obj);
         }
         
         objects.Clear();
         pool.Clear();

         spawnPerSecond = startSpawnPerSecond;
      }

      void RemoveObject(GameObject obj)
      {
         var hit = obj.GetComponent(typeof(IHit)) as IHit;
         hit.OnHit -= HandleOnHit;
         var movement = obj.GetComponent<MovementComponent>().MovementAgent;
         pool.CollectObject(movement.MovementType, obj, ClearObject);

         StatisticSystem.InGame--;
      }


      void RemoveOverScreenObjects()
      {
         List<GameObject> objsToRemove = new List<GameObject>();
         var panels = GeometryUtility.CalculateFrustumPlanes(mainCamera);;
         foreach(var obj in objects)
         {
            if(!obj.IsRenderedFrom(panels))
            {
               objsToRemove.Add(obj);
            }
         }
         objects.RemoveAll( obj => !obj.IsRenderedFrom(panels));

         foreach(var objToRemove in objsToRemove)
         {
            RemoveObject(objToRemove);
         }
      }

       /// <summary>
       /// Clear object to reuse
       /// </summary>
       /// <param name="obj">Object to reuse</param>
      void ClearObject(GameObject obj)
      {
         SetColor(obj, Color.white);
         obj.SetActive(false);
      }

      void Spawn()
      { 
         float minX = (extentsBound.x - originBound.x);
         float maxX = Screen.width - minX;
         float y = spawnFromTop ? Screen.height : 0;

         Quaternion rotation = Quaternion.identity;
         Vector3 position = Vector3.zero;
          //Choose random movement type
         var movements = Enum.GetValues(typeof(MovementType));
         MovementType currentMovementType = (MovementType)movements.GetValue(UnityEngine.Random.Range(0,movements.Length));
         MovementAgent movement = null;

         int countTries = numberOfTries;
         bool hasCollision = false;
         do
         {
            countTries--;

            var x = UnityEngine.Random.Range(minX, maxX);
            position = mainCamera.ScreenToWorldPoint(new Vector3(x, y, 5f));
            var screenPoint = mainCamera.WorldToScreenPoint(position);
            rotation = GeometryUtils.FindRotation(screenPoint, minX, maxX, spawnFromTop);
            movement = GetMovementByType(currentMovementType, speed, position, rotation);

            foreach (var otherObj in objects) 
            {
               float time;
               var otherObjMovement = otherObj.GetComponent<MovementComponent>().MovementAgent;
               if(otherObjMovement.MovementType == MovementType.Line && currentMovementType == MovementType.Line)
               {
                  time = GeometryUtils.PredictLineCollision(movement, otherObjMovement, collisionDistance);
               }
               else
               {
                  time = GeometryUtils.PredictCollisionBySimulation(movement, otherObjMovement, collisionDistance, Time.fixedDeltaTime);
               }

               if(time >= 0f)
               {
                  hasCollision = true;
                  break;
               }
            }
         }while(hasCollision && countTries > 0);

         if(!hasCollision)
         {
            var obj = InitObject(movement, position);
            objects.Add(obj);

            StatisticSystem.InGame++;
            StatisticSystem.Started++;
         }
         else
         {
            StatisticSystem.Canceled++;
         }
         spawnFromTop = !spawnFromTop;//switch side
      }

       /// <summary>
       /// Initialize object from pool or create new
       /// </summary>
       /// <param name="movement">Movement type</param>
       /// <param name="position">Start position</param>
       /// <returns></returns>
      GameObject InitObject(MovementAgent movement, Vector3 position)
      {
         var obj = pool.GetObject(movement.MovementType, PrepareObj, CreateObject);
         obj.transform.position = position;
         obj.GetComponent<MovementComponent>().MovementAgent = movement;
         var hit = obj.GetComponent<SphereHit>();
         hit.OnHit += HandleOnHit;

         return obj;
      }
       /// <summary>
       /// Prepare object before reuse
       /// </summary>
       /// <param name="obj"></param>
      void PrepareObj(GameObject obj)
      {
         obj.SetActive(true);
      }
       /// <summary>
       /// Create new object and add logic
       /// </summary>
       /// <returns>New object</returns>
      GameObject CreateObject()
      {
         GameObject obj;
         if(prototype == null)
            obj = new GameObject();
         else
            obj = GameObject.Instantiate(prototype) as GameObject;

         obj.AddComponent<SphereHit>();
         obj.AddComponent<MovementComponent>();
         obj.transform.parent = transform;

         return obj;
      }

      MovementAgent GetMovementByType(MovementType type, float speed, Vector3 position, Quaternion rotation)
      {
         switch(type)
         {
            case MovementType.Line: return new LineMovementAgent(speed, position, rotation);
            case MovementType.Sinus : return new SinMovementAgent(speed, position, rotation);
            //case MovementType.Circle : return new CircleMovementAgent(speed, position, rotation);
            default : return new LineMovementAgent(speed, position, rotation);
         }
      }

      void HandleOnHit (GameObject owner, GameObject other)
      {
         StatisticSystem.Collisions++;

         SetColor(owner, Color.red);
         SetColor(other, Color.red);
      }

      void SetColor(GameObject obj, Color color)
      {
         if(obj.renderer == null)
            return;

         obj.renderer.material.color = color;
      }
   }
}