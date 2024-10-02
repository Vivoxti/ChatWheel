using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Major
{
    public class WheelAnimator : MonoBehaviour
    {
        [SerializeField] private Image rotator;
        [SerializeField] private Image palette;
        [Space]
        [SerializeField] private Transform rotatorTransform;
        [SerializeField] private Transform paletteTransform;
        
        [Header("Show Animation")]
        [SerializeField] private float showDuration;
        [SerializeField] private Ease showRotatorEase;
        [SerializeField] private Ease showRotatorScaleEase;
        [SerializeField] private Ease showRotatorRotateEase;
        [Space]
        [SerializeField] private float showPaletteDelay;
        [SerializeField] private float showPaletteDuration;
        [SerializeField] private Ease showPaletteRotatorEase;
        [SerializeField] private Ease showPaletteRotatorScaleEase;
        [SerializeField] private Ease showPaletteRotatorRotateEase;
        
        [Header("Hide Animation")]
        [SerializeField] private float hideDuration;
        [SerializeField] private Ease hideRotatorEase;
        [SerializeField] private Ease hideRotatorScaleEase;
        [SerializeField] private Ease hideRotatorRotateEase;
        [Space]
        [SerializeField] private float hidePaletteDuration;
        [SerializeField] private Ease hidePaletteRotatorEase;
        [SerializeField] private float hidePaletteScale;
        [SerializeField] private float hidePaletteTransformsDuration;
        [SerializeField] private Ease hidePaletteRotatorScaleEase;
        [SerializeField] private Ease hidePaletteRotatorRotateEase;
        
        [Space(30)]
        [Header("Close Selection Animation")]
        [SerializeField] private float readyCloseStateDuration;
        [SerializeField] private float readyCloseStateScale;
        [SerializeField] private Ease readyCloseStateEase;
        [Space]
        [SerializeField] private float readySelectMessageStateDuration;
        [SerializeField] private Ease readySelectMessageStateEase;

        private const float SpeedCoef = 0.45f;
        private const float Speed = 0.085f;

        private Vector3 _initialScale;
        private Vector3 _initialRotation;
        
        private Vector3 _initialPaletteScale;
        private Vector3 _initialPaletteRotation;

        private float _targetScrollAngle;
        private float _scrollAngle;

        public float WorldVerticalWheelCenter => transform.position.y;

        private void Start()
        {
            _initialScale = rotatorTransform.localScale;
            _initialRotation = rotatorTransform.localRotation.eulerAngles;
            
            _initialPaletteScale = paletteTransform.localScale;
            _initialPaletteRotation = paletteTransform.localRotation.eulerAngles;

            _scrollAngle = palette.transform.localRotation.eulerAngles.z;
            _targetScrollAngle = _scrollAngle;
        }

        public void Show()
        {
            KillTwins();
            
            rotator.DOFade(1f, showDuration).SetEase(showRotatorEase);
            rotatorTransform.DOScale(Vector3.one, showDuration).SetEase(showRotatorScaleEase);
            rotatorTransform.DOLocalRotate(Vector3.zero, showDuration).SetEase(showRotatorRotateEase);

            paletteTransform.localScale = _initialPaletteScale;
            palette.DOFade(1f, showPaletteDuration).SetEase(showPaletteRotatorEase).SetDelay(showPaletteDelay);
            paletteTransform.DOScale(Vector3.one, showPaletteDuration).SetEase(showPaletteRotatorScaleEase).SetDelay(showPaletteDelay);
            paletteTransform.DOLocalRotate(new Vector3(0,0,-360), showPaletteDuration).SetEase(showPaletteRotatorRotateEase).SetDelay(showPaletteDelay);
        }
        
        public void Hide()
        {
            KillTwins();
            
            rotator.DOFade(0f, hideDuration).SetEase(hideRotatorEase);
            rotatorTransform.DOScale(_initialScale, hideDuration).SetEase(hideRotatorScaleEase);
            rotatorTransform.DOLocalRotate(_initialRotation, hideDuration).SetEase(hideRotatorRotateEase);
            
            palette.DOFade(0f, hidePaletteDuration).SetEase(hidePaletteRotatorEase);
            paletteTransform.DOScale(Vector3.one * hidePaletteScale, hidePaletteTransformsDuration).SetEase(hidePaletteRotatorScaleEase);
            paletteTransform.DOLocalRotate(_initialPaletteRotation, hidePaletteTransformsDuration).SetEase(hidePaletteRotatorRotateEase);
        }
        
        private void KillTwins()
        {
            rotator.DOKill();
            rotatorTransform.DOKill();
            palette.DOKill();
            paletteTransform.DOKill();
        }
        
        public void ReadyCloseState()
        {
            rotator.transform.DOKill();
            rotator.transform.DOScale(Vector3.one * readyCloseStateScale,readyCloseStateDuration ).SetEase(readyCloseStateEase);
        }


        public void ReadySelectMessageState()
        {
            rotator.transform.DOKill();
            rotator.transform.DOScale(Vector3.one,readySelectMessageStateDuration ).SetEase(readySelectMessageStateEase);
        }

        private void Update()
        {
            var angles = palette.transform.localRotation.eulerAngles;
            _targetScrollAngle = Mathf.Lerp(_targetScrollAngle, _scrollAngle, Speed);
            palette.transform.localRotation = Quaternion.Euler(angles.x, angles.y, _targetScrollAngle);
        }

        public void MoveWheel(float rotation) => _scrollAngle += (rotation * SpeedCoef);
    }
}
