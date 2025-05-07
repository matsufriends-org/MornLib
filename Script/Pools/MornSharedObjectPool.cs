using System.Collections.Generic;
using MornLib.Cores;
using UnityEngine;

namespace MornLib.Pools
{
    public static class MornSharedObjectPool<T> where T : IMornPoolItem, new()
    {
        private static readonly Queue<T> s_itemQueue = new();
        private static readonly HashSet<T> s_rentingInstanceHashSet = new();

        public static T Rent()
        {
            var result = s_itemQueue.Count > 0 ? s_itemQueue.Dequeue() : new T();
            s_rentingInstanceHashSet.Add(result);
            result.Clear();
            return result;
        }

        public static void Return(T item)
        {
            if (s_rentingInstanceHashSet.Remove(item) == false)
                Debug.LogWarning("Pool管理外のInstanceが渡された");
            else
                s_itemQueue.Enqueue(item);
        }
    }
}