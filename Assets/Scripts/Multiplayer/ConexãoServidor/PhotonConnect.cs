using Photon.Pun;
using UnityEngine;

public class PhotonConnect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Conectando ao Photon...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao servidor Photon!");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogError("Desconectado: " + cause);
    }
}
