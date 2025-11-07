using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModalCompeticaoManager : MonoBehaviour
{
    public GameObject modalCompeticao;
    public string gameTrello = "GameTrello";
    PlayerMovement playerMovement;

    //Update is called once per frame
    //public void Start()
    //{
    //    CloseModal();
    //}

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

        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.Disconnect();
        StartCoroutine(LoadSoloScene());
    }

    private IEnumerator LoadSoloScene()
    {
        while (PhotonNetwork.IsConnected)
            yield return null;

        PhotonNetwork.OfflineMode = true;
        SceneManager.LoadScene("GameTrello");
    }
}
