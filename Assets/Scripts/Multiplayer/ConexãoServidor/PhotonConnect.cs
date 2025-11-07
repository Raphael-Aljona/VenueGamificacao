using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonConnect : MonoBehaviourPunCallbacks
{

    public string hubScene = "Rooms";
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.OfflineMode = false;
        }
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Conectando ao Photon...");
        }
    }

    //public void EnterHub()
    //{
    //    if (PhotonNetwork.IsConnected)
    //    {
    //        TryJoinRoom();
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Ainda não conectado ao Photon, tentando novamente...");
    //        PhotonNetwork.ConnectUsingSettings();
    //    }
    //}

    void TryJoinRoom()
    {
        Debug.Log("Tentando entrar em uma sala existente...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Nenhuma sala encontrada, criando uma nova...");
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 10 };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Entrou na sala com sucesso!");
        // Carrega automaticamente o hub quando entrar em uma sala
        if (SceneManager.GetActiveScene().name != hubScene)
        {
            SceneManager.LoadScene(hubScene);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Desconectado: " + cause);
    }
}
