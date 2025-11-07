using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModalCompeticaoManager : MonoBehaviourPunCallbacks
{
    public GameObject modalCompeticao;
    public string gameTrello = "GameTrello";
    PlayerMovement playerMovement;

    private bool goingToMiniGame = false;

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
        if (PhotonNetwork.InRoom)
        {
            PlayerPrefs.SetFloat("PlayerX", playerMovement.playerPosition.position.x);
            PlayerPrefs.SetFloat("PlayerY", playerMovement.playerPosition.position.y);
            PlayerPrefs.Save();

            goingToMiniGame = true; // sinaliza que a próxima sala é o minigame
            PhotonNetwork.LeaveRoom(); //  sai da sala primeiro
        }
        else
        {
            Debug.LogWarning("O jogador não está em uma sala, indo direto para o minigame...");
            JoinMiniGameRoom();
        }
    }

    // Chamado automaticamente quando o jogador sai da sala
    public override void OnLeftRoom()
    {
        Debug.Log("Saiu da sala atual. Voltando ao Master Server...");
        StartCoroutine(WaitAndJoinMiniGame());
    }

    private IEnumerator WaitAndJoinMiniGame()
    {
        // Aguarda reconexão ao Master Server
        while (PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer)
        {
            yield return null;
        }

        if (goingToMiniGame)
        {
            JoinMiniGameRoom();
        }
    }

    private void JoinMiniGameRoom()
    {
        string roomName = "MiniGame_" + PhotonNetwork.LocalPlayer.UserId;
        RoomOptions options = new RoomOptions { MaxPlayers = 1, IsVisible = false };

        Debug.Log($"Criando ou entrando na sala: {roomName}");

        // Desativa a sincronização automática de cena neste modo solo
        PhotonNetwork.AutomaticallySyncScene = false;

        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.Name.StartsWith("MiniGame_"))
        {
            Debug.Log("Entrou no minigame, carregando cena GameTrello...");
            PhotonNetwork.LoadLevel(gameTrello);
        }
    }
}
