using System.Collections;
using UnityEngine;

namespace Major.Messages
{
    public class MessageFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform text;
        [SerializeField] private RectTransform container;
        
        [SerializeField] private float containerAdditionalSizeX;

        private void Start() => StartCoroutine(Fit());
        
        public IEnumerator Fit()
        {
            yield return null;
            container.sizeDelta = new Vector2(text.sizeDelta.x + containerAdditionalSizeX, container.sizeDelta.y);
        }
    }
}
