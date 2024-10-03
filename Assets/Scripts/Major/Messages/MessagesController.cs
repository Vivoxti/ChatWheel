using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Major.Messages
{
    public class MessagesController : MonoBehaviour
    {
        [SerializeField] private List<Message> availableMessages;
        [SerializeField] private Transform contentContainer;
        [SerializeField] private MessageSelection messagePrefab;
        [SerializeField] private WheelAnimator wheel;
        [Space]
        [SerializeField] private float positionOffset;
        [SerializeField] private float angleSelectionOffset;
        [SerializeField] private float angleSelectionThreshold;
        [Space] 
        [SerializeField] private List<Color> colorsByIndex;
        [Header("TempMessage")]
        [SerializeField] private TempMessage tempMessagePrefab;
        [SerializeField] private RectTransform canvas;
        
        private readonly List<MessageSelection> _messages = new();

        private float _anglePerMessage;

        private MessageSelection _selectedMessage;

        private void Awake() => _anglePerMessage = 360f / availableMessages.Count;

        private void Start()
        {
            SpawnMessages();
            HideMessages();
        }

        public void SpawnMessages()
        {
            var messages = availableMessages.Randomize().ToList();
            
            foreach (var message in messages)
            {
                var messageObject = Instantiate(messagePrefab, contentContainer);
                
                var zAngle = _anglePerMessage * _messages.Count;
                var position = (Vector2)(Quaternion.Euler(0,0,zAngle) * Vector2.right);
                var rect = messageObject.GetComponent<RectTransform>();
                
                rect.anchoredPosition = new Vector3( position.x * positionOffset, position.y * positionOffset,0);
                rect.localRotation = Quaternion.Euler(0, 0, zAngle);
                
                messageObject.Initialize(message.Text, colorsByIndex[_messages.Count]);
                
                _messages.Add(messageObject);
            }
        }
        
        public void ShowMessages()
        {
            foreach (var messageSelection in _messages)
                messageSelection.gameObject.SetActive(true);
        }

        public void HideMessages()
        {
            foreach (var messageSelection in _messages)
                messageSelection.gameObject.SetActive(false);
        }

        public void UpdateSelection(float angle)
        {
            if (_messages.Count <= 0) return;
            
            var offsetAngle = -angle + angleSelectionOffset;
            while (offsetAngle < 0) offsetAngle += 360;
            if (Mathf.Abs(offsetAngle) > 360) offsetAngle %= 360f;
            
            var selectedMessage = _selectedMessage;
            
            foreach (var messageSelection in _messages)
            {
                var angleDistance = Mathf.Abs(messageSelection.transform.localEulerAngles.z - offsetAngle);
                if (angleDistance > 180) angleDistance = 360 - angleDistance;
                
                if (!(angleSelectionThreshold > angleDistance)) continue;
                
                selectedMessage = messageSelection;
                break;
            }

            if (_selectedMessage == selectedMessage) return;
            
            _selectedMessage = selectedMessage;
            _selectedMessage.Selection();

        }

        public void SpawnTempMessage()
        {
            if (wheel.SkipMessage) return;
            
            var message = Instantiate(tempMessagePrefab, canvas);
            message.Initialize(_selectedMessage.Text);
        }
    }
    
    public static class Extensions
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source) => source.OrderBy(_ => Random.Range(0,100));
    }
}
