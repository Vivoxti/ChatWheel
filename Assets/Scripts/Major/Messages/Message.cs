using UnityEngine;

namespace Major.Messages
{
    [CreateAssetMenu(fileName = "Message", menuName = "Vivoderin/Messages", order = 1)]
    public class Message : ScriptableObject
    {
        [SerializeField] private string text;

        public string Text => text;
    }
}
