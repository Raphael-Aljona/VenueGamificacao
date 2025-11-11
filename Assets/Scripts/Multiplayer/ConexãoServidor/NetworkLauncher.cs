using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string sceneName = "Rooms";

    private static NetworkLauncher instance;
    private bool isReadyToJoinRoom = false;
    public MenuManager menuManager;

    private bool playerSpawned = false;

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
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
            if (menuManager == null)
                menuManager = FindAnyObjectByType<MenuManager>();

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
        PhotonNetwork.OfflineMode = false;
        string roomName = "HubGamificado";
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
        if (scene.name == sceneName && PhotonNetwork.InRoom && !playerSpawned)
        {
            if (!AlreadyHasLocalPlayer())
            {
                SpawnPlayer();
            }
        }
    }
    private bool AlreadyHasLocalPlayer()
    {
        foreach (var view in FindObjectsOfType<PhotonView>())
        {
            if (view.IsMine) return true;
        }
        return false;
    }

    private void SpawnPlayer()
    {
        Debug.Log("Cena do Hub carregada, spawnando player...");

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
        Vector3 spawnPos = Vector3.zero;

        if (spawnPoints.Length > 0)
        {
            spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        }

        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
    }
}
