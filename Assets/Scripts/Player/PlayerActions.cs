using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerActions : MonoBehaviourPun
{
    private KeyCode interactable = KeyCode.E;
    [SerializeField]private GameObject modalCompeticao;
    public bool inCompetitionArea;
    void Start()
    { 
        inCompetitionArea = false;
        Scene actualSceneName = SceneManager.GetActiveScene();
        if (actualSceneName.name  == "Rooms")
        {
            modalCompeticao = GameObject.FindGameObjectWithTag("ModalCompeticao");
            modalCompeticao.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (inCompetitionArea && Input.GetKeyDown(interactable))
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
