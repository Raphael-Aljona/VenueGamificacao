using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    private float timer = 0f;
    private bool isPlaying = false;

    public int score = 0;
    public int addScore = 10;
    public int removeScore = -5;

    void Start()
    {
        isPlaying = true;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {

        int min = Mathf.FloorToInt(timer / 60);
        int sec = Mathf.FloorToInt(timer % 60);

        timerText.text = $"{min:00}:{sec:00}";

        scoreText.text = score.ToString();

    }

    public void AddPoints()
    {
        score += addScore;
        UpdateUI();
    }
    public void RemovePoints()
    {
        score -= removeScore;
        if (score < 0) score = 0;
        UpdateUI();
    }

    public void Ended()
    {
        isPlaying = false;
    }
}
