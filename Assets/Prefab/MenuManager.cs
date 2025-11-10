using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement; // Necessário para gerenciar cenas e o Quit
using UnityEngine.UI; // Necessário para Text e Image

// Enums para o tipo de resultado da tela final
public enum GameResult
{
    Promocao,
    Excelente,
    Miseravel,
    Rebaixado
}
public class MenuManager : MonoBehaviour
{
    // --- NOVO: Singleton Instance ---
    public static MenuManager Instance; // Variável estática para acesso global

    // --- Referências aos Painéis (GameObjects) ---
    // Arraste os painéis da hierarquia (criados na Unity) para estas variáveis no Inspector
    [Header("Painéis Principais")]
    public GameObject loginMenuPanel;
    public GameObject creditosPanel;
    public GameObject multiplayerMenuPanel;
    public GameObject hudDoJogoPanel;

    [Header("Painéis de Estado do Jogo")]
    public GameObject pausePanel;
    public GameObject fundoConfigII;

    [Header("Componentes Tela Final")]
    public GameObject finalizadoPanel; // O painel base que contém tudo
    public TextMeshProUGUI resultadoText; // Ou Text (Unity Engine)
    public Image faixaImage; // A imagem da faixa que muda de cor

    [Header("Configurações de Áudio")]
    public AudioMixer mainMixer; // Arraste seu MainMixer aqui

    // Variáveis de estado
    private bool isMusicOn = true;
    private bool isSfxOn = true;

    [Header("Sprites de Feedback Visual")]
    public Image musicButtonImage; // Arraste o componente Image do botão de Música
    public Sprite musicOnSprite;    // Sprite para Música Ligada (ON)
    public Sprite musicOffSprite;   // Sprite para Música Desligada (OFF)

    public Image sfxButtonImage;   // Arraste o componente Image do botão de SFX
    public Sprite sfxOnSprite;      // Sprite para SFX Ligado (ON)
    public Sprite sfxOffSprite;     // Sprite para SFX Desligado (OFF)

    [Header("Gerenciamento de Cenas")]
    public string nomeCenaMapaPrincipal = "NomeDaSuaCenaDoMapaPrincipal"; // <--- IMPORTANTE: Preencha no Inspector!

    void Awake()
    {
        // 1. Singleton: Garante que haja apenas um MenuManager
        if (Instance == null)
        {
            Instance = this;
            // CRUCIAL: Mantém este objeto (e o Canvas) vivo
            DontDestroyOnLoad(this.gameObject);

            // Assina o evento para reagir a cada carregamento de cena
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Se já existe um MenuManager, destrói esta cópia duplicada (o que você colocou na cena)
            Destroy(this.gameObject);
            return; // Sai do Awake para não continuar a execução
        }
    }

    void OnDestroy()
    {
        // Boa prática: Remove o Listener
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // --- Configuração Inicial ---
    void Start()
    {
        // IMPORTANTE: Este Start() SÓ deve ser executado no Canvas da CENA PRINCIPAL.
        // Se este for o Canvas original (Instance == this), ele configura o estado inicial.
       
            ShowLoginMenu();
            Debug.Log("Iniciando Menu de Login");
        

        // Aplica o estado inicial ao Audio Mixer
        UpdateMusicVolume();
        UpdateSFXVolume();
    }

    // --- NOVO MÉTODO: Reage ao Carregamento de Cena ---
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Se a cena carregada for o mapa principal
        if (scene.name == nomeCenaMapaPrincipal)
        {
            // Reativa o Menu Principal
            ShowLoginMenu();
        }
        // Se não for o mapa principal, é um minigame
        else
        {
            // Ativa apenas HUD e desativa o resto
            ShowMinigameUI();
        }
    }

    public void ShowMinigameUI()
    {
        // Desativa tudo
        loginMenuPanel.SetActive(false);
        creditosPanel.SetActive(false);
        multiplayerMenuPanel.SetActive(false);
        finalizadoPanel.SetActive(false);

        // Mostra HUD apenas
        hudDoJogoPanel.SetActive(true);
        pausePanel.SetActive(false);
        fundoConfigII.SetActive(false);

        Time.timeScale = 1f;
    }


