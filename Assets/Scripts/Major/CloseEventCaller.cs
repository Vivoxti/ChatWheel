using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Major
{
    public class CloseEventCaller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action OnPointerEnter;
        public event Action OnPointerExit;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => OnPointerEnter?.Invoke();

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => OnPointerExit?.Invoke();
    }
}
