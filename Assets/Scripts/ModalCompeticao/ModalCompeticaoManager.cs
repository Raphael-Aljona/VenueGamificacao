using UnityEngine;
using UnityEngine.SceneManagement;

public class ModalCompeticaoManager : MonoBehaviour
{
    public GameObject modalCompeticao;
    public string teste = "teste";
    PlayerMovement playerMovement;

    // Update is called once per frame
    void Update()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();

        if (modalCompeticao.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseModal();
        }
    }

    public void CloseModal()
    {
        modalCompeticao.SetActive(false);
    }

    public void GoToScene()
    {
        PlayerPrefs.SetFloat("PlayerX", playerMovement.playerPosition.position.x);
        PlayerPrefs.SetFloat("PlayerY", playerMovement.playerPosition.position.y);

        PlayerPrefs.Save();

        SceneManager.LoadScene(teste);
    }
}
