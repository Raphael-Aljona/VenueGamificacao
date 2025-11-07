using Photon.Pun;
using UnityEngine;

public class PlayerSpawnerNetwork : MonoBehaviourPunCallbacks
{
    public override void OnJoinedRoom()
    {
        Debug.Log("Entrou na sala, criando player...");

        Vector3 spawnPos = new Vector3(Random.Range(-4f, 4f), Random.Range(-3f, 3f), 0);
        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
    }
}
