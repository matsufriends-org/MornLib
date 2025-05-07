using System.Collections.Generic;
using MornUtil;
using UnityEngine;

namespace MornLib.Mono.TcgLayout
{
    public class MornTcgLayoutMono : MonoBehaviour
    {
        [SerializeField] private RectTransform _ownRect;
        [SerializeField] private int _spacing;
        [SerializeField] private int _angle;
        [SerializeField] [Range(0, 1)] private float _circleRatio;
        [SerializeField] private Vector3 _focusScale;
        [SerializeField] private float _transitionK;
        private readonly List<IMornTcgRectController> _rectList = new();
        private IMornTcgRectController _curRect;
        private IMornTcgRectController _dragRect;

        private void Update()
        {
            if (_dragRect == null)
                NoDragUpdate();
            else
                DragUpdate();

            ContentUpdate();
        }

        private void NoDragUpdate()
        {
            if (Input.GetMouseButtonDown(0) && _curRect != null)
            {
                _dragRect = _curRect;
                _dragRect.Clicked();
            }
        }

        private void DragUpdate()
        {
            if (Input.GetMouseButton(0) == false)
            {
                _dragRect.Deselect();
                _dragRect = null;
                _curRect?.Select();
            }
        }

        private void ContentUpdate()
        {
            var transition = _transitionK * Time.deltaTime;
            var rectCount = _rectList.Count;
            if (rectCount == 0) return;

            var focusIndex = _dragRect?.Index ?? _curRect?.Index ?? -1;
            var leftWidthSum = 0f;
            var leftCount = 0;
            var focusWidth = 0f;
            var rightWidthSum = 0f;
            var rightCount = 0;
            {
                //フォーカスより左側と右側の横幅をそれぞれ求める．
                for (var i = 0; i < rectCount; i++)
                {
                    var rect = _rectList[i];
                    if (i < focusIndex)
                    {
                        leftWidthSum += rect.Size.x /* * rect.Scale.x*/ + _spacing;
                        leftCount++;
                    }
                    else if (i == focusIndex)
                    {
                        focusWidth = rect.Size.x /* * rect.Scale.x*/;
                    }
                    else
                    {
                        rightWidthSum += rect.Size.x /* * rect.Scale.x */ + _spacing;
                        rightCount++;
                    }
                }
            }
            var widthSum = leftWidthSum + focusWidth + rightWidthSum;
            {
                //横幅が超えていた場合用に，縮める係数を求める
                var rectWidth = _ownRect.sizeDelta.x;
                if (widthSum > rectWidth)
                {
                    var over = widthSum - rectWidth;
                    var shrinkK = 1 - over / (leftWidthSum + rightWidthSum);
                    widthSum = rectWidth;
                    leftWidthSum *= shrinkK;
                    focusWidth *= shrinkK;
                    rightWidthSum *= shrinkK;
                }
            }
            {
                var radius = _angle == 0 ? 0 : widthSum / 2f / Mathf.Sin(_angle * Mathf.Deg2Rad);
                for (var i = 0; i < rectCount; i++)
                {
                    float aimPosX;
                    if (i < focusIndex) //左側の時
                        aimPosX = -widthSum / 2f + (leftWidthSum - focusWidth / 2f) / leftCount * (i + 0.5f);
                    else if (i == focusIndex) //フォーカス中のやつ
                        aimPosX = -widthSum / 2f + widthSum / rectCount * (i + 0.5f);
                    else //右側のやつ
                        aimPosX = widthSum / 2f +
                                  (rightWidthSum - focusWidth / 2f) / rightCount * (i - rectCount + 0.5f);

                    var rect = _rectList[i];
                    var aimScale = i == focusIndex ? _focusScale : Vector3.one;
                    var xRatio = aimPosX / (widthSum / 2f); //-1 ~ 1
                    var aimAngle = i == focusIndex ? 0 : -_angle * xRatio;
                    var rad = _angle * xRatio * Mathf.Deg2Rad;
                    var rotatePos = new Vector2(aimPosX, (Mathf.Cos(rad) - 1) * radius * _circleRatio);
                    var constantPos = new Vector2(aimPosX, -_ownRect.sizeDelta.y / 2f + rect.Size.y * aimScale.y / 2f);
                    rect.SetUpdate(i == focusIndex ? constantPos : rotatePos, Quaternion.Euler(0, 0, aimAngle),
                        aimScale, transition);
                }
            }
        }

        public void AddItem(IMornTcgRectController mornTcgRect)
        {
            _rectList.Add(mornTcgRect);
            mornTcgRect.SetIndex(_rectList.Count - 1);
            mornTcgRect.Init(() =>
            {
                //Selected
                _curRect = mornTcgRect;
                if (_dragRect == null) _curRect.Select();
            }, () =>
            {
                //Deselected
                if (_curRect == mornTcgRect) _curRect = null;

                if (_dragRect == null) mornTcgRect.Deselect();
            });
        }

        public void RemoveItem(IMornTcgRectController mornTcgRect)
        {
            if (mornTcgRect.Index < 0 || _rectList.Count <= mornTcgRect.Index) Debug.LogError("不正なIndexです");

            _rectList.RemoveAt(mornTcgRect.Index);
            foreach (var rect in _rectList) rect.RemoveIndex(mornTcgRect.Index);

            if (_curRect == mornTcgRect) _curRect = null;

            if (_dragRect == mornTcgRect) _dragRect = null;

            mornTcgRect.ExeDestroy();
        }

        public void DestroyAll()
        {
            _rectList.Clear();
            _curRect = null;
            _dragRect = null;
            transform.DestroyChildren();
        }
    }
}