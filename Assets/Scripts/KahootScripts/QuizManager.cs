using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class QuizManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public TextMeshPro questionText;
    public TextMeshProUGUI timerText;
    public float roundTime = 10f;

    [Header("Answer Zones")]
    public AnswerZone2D[] answerZones; // zonas de resposta (0–3)
    private int correctZoneIndex;

    [System.Serializable]
    public class Question
    {
        public string question;
        public string[] answers;
        public int correctIndex;
    }

    [Header("Perguntas")]
    public Question[] questions;
    private int currentIndex = 0;

    private float timer;
    private bool roundActive = false;

    void Start()
    {
        //if (PhotonNetwork.IsMasterClient)
        //    StartCoroutine(StartRound());

        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
            StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        yield return new WaitForSeconds(1f);

        LoadNextQuestion();
        roundActive = true;
        timer = roundTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            photonView.RPC("UpdateTimer", RpcTarget.All, Mathf.CeilToInt(timer));
            yield return null;
        }

        roundActive = false;
        EvaluateAnswers();

        yield return new WaitForSeconds(2f);
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(StartRound());
    }

    void LoadNextQuestion()
    {
        var q = questions[currentIndex];
        currentIndex = (currentIndex + 1) % questions.Length;

        // Garante que a pergunta tem 4 respostas
        if (q.answers == null || q.answers.Length != 4)
        {
            Debug.LogError($"A pergunta \"{q.question}\" não tem exatamente 4 alternativas!");
            return; // cancela o carregamento dessa pergunta
        }

        // embaralha as respostas
        int[] shuffled = Enumerable.Range(0, 4).OrderBy(x => Random.value).ToArray();
        correctZoneIndex = shuffled.ToList().IndexOf(q.correctIndex);

        photonView.RPC("DisplayQuestion", RpcTarget.All,
            q.question,
            q.answers[shuffled[0]],
            q.answers[shuffled[1]],
            q.answers[shuffled[2]],
            q.answers[shuffled[3]],
            correctZoneIndex
        );
    }

    [PunRPC]
    void DisplayQuestion(string question, string a1, string a2, string a3, string a4, int correctIndex)
    {
        questionText.text = question;
        correctZoneIndex = correctIndex;

        string[] answers = { a1, a2, a3, a4 };
        for (int i = 0; i < answerZones.Length; i++)
        {
            answerZones[i].SetAnswerText(answers[i]);
        }
    }

    [PunRPC]
    void UpdateTimer(int timeLeft)
    {
        timerText.text = $" {timeLeft}s";
    }

    void EvaluateAnswers()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var p in players)
        {
            PlayerZoneChecker2D checker = p.GetComponent<PlayerZoneChecker2D>();
            if (checker != null)
            {
                if (checker.currentZone == correctZoneIndex)
                {
                    Debug.Log($"{p.name} acertou!");
                    // aqui dá pra mandar RPC de pontuação
                }
                else
                {
                    Debug.Log($"{p.name} errou!");
                }
            }
        }
    }
}
