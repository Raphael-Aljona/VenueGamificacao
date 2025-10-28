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


        if (!cardController.concluido)
        {
            if (cardController.status.Trim().ToLower() == statusTask.Trim().ToLower())
            {
                CardSpawner cardSpawner = FindAnyObjectByType<CardSpawner>();
                cardController.concluido = true;
                Debug.Log($"cardController{cardController.status}");

                cardSpawner.SpawnNextCard();
                gameManager.AddPoints();
            }
            else 
            {
                gameManager.RemovePoints();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CardController cardController = collision.GetComponent<CardController>();

        if (cardController.status.Trim().ToLower() == statusTask.Trim().ToLower())
        {
            Debug.Log($"Card correto removido da área {statusTask}");
            //cardController.concluido = false;
            gameManager.RemovePoints();
        }
    }
}