    // --- Métodos de Transição de Tela (Mostrar) ---

    // 1. LOGIN MENU
    public void ShowLoginMenu()
    {
        // Desativa todos os painéis (boa prática)
        DeactivateAllPanels();
        //// Ativa apenas o painel desejado
        //Debug.Log("Painel de Login Ativo");
        loginMenuPanel.SetActive(true);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Login");

    }

    // 2. CRÉDITOS
    public void ShowCreditos()
    {
        DeactivateAllPanels();
        // O wireframe mostra o "LOGIN CRÉDITOS", então assumimos que volta pro Login.
        creditosPanel.SetActive(true);
    }

    // 3. MULTIPLAYER MENU
    public void ShowMultiplayerMenu()
    {
        DeactivateAllPanels();
        multiplayerMenuPanel.SetActive(true);
    }

    // 4. HUD DO JOGO (Normalmente ativa com o carregamento da cena do jogo)
    public void ShowHUD()
    {
        DeactivateAllPanels();
        hudDoJogoPanel.SetActive(true);
        //SceneManager.LoadScene("Rooms");
    }

    private bool isPaused = false; // Variável para rastrear o estado

    // 5. PAUSA
    public void ShowPauseMenu()
    {
        // Não desativamos os outros, apenas mostramos a Pausa por cima do HUD/Jogo
        pausePanel.SetActive(true);
        fundoConfigII.SetActive(false);
        hudDoJogoPanel.SetActive(false);
        // Opcional: Pausar o tempo do jogo
        //Time.timeScale = 0f;

        isPaused = true;
        Debug.Log("Jogo Pausado. Pressione ESC para retomar.");
    }


    public void HidePauseMenu()
    {
        pausePanel.SetActive(false);
        hudDoJogoPanel.SetActive(true);
        // Opcional: Retomar o tempo do jogo
        Time.timeScale = 1f;

        isPaused = false;
        Debug.Log("Jogo Despausado.");
    }
    public void FundoConfig2()
    {
        fundoConfigII.SetActive(true);
    }

    // --- NOVO MÉTODO: Alterna o painel de Configurações DENTRO do Menu de Pausa ---
    public void ToggleConfigPause()
    {
        // 1. Verifica o estado atual do painel de Configurações
        bool isConfigActive = fundoConfigII.activeSelf;

        if (isConfigActive)
        {
            // A. Se estiver ativo, fecha as Configurações:
            fundoConfigII.SetActive(false);

            // O painel de pausa (pausePanel) JÁ ESTÁ ATIVO, então não precisamos ativá-lo novamente.
            // Se você quiser garantir que ele esteja ativo, pode deixar esta linha:
            // if (pausePanel != null) { pausePanel.SetActive(true); }

            Debug.Log("Configurações Fechadas e voltando para o Menu de Pausa.");
        }
        else
        {
            // B. Se estiver inativo, abre as Configurações:
            fundoConfigII.SetActive(true);

            // REMOVEMOS AQUI: Não desative o pausePanel.
            // O painel de configurações deve aparecer POR CIMA dele.
            // if (pausePanel != null) { pausePanel.SetActive(false); } // <-- REMOVA OU COMENTE ESTA LINHA!

            Debug.Log("Configurações Abertas.");
        }
    }

    // --- Funções de Áudio ---

    public void ToggleMusic()
    {
        // 1. Inverte o estado
        isMusicOn = !isMusicOn;

        // 2. Aplica o volume e atualiza o visual
        UpdateMusicVolume();

        Debug.Log("Música: " + (isMusicOn ? "Ligada" : "Desligada"));
    }

    public void ToggleSFX()
    {
        // 1. Inverte o estado
        isSfxOn = !isSfxOn;

        // 2. Aplica o volume e atualiza o visual
        UpdateSFXVolume();

        Debug.Log("SFX: " + (isSfxOn ? "Ligado" : "Desligado"));
    }

    // --- Métodos de Aplicação de Volume ---

