using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ExpectedArea : MonoBehaviour
{
    public string statusTask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CardController cardController = collision.GetComponent<CardController>();
        Debug.Log(cardController.tarefa.status);
        if (cardController.tarefa.status.Trim() == statusTask.Trim())
        {
            Debug.Log("Atividade no lugar correto");
        }
        else
        {
            Debug.Log("Local da atividade errado");
        }
    }
}
