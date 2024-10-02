using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Major.Messages
{
    public class MessagesController : MonoBehaviour
    {
        [SerializeField] private List<Message> availableMessages;

        private void SpawnMessages()
        {
            var messages = availableMessages.Randomize().ToList();
        }
    }
    
    public static class Extensions
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source) => source.OrderBy(_ => Random.Range(0,100));
    }
}
