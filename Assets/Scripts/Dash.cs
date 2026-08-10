using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Dash")]
    public float velocidadeDash = 18f;
    public float duracaoDash = 0.15f;
    public float cooldownDash = 0.6f;


    [Header("Ghost Trail")]
    public Color corFantasma = new Color(0.4f, 0.7f, 1f, 0.6f);
    public float intervaloFantasma = 0.04f;
    public float tempoSumirFantasma = 0.3f;

    Rigidbody2D rig;
    SpriteRenderer meuSprite;
    Player player;

    PlayerControle controle;

    bool emDash = false;
    bool podeDash = true;

    float contadorDash;
    float contadorCooldown;
    float contadorFantasma;

    float gravidadeOriginal;


    private void Awake()
    {
        // Componentes do próprio Player.
        rig = GetComponent<Rigidbody2D>();

        meuSprite = GetComponent<SpriteRenderer>();


        // Localiza o script Player.
        player = GetComponent<Player>();


        // Cria os controles.
        controle = new PlayerControle();
    }


    private void Start()
    {
        // Guarda a gravidade original.

        // Dessa maneira podemos colocar
        // qualquer Gravity Scale no Inspector
        // sem precisar escrever o mesmo valor aqui.
        gravidadeOriginal = rig.gravityScale;
    }


    private void OnEnable()
    {
        // Ativa o Input System.
        controle.Enable();
    }


    private void OnDisable()
    {
        // Desativa o Input System.
        controle.Disable();
    }


    void Update()
    {
        // -------------------------
        // INICIAR DASH
        // -------------------------

        if (controle.Player.Sprint.WasPressedThisFrame() && podeDash && !emDash)
        {
            iniciarDash();
        }


        // -------------------------
        // DASH EM EXECUÇÃO
        // -------------------------

        if (emDash)
        {
            // Diminui o tempo restante
            // do Dash.
            contadorDash -= Time.deltaTime;


            // Diminui o contador usado
            // para criar os fantasmas.
            contadorFantasma -= Time.deltaTime;


            // Está na hora de criar
            // outro fantasma.
            if (contadorFantasma <= 0f)
            {
                criarFantasma();
                contadorFantasma = intervaloFantasma;
            }


            // Tempo do Dash terminou.
            if (contadorDash <= 0f)
            {
                terminarDash();
            }
        }


        // -------------------------
        // COOLDOWN
        // -------------------------

        if (!podeDash && !emDash)
        {
            contadorCooldown -= Time.deltaTime;


            if (contadorCooldown <= 0f)
            {
                podeDash = true;
            }
        }
    }


    void iniciarDash()
    {
        // Marca que o Dash começou.
        emDash = true;

        // Impede outro Dash imediatamente.
        podeDash = false;


        // Define o tempo do Dash.
        contadorDash = duracaoDash;


        // O primeiro fantasma aparecerá
        // imediatamente.
        contadorFantasma = 0f;


        // Impede o Player.cs de alterar
        // a velocidade durante o Dash.
        player.movimentoBloqueado = true;


        // Remove temporariamente
        // a gravidade.
        rig.gravityScale = 0f;


        // Remove a velocidade anterior.
        rig.linearVelocity = Vector2.zero;


        // Aplica o impulso na direção
        // em que o Player está olhando.
        rig.AddForce(new Vector2(player.Direcao * velocidadeDash, 0f), ForceMode2D.Impulse);
    }


    void terminarDash()
    {
        // Dash terminou.
        emDash = false;


        // Libera novamente o movimento
        // controlado pelo Player.cs.
        player.movimentoBloqueado = false;


        // Recupera a gravidade original.
        rig.gravityScale = gravidadeOriginal;


        // Interrompe a velocidade
        // horizontal causada pelo Dash.
        rig.linearVelocityX = 0f;


        // Inicia o cooldown.
        contadorCooldown = cooldownDash;
    }


    void criarFantasma()
    {
        // -------------------------
        // 1. Criar o objeto
        // -------------------------

        GameObject fantasma =
            new GameObject("Fantasma");


        // -------------------------
        // 2. Copiar Transform
        // -------------------------

        fantasma.transform.position = transform.position;

        fantasma.transform.rotation = transform.rotation;

        fantasma.transform.localScale = transform.localScale;


        // -------------------------
        // 3. Criar SpriteRenderer
        // -------------------------

        SpriteRenderer srFantasma = fantasma.AddComponent<SpriteRenderer>();


        // Usa exatamente o sprite atual
        // do Player.
        srFantasma.sprite = meuSprite.sprite;


        // Mantém a mesma Sorting Layer.
        srFantasma.sortingLayerID = meuSprite.sortingLayerID;


        // Coloca o fantasma atrás
        // do Player.
        srFantasma.sortingOrder = meuSprite.sortingOrder - 1;


        // Define a aparência.
        srFantasma.color = corFantasma;


        // -------------------------
        // 4. Fazer desaparecer
        // -------------------------

        Fantasma scriptFantasma = fantasma.AddComponent<Fantasma>();


        scriptFantasma.tempoParaSumir = tempoSumirFantasma;
    }
}