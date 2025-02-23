using System;
using UnityEngine;

namespace MornSingleton
{
    public abstract class MornSingletonMono<T> : MonoBehaviour where T : MornSingletonMono<T>
    {
        private static T s_instance;
        private bool _didAwake;

        public static T Instance
        {
            get
            {
                if (s_instance != null) return s_instance;

                s_instance = FindFirstObjectByType<T>();
                if (s_instance == null) Debug.LogError($"{typeof(T)}が見つかりません");

                s_instance.OnInstanced();
                return s_instance;
            }
        }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = (T)this;
                DontDestroyOnLoad(gameObject);
                OnInstanced();
            }
            else if (s_instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected abstract void OnInstanced();
    }

    public abstract class MornSingletonMono<TMono, TInterface> : MonoBehaviour
        where TMono : MornSingletonMono<TMono, TInterface>, TInterface, IMornSingleton
    {
        private static TInterface s_instance;

        public static TInterface Instance
        {
            get
            {
                if (s_instance != null) return s_instance;

                var mono = FindFirstObjectByType<TMono>();
                s_instance = mono;
                if (s_instance == null) throw new Exception($"{typeof(TMono)}が見つかりません");

                mono.Instanced();
                return s_instance;
            }
        }
    }
}