using UnityEngine;
using UnityEngine.EventSystems;

namespace Major
{
    public class WheelTapBehaviour : MonoBehaviour, IPointerMoveHandler
    {
        [SerializeField] private ShowButton showButton;
        [SerializeField] private WheelAnimator wheelAnimator;
        [SerializeField] private CloseEventCaller closeEventCaller;
        
        [Space(30)]
        [Header("Scroll")]
        [SerializeField] private AnimationCurve horizontalScrollDistanceCoef;

        private const float HorizontalScrollCoef = 0.6f;

        private void OnEnable()
        {
            closeEventCaller.OnPointerEnter += CloseEventCallerOnOnPointerEnter;
            closeEventCaller.OnPointerExit += CloseEventCallerOnOnPointerExit;
        }
        
        private void OnDisable()
        {
            closeEventCaller.OnPointerEnter -= CloseEventCallerOnOnPointerEnter;
            closeEventCaller.OnPointerExit -= CloseEventCallerOnOnPointerExit;
        }

        private void CloseEventCallerOnOnPointerEnter()
        {
            showButton.ReadyCloseState();
            wheelAnimator.ReadyCloseState();
        }

        private void CloseEventCallerOnOnPointerExit()
        {
            showButton.ReadySelectMessageState();
            wheelAnimator.ReadySelectMessageState();
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            var resultScrollValue = 0f;
            
            var verticalScroll = -eventData.delta.y;
            var horizontalScroll = -eventData.delta.x;

            if (!(float.IsPositiveInfinity(verticalScroll) || float.IsNegativeInfinity(verticalScroll)))
                resultScrollValue += verticalScroll;
            
            if (!(float.IsPositiveInfinity(horizontalScroll) || float.IsNegativeInfinity(horizontalScroll)))
            {
                var isClockwiseRotation = eventData.position.y > wheelAnimator.WorldVerticalWheelCenter;
                var centerDistance = Mathf.Abs(eventData.position.y - wheelAnimator.WorldVerticalWheelCenter);
                var scrollValue = (isClockwiseRotation ? horizontalScroll : -horizontalScroll);
                resultScrollValue += scrollValue * HorizontalScrollCoef * horizontalScrollDistanceCoef.Evaluate(centerDistance);
            }
            
            wheelAnimator.MoveWheel(resultScrollValue);
        }
    }
}
