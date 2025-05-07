using System;
using MornUtil;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib.Mono.UI
{
    public class MornUIMonoBase : MonoBehaviour, IMornPointerEvent
    {
        [SerializeField] private UIBehaviour _ui;
        [SerializeField] private bool _isOnMouseRight;
        [SerializeField] private bool _isOnMouseMiddle;
        [SerializeField] private bool _isOnMouseLeft = true;
        [SerializeField] private bool _isReactChild;
        private readonly Subject<MouseClickSet> _mouseClickSubject = new();
        private readonly Subject<MouseClickSet> _mouseDownSubject = new();
        private readonly Subject<MouseClickSet> _mouseUpSubject = new();
        private readonly Subject<Unit> _pointerExitSubject = new();
        private bool _isOver;

        private void Awake()
        {
            _ui.OnPointerUpAsObservable().Subscribe(eventData => InvokeSubject(eventData, _mouseUpSubject)).AddTo(this);
            _ui.OnPointerDownAsObservable()
                .Subscribe(eventData => InvokeSubject(eventData, _mouseDownSubject))
                .AddTo(this);
            _ui.OnPointerClickAsObservable()
                .Subscribe(eventData => InvokeSubject(eventData, _mouseClickSubject))
                .AddTo(this);
            OnPointerEnter.Subscribe(_ => _isOver = true).AddTo(this);
            _ui.OnPointerExitAsObservable()
                .Subscribe(_ =>
                {
                    _isOver = false;
                    _pointerExitSubject.OnNext(Unit.Default);
                })
                .AddTo(this);
            gameObject.OnDisableAsObservable()
                .Where(_ => _isOver)
                .Subscribe(_ =>
                {
                    _isOver = false;
                    _pointerExitSubject.OnNext(Unit.Default);
                })
                .AddTo(this);
            var users = GetComponents<IMornUIUserBase>();
            if (users == null) return;

            ((IMornPointerEvent)this).OnPointerEnter.Subscribe(_ =>
                {
                    foreach (var user in users) user.OnSelect();
                })
                .AddTo(this);
            ((IMornPointerEvent)this).OnPointerExit.Subscribe(_ =>
                {
                    foreach (var user in users) user.OnDeSelect();
                })
                .AddTo(this);
            ((IMornPointerEvent)this).OnPointerClick.Subscribe(_ =>
                {
                    foreach (var user in users) user.OnClick();
                })
                .AddTo(this);
        }

        private void Reset()
        {
            _ui = GetComponent<UIBehaviour>();
        }

        public IObservable<Unit> OnPointerEnter => _ui.OnPointerEnterAsObservable()
            .Where(x => _isReactChild || x.pointerEnter == gameObject)
            .Select(_ => Unit.Default);

        public IObservable<Unit> OnPointerExit => _pointerExitSubject;
        public IObservable<MouseClickSet> OnPointerUp => _mouseUpSubject;
        public IObservable<MouseClickSet> OnPointerDown => _mouseDownSubject;
        public IObservable<MouseClickSet> OnPointerClick => _mouseClickSubject;

        private void InvokeSubject(PointerEventData eventData, IObserver<MouseClickSet> subject)
        {
            var isRight = _isOnMouseRight && eventData.IsRightClick();
            var isMiddle = _isOnMouseMiddle && eventData.IsMiddleClick();
            var isLeft = _isOnMouseLeft && eventData.IsLeftClick();
            if (isRight || isMiddle || isLeft) subject.OnNext(new MouseClickSet(isRight, isMiddle, isLeft));
        }
    }
}