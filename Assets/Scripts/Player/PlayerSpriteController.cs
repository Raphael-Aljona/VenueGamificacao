using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    [Header("Idle")]
    public Sprite[] idleDown;
    public Sprite[] idleUp;
    public Sprite[] idleRight;
    public Sprite[] idleLeft;

    [Header("Walk")]
    public Sprite[] walkDown;
    public Sprite[] walkUp;
    public Sprite[] walkRight;
    public Sprite[] walkLeft;

    [Header("Push (only right/left)")]
    public Sprite[] pushRight;
    public Sprite[] pushLeft;

    [Header("Interact (only right/left)")]
    public Sprite[] interactRight;
    public Sprite[] interactLeft;

    [Header("Config")]
    public float tempoEntreFrames = 0.12f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private Sprite[] animAtual;
    private int frameAtual;
    private float contadorTempo;

    private enum Estado { Idle, Walk, Push, Interact }
    private Estado estado = Estado.Idle;

    private enum Direcao { Down, Up, Right, Left }
    private Direcao ultimaDirecao = Direcao.Down;
    private Direcao direcaoAtual = Direcao.Down;

    private Vector2 movimentoInput;

    private PhotonView view;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();

        SetAnimation(GetIdleArray(ultimaDirecao));

        // 🟢 Força sincronização inicial (com leve atraso)
        StartCoroutine(WaitForPhotonView());
    }

    IEnumerator WaitForPhotonView()
    {
        // espera até o PhotonView estar pronto
        yield return new WaitUntil(() => view != null && view.ViewID != 0 && view.IsMine);

        // força sincronização inicial para todos os clientes
        view.RPC(nameof(RPC_SyncAnim), RpcTarget.AllBuffered, (int)estado, (int)direcaoAtual);
    }


    void Update()
    {
        if (!view.IsMine) return;

        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");
        movimentoInput = new Vector2(movX, movY).normalized;

        if (movimentoInput.magnitude > 0)
        {
            if (Mathf.Abs(movX) > Mathf.Abs(movY))
                direcaoAtual = movX > 0 ? Direcao.Right : Direcao.Left;
            else
                direcaoAtual = movY > 0 ? Direcao.Up : Direcao.Down;
        }

        Estado novoEstado =
            Input.GetKey(KeyCode.E) ? Estado.Interact :
            (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? Estado.Push :
            (movimentoInput.magnitude > 0) ? Estado.Walk :
            Estado.Idle;

        if (novoEstado != estado || direcaoAtual != ultimaDirecao)
        {
            estado = novoEstado;
            ultimaDirecao = direcaoAtual;
            view.RPC(nameof(RPC_SyncAnim), RpcTarget.All, (int)estado, (int)direcaoAtual);
        }

        AtualizarAnimacaoTempo();
    }

    [PunRPC]
    void RPC_SyncAnim(int estadoRPC, int direcaoRPC)
    {
        estado = (Estado)estadoRPC;
        ultimaDirecao = (Direcao)direcaoRPC;
        AtualizarAnimacaoPorEstado();
    }
    private void AtualizarAnimacaoPorEstado()
    {
        frameAtual = 0;
        contadorTempo = 0f;

        switch (estado)
        {
            case Estado.Idle:
                SetAnimation(GetIdleArray(ultimaDirecao));
                break;
            case Estado.Walk:
                SetAnimation(GetWalkArray(ultimaDirecao));
                break;
            case Estado.Push:
                SetAnimation(ultimaDirecao == Direcao.Left ? pushLeft : pushRight);
                break;
            case Estado.Interact:
                SetAnimation(ultimaDirecao == Direcao.Left ? interactLeft : interactRight);
                break;
        }
    }

    private void SetAnimation(Sprite[] novoArray)
    {
        animAtual = (novoArray != null && novoArray.Length > 0) ? novoArray : null;
        frameAtual = 0;
        contadorTempo = 0f;
        AtualizarSpriteImediato();
    }

    private void AtualizarAnimacaoTempo()
    {
        if (animAtual == null || animAtual.Length == 0) return;

        contadorTempo += Time.deltaTime;
        if (contadorTempo >= tempoEntreFrames)
        {
            contadorTempo = 0f;
            frameAtual++;
            if (frameAtual >= animAtual.Length) frameAtual = 0;
            sr.sprite = animAtual[frameAtual];
        }
    }

    private void AtualizarSpriteImediato()
    {
        if (animAtual == null || animAtual.Length == 0) return;
        if (frameAtual >= animAtual.Length) frameAtual = 0;
        sr.sprite = animAtual[frameAtual];
    }

    private Sprite[] GetIdleArray(Direcao d)
    {
        switch (d)
        {
            case Direcao.Down: return idleDown;
            case Direcao.Up: return idleUp;
            case Direcao.Right: return idleRight;
            case Direcao.Left: return idleLeft;
            default: return idleDown;
        }
    }

    private Sprite[] GetWalkArray(Direcao d)
    {
        switch (d)
        {
            case Direcao.Down: return walkDown;
            case Direcao.Up: return walkUp;
            case Direcao.Right: return walkRight;
            case Direcao.Left: return walkLeft;
            default: return walkDown;
        }
    }
}
