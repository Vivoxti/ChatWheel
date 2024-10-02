using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Minor
{
    public class InitialFade : MonoBehaviour
    {
        [SerializeField] private RectTransform startFadeTransform;
        [SerializeField] private Image startFadeImage;
        [SerializeField] private CanvasScaler scaler;
        
        [SerializeField][Range(0,5f)] private float duration;

        private void Start() => InitTweenPartOne();
        
        private void InitTweenPartOne()
        {
            var maxScale = Mathf.Max(scaler.referenceResolution.x, scaler.referenceResolution.y);
            startFadeTransform.DOSizeDelta(new Vector2(maxScale, maxScale), duration / 2f).OnComplete(InitTweenPartTwo).SetEase(Ease.Linear);
        }
        
        private void InitTweenPartTwo()
        {
            startFadeImage.DOFade(0f, duration / 2f).SetEase(Ease.Linear);
            startFadeTransform.DOScale(new Vector3(4, 4, 1), duration / 2f).SetEase(Ease.Linear).OnComplete(Hide); 
        }
        private void Hide() => Destroy(startFadeTransform.gameObject); 
    }
}
