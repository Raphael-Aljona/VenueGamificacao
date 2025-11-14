using TMPro;
using UnityEngine;

public class AnswerZone2D : MonoBehaviour
{
    public int zoneIndex;
    public TextMeshPro answerText;

    public void SetAnswerText(string text)
    {
        if (answerText != null)
        {
            answerText.text = text;
        }
    }
}
