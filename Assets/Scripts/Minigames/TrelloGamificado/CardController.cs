using TMPro;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public string titulo;
    public string status;
    public string descricao;
    public int id;

    public Tarefa tarefa;

    public TextMeshProUGUI tituloModal;
    public TextMeshProUGUI descModal;
    public bool concluido;

    public void ConfigurarCard (Tarefa tarefa)
    {

        titulo = tarefa.titulo;
        status = tarefa.status;
        descricao = tarefa.textoDaConclusao;
        id = tarefa.id;

        Debug.Log(tarefa);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tituloModal.text = titulo;
            descModal.text = descricao;
            Debug.Log($"tituloModal:{tituloModal.text}");
        }

    }
}
