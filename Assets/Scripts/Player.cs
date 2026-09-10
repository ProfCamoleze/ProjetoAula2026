using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    public bool podeMover = true;

    Rigidbody2D rig;
    Vector2 mover;
    PlayerControle controle;
    Animator anime;


    [Header("Pulo")]
    public float forcaPulo = 6f;
    public float qtdPulos = 2f;

    bool ehchao;

    public Transform checkChao;
    public float raioCheckChao = 0.2f;
    public LayerMask oqueEchao;


    // Direção atual do personagem.
    // 1 = direita
    // -1 = esquerda
    public float Direcao { get; private set; } = 1f;


    // Outros scripts podem bloquear temporariamente
    // o movimento normal do Player.
    public bool movimentoBloqueado = false;


    private void Awake()
    {
        // Localiza os componentes do Player.
        rig = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        // Cria os controles do Input System.
        controle = new PlayerControle();
    }


    private void OnEnable()
    {
        // Ativa os controles.
        controle.Enable();
    }


    private void OnDisable()
    {
        // Desativa os controles.
        controle.Disable();
    }


    void Update()
    {
        // Lê a movimentação do jogador.
        if (podeMover)
        {
            mover = controle.Player.Move.ReadValue<Vector2>();


            // Verifica se o Player está tocando o chão.
            ehchao = Physics2D.OverlapCircle(
                checkChao.position,
                raioCheckChao,
                oqueEchao
            );


            // -------------------------
            // PULO
            // -------------------------

            if (controle.Player.Jump.WasPressedThisFrame() && qtdPulos > 1)
            {
                qtdPulos--;

                pular();
            }
            else if (ehchao)
            {
                qtdPulos = 2f;
            }
        }

        // -------------------------
        // DIREÇÃO DO PLAYER
        // -------------------------

        if (mover.x > 0.01f)
        {
            // Vira para a direita.
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            Direcao = 1f;
        }
        else if (mover.x < -0.01f)
        {
            // Vira para a esquerda.
            transform.rotation =    Quaternion.Euler(0f, 180f, 0f);

            Direcao = -1f;
        }


        // Atualiza as animações.
        animar();
    }


    void FixedUpdate()
    {
        // Se outro sistema estiver controlando
        // o movimento, não alteramos a velocidade.
        if (movimentoBloqueado && !podeMover)
        {
            rig.linearVelocityX = 0f; rig.linearVelocityY = 0f;
            return;
        }


        // Movimento horizontal normal.
        rig.linearVelocityX =
            mover.x * velocidade;
    }

    public void bloquearMovimento()
    {
        podeMover = false;
        mover = Vector2.zero;
        rig.linearVelocityX = 0f;
    }

    public void liberarMovimento()
    {
        podeMover = true;
    }

    void pular()
    {
        // Remove a velocidade vertical anterior.
        rig.linearVelocityY = 0f;

        // Aplica o impulso do pulo.
        rig.AddForce(
            Vector2.up * forcaPulo,
            ForceMode2D.Impulse
        );
    }


    void animar()
    {
        // Animação de caminhada.
        anime.SetFloat(
            "andar",
            Mathf.Abs(rig.linearVelocityX)
        );


        // Está no chão.
        if (ehchao)
        {
            anime.SetBool("pular", false);
            anime.SetBool("cair", false);
        }
        else
        {
            // Está subindo.
            if (rig.linearVelocityY > 0.1f)
            {
                anime.SetBool("pular", true);
                anime.SetBool("cair", false);
            }

            // Está caindo.
            else if (rig.linearVelocityY < -0.1f)
            {
                anime.SetBool("pular", false);
                anime.SetBool("cair", true);
            }
        }
    }
}