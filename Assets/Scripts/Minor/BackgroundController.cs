using UnityEngine;
using UnityEngine.UI;

namespace Minor
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField]
        private GameObject phoneBackground;
        
        [SerializeField]
        private AnimationCurve phoneBackgroundBorderMultiplier;
        
        [SerializeField]
        private Image phoneBackgroundImage;
        
        [SerializeField]
        private Image phoneBackgroundFrameImage;
    
        [SerializeField]
        private GameObject tabletBackground;

        private float _aspectRatio;
        private float _phoneBackgroundImageAspectRatio;
    
        private void Awake()
        {
            _phoneBackgroundImageAspectRatio = (float) phoneBackgroundImage.sprite.texture.height / phoneBackgroundImage.sprite.texture.width;
            _aspectRatio = AspectRatio();
            UpdateBackground();
        }
    
        private void Update()
        {
            var currentAspectRatio = AspectRatio();

            if (_aspectRatio == currentAspectRatio) return;
            
            _aspectRatio = currentAspectRatio;
            UpdateBackground();

        }

        private void UpdateBackground()
        {
            var usePhoneBackground = _phoneBackgroundImageAspectRatio <= _aspectRatio;
            phoneBackground.SetActive(usePhoneBackground);
            tabletBackground.SetActive(!usePhoneBackground);

            if (usePhoneBackground)
                phoneBackgroundFrameImage.pixelsPerUnitMultiplier = phoneBackgroundBorderMultiplier.Evaluate(_aspectRatio);
        }

        private static float AspectRatio() => (float) Screen.height / Screen.width;
    }
}
