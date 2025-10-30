using TMPro;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public string titulo;
    public string status;
    public string descricao;
    public int id;
    public bool inArea = false;
    public bool inCorrectArea = false;

    public Tarefa tarefa;

    public TextMeshProUGUI tituloModal;
    public TextMeshProUGUI descModal;
    public bool concluido;

    public Sprite spriteCorreto;
    public Sprite spriteErrado;
    public Sprite spriteNeutro;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ConfigurarCard(Tarefa tarefa)
    {

        titulo = tarefa.titulo;
        status = tarefa.status;
        descricao = tarefa.textoDaConclusao;
        id = tarefa.id;
        inArea = false;
        inCorrectArea = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tituloModal.text = titulo;
            descModal.text = descricao;
        }

    }

    public void SetSpriteCorreto()
    {
        spriteRenderer.sprite = spriteCorreto;
    }
    public void SetSpriteErrado()
    {
        spriteRenderer.sprite = spriteErrado;
    }
    public void SetSpriteNeutro()
    {
        spriteRenderer.sprite = spriteNeutro;
    }
}
