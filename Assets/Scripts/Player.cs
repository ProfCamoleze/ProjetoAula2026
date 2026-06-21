using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
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

    [Header("Dash")]                          // >>> NOVO
    public float velocidadeDash = 18f;          // forca do impulso
    public float duracaoDash = 0.15f;           // duracao em segundos
    public float cooldownDash = 0.6f;           // tempo de recarga

    bool emDash = false;                        // esta em dash?
    bool podeDash = true;                        // pode dar dash?
    float contadorDash;                          // cronometro da duracao
    float contadorCooldown;                      // cronometro da recarga
    float gravidadeOriginal;                     // gravidade normal salva
    float direcaoDash = 1f;                      // 1 = direita, -1 = esquerda

    [Header("Ghost Trail (rastro)")]
    public Color corFantasma = new Color(0.4f, 0.7f, 1f, 0.6f); // cor azulada
    public float intervaloFantasma = 0.04f;  // tempo entre um fantasma e outro
    public float tempoSumirFantasma = 0.3f;  // quanto cada fantasma demora a sumir

    SpriteRenderer meuSprite;   // referencia ao SpriteRenderer do player
    float contadorFantasma;     // cronometro para criar o proximo fantasma


    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        controle = new PlayerControle();
        anime = GetComponent<Animator>();
        meuSprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() { controle.Enable(); }
    private void OnDisable() { controle.Disable(); }

    void Start()
    {
        gravidadeOriginal = rig.gravityScale;    // >>> NOVO: salva a gravidade
    }

    void Update()
    {
        mover = controle.Player.Move.ReadValue<Vector2>();
        animar();

        // verificar se ta no chao
        ehchao = Physics2D.OverlapCircle(checkChao.position, raioCheckChao, oqueEchao);

        // ----- PULO (igual antes) -----
        if (controle.Player.Jump.WasPressedThisFrame() && qtdPulos > 1)
        {
            qtdPulos--;
            pular();
        }
        else if (ehchao)
        {
            qtdPulos = 2f;
        }

        // ----- VIRAR + guardar direcao do dash -----
        if (mover.x > 0.01f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            direcaoDash = 1f;                    // >>> NOVO
        }
        else if (mover.x < -0.01f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            direcaoDash = -1f;                   // >>> NOVO
        }

        // ----- DASH: detecta o botao Sprint ----- >>> NOVO
        if (controle.Player.Sprint.WasPressedThisFrame() && podeDash && !emDash)
        {
            iniciarDash();
        }

        // ----- DASH: conta a duracao ----- >>> NOVO
        if (emDash)
        {
            // Diminui o tempo restante do dash
            contadorDash -= Time.deltaTime;

            // Conta o tempo para soltar o próximo fantasma
            contadorFantasma -= Time.deltaTime;

            if (contadorFantasma <= 0f)
            {
                criarFantasma();
                contadorFantasma = intervaloFantasma;
            }

            // Quando o tempo terminar, encerra o dash
            if (contadorDash <= 0f)
            {
                terminarDash();
            }

        }

        // ----- DASH: conta o cooldown ----- >>> NOVO
        if (!podeDash && !emDash)
        {
            contadorCooldown -= Time.deltaTime;
            if (contadorCooldown <= 0f) podeDash = true;
        }
    }

    void FixedUpdate()
    {
        // durante o dash NAO controlamos a velocidade pelo input,
        // deixamos a forca do dash agir. >>> NOVO
        if (!emDash)
        {
            rig.linearVelocityX = mover.x * velocidade;
        }
    }

    void pular()
    {
        rig.linearVelocityY = 0f;
        rig.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
    }

    // >>> NOVO: comeca o dash
    void iniciarDash()
    {
        emDash = true;
        podeDash = false;
        contadorDash = duracaoDash;
        contadorFantasma = 0f;   // >>> NOVO: solta o 1o fantasma na hora

        rig.gravityScale = 0f;
        rig.linearVelocity = Vector2.zero;
        rig.AddForce(new Vector2(direcaoDash * velocidadeDash, 0f),
                     ForceMode2D.Impulse);

    }

    // >>> NOVO: termina o dash e arma o cooldown
    void terminarDash()
    {
        emDash = false;

        // Recupera a gravidade normal
        rig.gravityScale = gravidadeOriginal;

        // Interrompe o deslocamento horizontal do dash
        rig.linearVelocityX = 0f;

        // Inicia o tempo de recarga
        contadorCooldown = cooldownDash;
    }

    void criarFantasma()
    {
        // 1. cria um objeto vazio na cena
        GameObject fantasma = new GameObject("Fantasma");

        // 2. coloca o fantasma na MESMA posicao e rotacao do player
        fantasma.transform.position = transform.position;
        fantasma.transform.rotation = transform.rotation;
        fantasma.transform.localScale = transform.localScale;

        // 3. adiciona um SpriteRenderer e copia o sprite atual do player
        SpriteRenderer srFantasma = fantasma.AddComponent<SpriteRenderer>();
        srFantasma.sprite = meuSprite.sprite;          // mesmo desenho
        srFantasma.sortingLayerID = meuSprite.sortingLayerID;
        srFantasma.sortingOrder = meuSprite.sortingOrder - 1; // atras do player
        srFantasma.color = corFantasma;                // cor azulada

        // 4. cola o script Fantasma para ele sumir sozinho
        Fantasma scriptFantasma = fantasma.AddComponent<Fantasma>();
        scriptFantasma.tempoParaSumir = tempoSumirFantasma;
    }

    void animar()
    {
        anime.SetFloat("andar", Mathf.Abs(rig.linearVelocityX));
        if (ehchao)
        {
            anime.SetBool("pular", false);
            anime.SetBool("cair", false);
        }
        else
        {
            if (rig.linearVelocityY > 0.1f)
            {
                anime.SetBool("pular", true);
                anime.SetBool("cair", false);
            }
            else if (rig.linearVelocityY < -0.1f)
            {
                anime.SetBool("pular", false);
                anime.SetBool("cair", true);
            }
        }
    }
}
