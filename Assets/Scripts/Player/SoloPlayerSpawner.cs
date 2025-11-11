using Photon.Pun;
using UnityEngine;

public class SoloPlayerSpawner : MonoBehaviour
{
    public GameObject PlayerSpawner;
    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.Instantiate(PlayerSpawner.name, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
