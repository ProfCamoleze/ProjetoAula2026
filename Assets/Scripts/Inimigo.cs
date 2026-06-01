using UnityEngine;

// Garante que o objeto possua um Rigidbody2D.
// Caso o componente não exista, a Unity adicionará automaticamente.

public class Inimigo: MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Configurações de movimento")]
    [SerializeField] private float velocidade = 2f;
    [SerializeField] private float distanciaParaParar = 0.6f;

    [Header("Campo de visão")]
    [SerializeField] private float raioVisao = 5f;

    [Header("Retorno ao ponto inicial")]
    [SerializeField] private float margemOrigem = 0.1f;

    // Componente responsável pela física do inimigo.
    private Rigidbody2D rig;

    // Guarda o ponto onde o inimigo estava quando a fase começou.
    private Vector2 posicaoInicial;

    private void Awake()
    {
        // Localiza o Rigidbody2D do próprio inimigo.
        rig = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Salva a posição inicial do inimigo.
        posicaoInicial = rig.position;
    }

    private void FixedUpdate()
    {
        // Evita erro caso o Player não tenha sido conectado no Inspector.
        if (player == null)
        {
            Parar();
            return;
        }

        // Converte a posição do Player para Vector2.
        Vector2 posicaoPlayer = player.position;

        // Mede a distância entre o inimigo e o Player.
        float distanciaDoPlayer = Vector2.Distance(rig.position, posicaoPlayer);

        // Decide qual comportamento será executado.
        if (distanciaDoPlayer <= raioVisao)
        {
            PerseguirPlayer(posicaoPlayer);
        }
        else
        {
            VoltarParaOrigem();
        }
    }

    private void PerseguirPlayer(Vector2 posicaoPlayer)
    {
        // Mede somente a diferença horizontal entre inimigo e Player.
        float distanciaHorizontal = Mathf.Abs(posicaoPlayer.x - rig.position.x);

        // Evita que o inimigo fique empurrando ou tremendo próximo ao Player.
        if (distanciaHorizontal <= distanciaParaParar)
        {
            Parar();
            return;
        }

        // Retorna -1 para esquerda ou 1 para direita.
        float direcao = Mathf.Sign(posicaoPlayer.x - rig.position.x);

        Movimentar(direcao);
    }

    private void VoltarParaOrigem()
    {
        // Mede a distância horizontal entre o inimigo e sua posição inicial.
        float distanciaHorizontal = Mathf.Abs(posicaoInicial.x - rig.position.x);

        // Para o inimigo quando ele estiver próximo da origem.
        if (distanciaHorizontal <= margemOrigem)
        {
            Parar();
            return;
        }

        // Descobre se a origem está à esquerda ou à direita.
        float direcao = Mathf.Sign(posicaoInicial.x - rig.position.x);

        Movimentar(direcao);
    }

    private void Movimentar(float direcao)
    {
        // Altera apenas a velocidade horizontal.
        // A velocidade vertical continua sendo controlada pela gravidade.
        rig.linearVelocityX = direcao * velocidade;

        AtualizarLadoDoSprite(direcao);
    }

    private void Parar()
    {
        // Para apenas o movimento horizontal.
        // O inimigo continua sujeito à gravidade.
        rig.linearVelocityX = 0f;
    }

    private void AtualizarLadoDoSprite(float direcao)
    {
        // Executa somente se o SpriteRenderer foi conectado.
        if (spriteRenderer != null)
        {
            // Esta configuração considera que a arte original olha para a direita.
            spriteRenderer.flipX = direcao > 0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha um círculo amarelo no editor para visualizar o campo de visão.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioVisao);
    }
}