using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Major
{
    public class ShowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Transform icon;
        [SerializeField] private Transform close;
        [SerializeField] private Image openImage;
        [SerializeField] private Image closeImage;
        
        [Header("Idle Animation")]
        [SerializeField] private float levitationDuration;
        [SerializeField] private float levitationStrength;
        [SerializeField] private int vibrato;
        
        [Header("Wheel Animation")]
        [SerializeField] private float wheelDuration;
        [SerializeField] private Ease wheelEase;
        [SerializeField] private Ease wheelEaseCloseButton;
        [Space]
        [SerializeField] private float wheelScaleDuration;
        [SerializeField] private Ease wheelScaleEase;
        [SerializeField] private Ease wheelEaseScaleCloseButton;
        [Space]
        [SerializeField] private float wheelMoveDuration;
        [SerializeField] private Ease wheelMoveEase;
        [SerializeField] private Ease wheelEaseMoveCloseButton;
        [SerializeField] private float wheelMoveHorizontalPosition;
        [SerializeField] private float wheelMoveHorizontalPositionDelay;
        [Space(30)]
        [Header("Close Animation")]
        [SerializeField] private float closeDuration;
        [SerializeField] private Ease closeEase;
        [SerializeField] private Ease closeEaseIconButton;
        [Space]
        [SerializeField] private float closeScaleDuration;
        [SerializeField] private Ease closeScaleEase;
        [SerializeField] private Ease closeEaseScaleIconButton;
        [Space]
        [SerializeField] private float closeMoveDuration;
        [SerializeField] private Ease closeMoveEase;
        [SerializeField] private Ease closeEaseMoveIconButton;
        [SerializeField] private float closeMoveHorizontalPosition;
        [SerializeField] private float closeMoveHorizontalPositionDelay;
        [Space]
        [SerializeField] private Ease closeNoMessageEaseMoveIconButton;
        [SerializeField] private float closeNoMessageMoveHorizontalPosition;
        
        [Space(30)]
        [Header("Close Selection Animation")]
        [SerializeField] private float readyCloseStateDuration;
        [SerializeField] private float readyCloseStateScale;
        [SerializeField] private Ease readyCloseStateEase;
        [Space]
        [SerializeField] private float readySelectMessageStateDuration;
        [SerializeField] private Ease readySelectMessageStateEase;

        [Space] [Header("Reaction")] 
        [SerializeField] private float showWheelInvokeDelay;
        [SerializeField] private UnityEvent showWheel;
        [Space]
        [SerializeField] private float hideWheelInvokeDelay;
        [SerializeField] private UnityEvent hideWheelInstant;

        private Coroutine _showWheelRoutine;
        private Coroutine _hideWheelRoutine;

        private bool _isCloseWithoutMessage;

        private float _initialCloseHorizontalPosition;

        private void Start()
        {
            _initialCloseHorizontalPosition = close.localPosition.x;
            LevitationAnimation();
        }
        
        private void LevitationAnimation() => 
            openImage.transform.DOShakePosition(levitationDuration, levitationStrength, randomness: 180, vibrato: vibrato, fadeOut: false).OnComplete(LevitationAnimation);

        private void GoWheelState()
        {
            StopHideInvoke();
            KillTwins();
            
            _isCloseWithoutMessage = false;
                
            openImage.DOFade(0, wheelDuration).SetEase(wheelEase);
            closeImage.DOFade(0.94f, wheelDuration).SetEase(wheelEaseCloseButton).OnComplete(StateChanged);
            
            icon.DOLocalMoveX(wheelMoveHorizontalPosition, wheelMoveDuration).SetEase(wheelMoveEase);
            
            var halfPosition = (_initialCloseHorizontalPosition + wheelMoveHorizontalPosition) / 2f;
            close.localPosition = new Vector3(halfPosition, close.localPosition.y, close.localPosition.z);
            close.DOLocalMoveX(wheelMoveHorizontalPosition, wheelMoveDuration).SetEase(wheelEaseMoveCloseButton).SetDelay(wheelMoveHorizontalPositionDelay);
            
            icon.DOScale(0, wheelScaleDuration).SetEase(wheelScaleEase);
            close.DOScale(1, wheelScaleDuration).SetEase(wheelEaseScaleCloseButton);

            StopWheelInvoke();
            _showWheelRoutine = StartCoroutine(ShowWheel());
            return;

            void StateChanged()
            {
                closeImage.raycastTarget = true;
            }
        }

        private void StopWheelInvoke()
        {
            if (_showWheelRoutine != null) 
                StopCoroutine(_showWheelRoutine);
        }
        
        private void StopHideInvoke()
        {
            if (_hideWheelRoutine != null) 
                StopCoroutine(_hideWheelRoutine);
        }

        private IEnumerator ShowWheel()
        {
            yield return new WaitForSeconds(showWheelInvokeDelay);
            showWheel?.Invoke();
        }
        
        private IEnumerator GoButtonState()
        {
            var showCloseMove = _isCloseWithoutMessage;
            StopWheelInvoke();
            yield return new WaitForSeconds(hideWheelInvokeDelay);
            
            KillTwins();

            closeImage.raycastTarget = false;
                
            openImage.DOFade(1, closeDuration).SetEase(closeEaseIconButton);
            
            if(showCloseMove)
                closeImage.DOFade(0, closeDuration).SetEase(closeEase);
            
            var halfPosition = (icon.localPosition.x + closeMoveHorizontalPosition) / 2f;
            icon.localPosition = new Vector3(halfPosition, openImage.transform.localPosition.y, openImage.transform.localPosition.z);
            icon.DOLocalMoveX(closeMoveHorizontalPosition, closeMoveDuration).SetEase(closeEaseMoveIconButton).SetDelay(closeMoveHorizontalPositionDelay);
            
            if(showCloseMove)
                close.DOLocalMoveX(closeMoveHorizontalPosition, closeMoveDuration).SetEase(closeMoveEase);
            
            icon.DOScale(1, closeScaleDuration).SetEase(closeEaseScaleIconButton);
            close.DOScale(0, closeScaleDuration).SetEase(closeScaleEase);
        }

        public void ReadyCloseState()
        {
            closeImage.transform.DOKill();
            _isCloseWithoutMessage = true;
            closeImage.transform.DOScale(Vector3.one * readyCloseStateScale,readyCloseStateDuration ).SetEase(readyCloseStateEase);
        }


        public void ReadySelectMessageState()
        {
            closeImage.transform.DOKill();
            _isCloseWithoutMessage = false;
            closeImage.transform.DOScale(Vector3.one,readySelectMessageStateDuration ).SetEase(readySelectMessageStateEase);
        }
          

        private void KillTwins()
        {
            openImage.DOKill();
            closeImage.DOKill();
            icon.DOKill();

        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            GoWheelState();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            hideWheelInstant?.Invoke();
            StopHideInvoke();
            _hideWheelRoutine = StartCoroutine(GoButtonState());

            if (_isCloseWithoutMessage) return;
            
            closeImage.DOFade(0, closeDuration/2f).SetEase(closeEase);
            close.DOScale(Vector3.zero, closeDuration/2f).SetEase(closeEase);
            close.DOLocalMoveX(closeNoMessageMoveHorizontalPosition, closeDuration/2f).SetEase(closeNoMessageEaseMoveIconButton);
        }
    }
}
