using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ReturnToHubManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string hubScene = "Rooms";
    [SerializeField] private string roomName = "HubGamificado";

    private bool returningToHub = false;
    private bool waitingForLobby = false;

    public void OnReturnToHubPressed()
    {
        if (returningToHub) return;

        returningToHub = true;
        Debug.Log("Voltando ao hub...");

        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Saindo da sala atual...");
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            TryJoinHubRoom();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Saiu da sala atual. Indo para o hub...");
        TryJoinHubRoom();
    }

    private void TryJoinHubRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Não conectado. Tentando reconectar...");
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("Aguardando entrada no lobby...");
            waitingForLobby = true;
            PhotonNetwork.JoinLobby();
            return;
        }

        Debug.Log($"Tentando entrar ou criar a sala '{roomName}'...");
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 20 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        if (waitingForLobby)
        {
            waitingForLobby = false;
            Debug.Log("Entrou no lobby. Agora tentando entrar na sala do hub...");
            TryJoinHubRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Entrou na sala '{PhotonNetwork.CurrentRoom.Name}', carregando o hub...");
        PhotonNetwork.LoadLevel(hubScene);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($" Falhou ao entrar na sala '{roomName}': {message}. Tentando criar nova...");
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 20 };
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao Master. Indo para o hub...");
        if (returningToHub)
            TryJoinHubRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"Desconectado do Photon: {cause}");
    }
}
