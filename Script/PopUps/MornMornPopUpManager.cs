using System;
using MornSingleton;
using UnityEngine;

namespace MornLib.PopUps
{
    public class MornMornPopUpManager : MornSingleton<MornMornPopUpManager, IMornPopUpManager>, IMornPopUpManager,
        IMornSingleton
    {
        private float _cachedLastShowTime;
        private bool _initialized;
        private RectTransform _popRect;
        private Action<IMornPopUpCaller> _setText;

        void IMornPopUpManager.Init(RectTransform rect, Action<IMornPopUpCaller> setText)
        {
            _initialized = true;
            _popRect = rect;
            _setText = setText;
        }

        void IMornPopUpManager.Show(IMornPopUpCaller mornPopUpCaller)
        {
            if (_initialized == false)
            {
                Debug.LogWarning("PopUpはまだセットアップされていません");
                return;
            }

            _cachedLastShowTime = Time.unscaledTime;
            if (_popRect) _popRect.gameObject.SetActive(true);

            _setText(mornPopUpCaller);
        }

        void IMornPopUpManager.Hide()
        {
            if (_initialized == false)
            {
                Debug.LogWarning("PopUpはまだセットアップされていません");
                return;
            }

            if (Time.unscaledTime - _cachedLastShowTime == 0) return;

            if (_popRect) _popRect.gameObject.SetActive(false);
        }

        public void Instanced()
        {
        }
    }
}