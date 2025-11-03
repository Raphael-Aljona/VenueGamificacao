using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    //public TextMeshProUGUI scoreText;

    public TextMeshProUGUI finalTimerText;
    public TextMeshProUGUI finalScoreText;

    private float timer = 0f;
    private bool isPlaying = false;

    public int score = 0;
    public int addScore = 100;
    public int removeScore = 50;

    public GameObject infoGame;
    public GameObject gameOver;
    public GameObject topPanel;
    void Start()
    {
        isPlaying = true;
        UpdateUI();
        infoGame.SetActive(true);
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
            UpdateUI();

            if (AllCardsPositioned())
            {
                Ended();
            }
        }
    }

    public void UpdateUI()
    {

        int min = Mathf.FloorToInt(timer / 60);
        int sec = Mathf.FloorToInt(timer % 60);

        timerText.text = $"{min:00}:{sec:00}";

        //scoreText.text = score.ToString();

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

    public bool AllCardsPositioned()
    {
        CardController[] cards = FindObjectsOfType<CardController>();

        foreach (CardController card in cards) {
            if (!card.inArea || !card.inCorrectArea)
            {
                return false;
            }
        }
        return true;
    }

    public void Ended()
    {
        isPlaying = false;
        infoGame.SetActive(false);
        topPanel.SetActive(false);
        
        int min = Mathf.FloorToInt(timer / 60);
        int sec = Mathf.FloorToInt(timer % 60);

        finalTimerText.text = $"Tempo final: {min:00}:{sec:00}";
        // Salvar valor abaixo em uma variavel
        finalScoreText.text =$"Pontuação final: {Mathf.RoundToInt(score * (100f / (timer + 1f))).ToString()}";

        gameOver.SetActive(true);
    }
}
