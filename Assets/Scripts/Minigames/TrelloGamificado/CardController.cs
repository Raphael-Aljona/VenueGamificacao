using UnityEngine;

public class CardController : MonoBehaviour
{
    public string titulo;
    public string status;
    public string descricao;
    public int id;

    public Tarefa tarefa;
    public void ConfigurarCard (Tarefa tarefa)
    {
        titulo = tarefa.titulo;
        status = tarefa.status;
        descricao = tarefa.textoDaConclusao;
        id = tarefa.id;

        Debug.Log(tarefa);
    }

}