    private void UpdateMusicVolume()
    {
        if (isMusicOn)
        {
            // APLICAÇÃO VISUAL: Seta a Sprite de Ligado
            if (musicButtonImage != null && musicOnSprite != null)
                musicButtonImage.sprite = musicOnSprite;

            // APLICAÇÃO DE ÁUDIO: Volume total (0dB)
            mainMixer.SetFloat("MusicVolume", 0f);
        }
        else
        {
            // APLICAÇÃO VISUAL: Seta a Sprite de Desligado
            if (musicButtonImage != null && musicOffSprite != null)
                musicButtonImage.sprite = musicOffSprite;

            // APLICAÇÃO DE ÁUDIO: Mute (-80dB)
            mainMixer.SetFloat("MusicVolume", -80f);
        }
    }

    // ... (Seu método ToggleSFX() permanece o mesmo)

    private void UpdateSFXVolume()
    {
        if (isSfxOn)
        {
            // APLICAÇÃO VISUAL: Seta a Sprite de Ligado
            if (sfxButtonImage != null && sfxOnSprite != null)
                sfxButtonImage.sprite = sfxOnSprite;

            // APLICAÇÃO DE ÁUDIO: Volume total (0dB)
            mainMixer.SetFloat("SFXVolume", 0f);
        }
        else
        {
            // APLICAÇÃO VISUAL: Seta a Sprite de Desligado
            if (sfxButtonImage != null && sfxOffSprite != null)
                sfxButtonImage.sprite = sfxOffSprite;

            // APLICAÇÃO DE ÁUDIO: Mute (-80dB)
            mainMixer.SetFloat("SFXVolume", -80f);
        }
    }


    // --- Método Auxiliar para Desativar Tudo ---
    void DeactivateAllPanels()
    {
        loginMenuPanel.SetActive(false);
        creditosPanel.SetActive(false);
        multiplayerMenuPanel.SetActive(false);
        hudDoJogoPanel.SetActive(false);
        pausePanel.SetActive(false);
        finalizadoPanel.SetActive(false); // Se aplicável
        // Certifique-se de que o tempo não está pausado se estivermos no menu principal
        Time.timeScale = 1f;
    }

    // --- Funções de Navegação de Cena ---

    // Botão HOME (Voltar ao Mapa Principal)
    public void GoToMainMap()
    {
        if (isPaused)
            HidePauseMenu();

        SceneManager.LoadScene(nomeCenaMapaPrincipal);
    }

    // --- Métodos de Funcionalidade ---

    // **IMPORTANTE: Botão Sair/Fechar o Jogo**
    // (O último botão no LOGIN MENU)
    public void QuitGame()
    {
        Debug.Log("Saindo do Jogo...");

        // Para jogos rodando no Editor da Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // Para jogos compilados
#else
            Application.Quit();
#endif
    }

    // --- Método para Mostrar a Tela Final ---
    public void ShowFinalScreen(GameResult result)
    {
        DeactivateAllPanels();
        finalizadoPanel.SetActive(true);

        switch (result)
        {
            case GameResult.Promocao:
                resultadoText.text = "PROMOÇÃO";
                // Cor da Faixa (Exemplo: Verde)
                faixaImage.color = new Color32(50, 200, 50, 255);
                break;

            case GameResult.Excelente:
                resultadoText.text = "EXCELENTE!";
                // Cor da Faixa (Exemplo: Amarelo/Ouro)
                faixaImage.color = new Color32(255, 215, 0, 255);
                break;

            case GameResult.Miseravel:
                resultadoText.text = "MISERÁVEL";
                // Cor da Faixa (Exemplo: Laranja)
                faixaImage.color = new Color32(255, 165, 0, 255);
                break;

            case GameResult.Rebaixado:
                resultadoText.text = "REBAIXADO";
                // Cor da Faixa (Exemplo: Vermelho)
                faixaImage.color = new Color32(200, 50, 50, 255);
                break;
        }

        // Opcional: Pausar o jogo se o HUD estiver ativo
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Se estiver pausado (isPaused == true), CHAMA O MÉTODO DE DESPAUSA
                HidePauseMenu();
            }
            else
            {
                // Se NÃO estiver pausado (isPaused == false), CHAMA O MÉTODO DE PAUSA
                ShowPauseMenu();
            }
        }

    }
}