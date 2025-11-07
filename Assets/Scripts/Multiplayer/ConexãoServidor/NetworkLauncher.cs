using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string sceneName = "Hub";

    private static NetworkLauncher instance;
    private bool isReadyToJoinRoom = false;
    public MenuManager menuManager;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Ouve quando uma cena é carregada
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Debug.Log("Conectando ao Photon...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnLoginButtonPressed()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("Botão de login clicado! Criando/entrando na sala...");
            isReadyToJoinRoom = true;
            menuManager.ShowHUD();
            JoinOrCreateRoom();
        }
        else
        {
            Debug.LogWarning("Ainda não conectado ao Photon!");
        }
    }

    void JoinOrCreateRoom()
    {
        string roomName = "Sala_Global";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (isReadyToJoinRoom)
        {
            Debug.Log("Entrou na sala, carregando cena...");
            PhotonNetwork.LoadLevel(sceneName);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneName && PhotonNetwork.InRoom)
        {
            Debug.Log("Cena do Hub carregada, spawnando player...");
            Vector3 spawnPos = new Vector3(Random.Range(-4f, 4f), Random.Range(-3f, 3f), 0);
            PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        }
    }
}
