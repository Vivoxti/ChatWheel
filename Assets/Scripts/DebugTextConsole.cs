using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugTextConsole : MonoBehaviour
{
    public Slider a1;
    public Slider a2;
    public Slider a3;

    public TextMeshProUGUI text;

    private void Update()
    {
        text.text = $"{a1.value} - {a2.value} - {a3.value}";
    }
}
