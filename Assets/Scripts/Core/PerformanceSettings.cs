using UnityEngine;

namespace Architecture
{
    public class PerformanceSettings : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 60;
    
        private void Start()
        {
            Application.targetFrameRate = targetFrameRate; 
        }
    }
}
