using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ExpectedArea : MonoBehaviour
{
    public string statusTask;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CardController cardController = collision.GetComponent<CardController>();
        Debug.Log($"cardController{cardController.status}");

        cardController.inArea = true;

        if (!cardController.concluido)
        {
            if (cardController.status.Trim().ToLower() == statusTask.Trim().ToLower())
            {
                CardSpawner cardSpawner = FindAnyObjectByType<CardSpawner>();
                cardController.concluido = true;
                Debug.Log($"cardController{cardController.status}");
                cardController.inCorrectArea = true;

                cardController.SetSpriteCorreto();
                cardSpawner.SpawnNextCard();
                gameManager.AddPoints();
            }
            else 
            {
                cardController.inCorrectArea = false;
                cardController.SetSpriteErrado();
                gameManager.RemovePoints();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CardController cardController = collision.GetComponent<CardController>();

        cardController.inArea = false;
        cardController.inCorrectArea = false;
        cardController.SetSpriteNeutro();

        if (cardController.status.Trim().ToLower() == statusTask.Trim().ToLower())
        {
            Debug.Log($"Card correto removido da área {statusTask}");
            //cardController.concluido = false;
            gameManager.RemovePoints();
        }
    }
}
