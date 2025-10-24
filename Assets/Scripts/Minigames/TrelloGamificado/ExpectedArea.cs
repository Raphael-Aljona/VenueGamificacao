using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ExpectedArea : MonoBehaviour
{
    public string statusTask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CardController cardController = collision.GetComponent<CardController>();
        if (!cardController.concluido)
        {
            if (cardController.status.Trim().ToLower() == statusTask.Trim().ToLower())
            {
                Debug.Log("Atividade no lugar correto");
                CardSpawner cardSpawner = FindAnyObjectByType<CardSpawner>();
                cardController.concluido = true;

                cardSpawner.SpawnNextCard();

            }
        }
        else
        {
            Debug.Log("Local da atividade errado");
        }
    }
}
