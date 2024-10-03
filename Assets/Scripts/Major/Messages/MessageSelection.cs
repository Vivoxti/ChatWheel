using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Major.Messages
{
    public class MessageSelection : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TextMeshProUGUI textFitter;
        [SerializeField] private MessageFitter fitter;
        
        [SerializeField] private AnimationCurve opacityByAngle;
        
        [Space] 
        
        [SerializeField] private float selectionDuration;
        [SerializeField] private Vector3 selectionForce;
        [SerializeField] private Ease selectionEase;

        public string Text => textLabel.text;
        
        public void Initialize(string messageText, Color textColor)
        {
            textLabel.text = messageText;
            textFitter.text = messageText;
            textLabel.color = textColor;
            StartCoroutine(fitter.Fit());
        }

        private void Update() => UpdateOpacity();

        private void UpdateOpacity()
        {
            var alpha = opacityByAngle.Evaluate(rect.eulerAngles.z);
            textLabel.alpha = alpha;

            var backgroundColor = background.color;
            backgroundColor.a = alpha;
            background.color = backgroundColor;
        }

        public void Selection()
        {
            textLabel.transform.DOPunchScale(selectionForce, selectionDuration).SetEase(selectionEase)
                .OnComplete(() => textLabel.transform.localScale = Vector3.one);
        }
    }
}
