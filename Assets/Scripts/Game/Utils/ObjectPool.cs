using System;
using System.Collections.Generic;

namespace Balls.Game.Utils
{
   public class ObjectPool<TKey, TObj>
   {
      private readonly uint capacity;
      private Dictionary<TKey, Queue<TObj>> objects;

      public ObjectPool() : this(10)
      {
      }
      public ObjectPool(uint capacity)
      {
         this.capacity = capacity;
         objects = new Dictionary<TKey, Queue<TObj>>();
      }
      public TObj GetObject(TKey key, Action<TObj> prepareAction, Func<TObj> createFunc)
      {
         var obj = default(TObj);
         Queue<TObj> objQueue;
         if(objects.TryGetValue(key, out objQueue) && objQueue.Count > 0)
         {
            obj = objQueue.Dequeue();
         }
         else if(createFunc != null)
            obj = createFunc();

         if(prepareAction != null)
            prepareAction(obj);

         return obj;
      }

      public void CollectObject(TKey key, TObj obj, Action<TObj> clearAction)
      {
         Queue<TObj> objQueue;
         if(!objects.TryGetValue(key, out objQueue))
         {
            objQueue = new Queue<TObj>();
            objects.Add(key, objQueue);
         }

         if(objQueue.Count < capacity)
            objQueue.Enqueue(obj);

         if(clearAction != null)
            clearAction(obj);
      }

      public void Clear()
      {
         objects.Clear();
      }
   }
}

