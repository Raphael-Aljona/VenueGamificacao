using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string miniGameSceneName = "MiniGameSolo";
    [SerializeField] private string lobbySceneName = "Lobby";

    private bool isCreatingSoloRoom = false;

    public void StartMiniGame()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Saindo da sala atual antes de criar sala solo...");
            isCreatingSoloRoom = true;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            CreateSoloRoom();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Saiu da sala. Aguardando reconexão ao Master Server...");
    }

    public override void OnConnectedToMaster()
    {
        if (isCreatingSoloRoom)
        {
            Debug.Log("Reconectado ao Master Server! Criando sala solo...");
            CreateSoloRoom();
        }
    }

    private void CreateSoloRoom()
    {
        string roomName = "Solo_" + PhotonNetwork.NickName + "_" + Random.Range(1000, 9999);
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 1,
            IsVisible = false,
            IsOpen = false
        };

        Debug.Log($"Criando sala solo: {roomName}");
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Sala solo criada com sucesso! Carregando minigame...");
        PhotonNetwork.LoadLevel(miniGameSceneName);
    }

    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    Debug.LogError($"Falha ao criar sala solo: {message}. Iniciando modo offline...");
    //    PhotonNetwork.Disconnect();
    //    SceneManager.LoadScene(miniGameSceneName);
    //}

    public void ReturnToLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Saindo da sala solo...");
            isCreatingSoloRoom = false;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(lobbySceneName);
        }
    }

    public override void OnLeftLobby()
    {
        SceneManager.LoadScene(lobbySceneName);
    }
}
