using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private KeyCode interactable = KeyCode.E;
    public GameObject modalCompeticao;
    public bool inCompetitionArea;
    void Start()
    {
        inCompetitionArea = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(inCompetitionArea && Input.GetKeyDown(interactable))
        {
            modalCompeticao.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Competicao"))
        {
            inCompetitionArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Competicao"))
        {
            inCompetitionArea = false;
        }
    }
}
