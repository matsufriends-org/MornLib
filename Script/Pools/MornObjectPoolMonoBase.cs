using MornUtil;
using UnityEngine;

namespace MornLib.Pools
{
    public abstract class MornObjectPoolMonoBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] protected T _prefab;
        private MornObjectPool<T> _mornObjectPool;

        protected virtual int StartCount => 10;

        private void Awake()
        {
            _mornObjectPool = new MornObjectPool<T>(OnGenerate, OnRent, OnReturn, StartCount);
        }

        protected virtual void OnReturn(T x)
        {
            x.gameObject.SetActive(false);
            x.transform.SetParent(transform);
        }

        protected virtual void OnRent(T x)
        {
            x.transform.SetParent(transform);
            x.gameObject.SetActive(true);
        }

        protected virtual T OnGenerate()
        {
            return Instantiate(_prefab, transform);
        }

        public T Rent()
        {
            return _mornObjectPool.Rent();
        }

        public void Return(T poolObject)
        {
            _mornObjectPool.Return(poolObject);
        }
    }
}