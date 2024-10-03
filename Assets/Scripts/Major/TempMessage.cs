using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Major
{
    public class TempMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        [SerializeField] private Ease scaleEase;
        [Space]
        [SerializeField] private float showDuration;
        [SerializeField] private Ease showEase;
        [Space]
        [SerializeField] private float hideDuration;
        [SerializeField] private Ease hideEase;
       
        private void Start() => Show();

        private void Show()
        {
            text.DOFade(1f,showDuration).SetEase(showEase).OnComplete(Hide);
            text.transform.DOScale(1.1f,showDuration+hideDuration).SetEase(scaleEase);
        }

        private void Hide()
        {
            text.DOFade(0f,hideDuration).SetEase(hideEase).OnComplete(() => Destroy(this.gameObject));
        }

        public void Initialize(string message) => text.text = message;
    }
}
