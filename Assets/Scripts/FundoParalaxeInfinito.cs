using UnityEngine;

// Este script deve ser colocado no objeto PAI
// de cada camada do fundo.
//
// Exemplo:
// Fundo01_MuitoDistante
// Fundo02_Distante
// Fundo03_Medio
// Fundo04_Proximo
// Fundo05_MuitoProximo

public class FundoParalaxeInfinito : MonoBehaviour
{
    [Header("Referências")]

    [Tooltip("Arraste aqui a Main Camera do jogo.")]
    [SerializeField] private Camera cameraDoJogo;

    [Tooltip("Arraste os três painéis desta camada: esquerda, centro e direita.")]
    [SerializeField] private SpriteRenderer[] paineis;


    [Header("Paralaxe Horizontal")]

    [Tooltip("Quanto maior o valor, mais a camada acompanha a câmera e mais distante ela parece.")]
    [Range(0f, 1f)]
    [SerializeField] private float fatorX = 0.8f;


    [Header("Paralaxe Vertical")]

    [Tooltip("Ative se quiser que esta camada também acompanhe o movimento vertical da câmera.")]
    [SerializeField] private bool usarParalaxeY = false;

    [Tooltip("Quanto maior o valor, mais a camada acompanha a câmera no eixo Y.")]
    [Range(0f, 1f)]
    [SerializeField] private float fatorY = 0.8f;


    // Posição da câmera quando o jogo começa.
    private Vector3 posicaoInicialCamera;

    // Posição desta camada quando o jogo começa.
    private Vector3 posicaoInicialCamada;

    // Largura de um painel no mundo.
    private float larguraPainel;

    // Quantidade total de painéis.
    private int quantidadePaineis;


    private void Start()
    {
        PrepararCamera();

        if (!enabled)
        {
            return;
        }

        PrepararPaineis();

        if (!enabled)
        {
            return;
        }

        // Guarda a posição inicial da câmera.
        posicaoInicialCamera = cameraDoJogo.transform.position;

        // Guarda a posição inicial desta camada.
        posicaoInicialCamada = transform.position;
    }


    private void PrepararCamera()
    {
        // Caso nenhuma câmera tenha sido arrastada,
        // procura automaticamente pela Main Camera.
        if (cameraDoJogo == null)
        {
            cameraDoJogo = Camera.main;
        }


        // Se ainda não encontrou uma câmera,
        // o script não pode continuar.
        if (cameraDoJogo == null)
        {
            Debug.LogError(
                "FundoParalaxeInfinito: nenhuma Main Camera foi encontrada.",
                this
            );

            enabled = false;

            return;
        }


        // Este sistema foi criado para câmera Orthographic.
        if (!cameraDoJogo.orthographic)
        {
            Debug.LogError(
                "FundoParalaxeInfinito: a Main Camera precisa estar em modo Orthographic.",
                this
            );

            enabled = false;
        }
    }


    private void PrepararPaineis()
    {
        // Verifica se o array existe.
        if (paineis == null || paineis.Length == 0)
        {
            Debug.LogError(
                "FundoParalaxeInfinito: nenhum painel foi configurado.",
                this
            );

            enabled = false;

            return;
        }


        // Para este sistema precisamos de pelo menos três painéis.
        if (paineis.Length < 3)
        {
            Debug.LogError(
                "FundoParalaxeInfinito: utilize pelo menos três painéis.",
                this
            );

            enabled = false;

            return;
        }


        // Verifica se o primeiro painel existe.
        if (paineis[0] == null)
        {
            Debug.LogError(
                "FundoParalaxeInfinito: o primeiro painel não foi configurado.",
                this
            );

            enabled = false;

            return;
        }


        // Descobre automaticamente a largura
        // real ocupada pelo primeiro sprite no mundo.
        larguraPainel = paineis[0].bounds.size.x;


        // Guarda quantos painéis existem.
        quantidadePaineis = paineis.Length;


        // Verifica todos os elementos do array.
        foreach (SpriteRenderer painel in paineis)
        {
            if (painel == null)
            {
                Debug.LogError(
                    "FundoParalaxeInfinito: existe um painel vazio no Inspector.",
                    this
                );

                enabled = false;

                return;
            }
        }
    }


    private void LateUpdate()
    {
        // Primeiro movimentamos a camada
        // para criar o efeito de paralaxe.
        AtualizarParalaxe();


        // Depois verificamos se algum painel
        // saiu completamente da câmera.
        AtualizarFundoInfinito();
    }


    private void AtualizarParalaxe()
    {
        // Descobre quanto a câmera se deslocou
        // desde o começo do jogo.
        Vector3 deslocamentoCamera =
            cameraDoJogo.transform.position -
            posicaoInicialCamera;


        // Calcula o deslocamento horizontal da camada.
        float movimentoX =
            deslocamentoCamera.x * fatorX;


        // Por padrão não existe movimento vertical.
        float movimentoY = 0f;


        // Se o paralaxe vertical estiver ativado,
        // calcula também o eixo Y.
        if (usarParalaxeY)
        {
            movimentoY =
                deslocamentoCamera.y * fatorY;
        }


        // Move o objeto-pai da camada.
        transform.position =
            posicaoInicialCamada +
            new Vector3(
                movimentoX,
                movimentoY,
                0f
            );
    }


    private void AtualizarFundoInfinito()
    {
        // Descobre a metade da altura
        // que a câmera consegue enxergar.
        float metadeAlturaCamera =
            cameraDoJogo.orthographicSize;


        // A largura depende da altura
        // e da proporção atual da câmera.
        float metadeLarguraCamera =
            metadeAlturaCamera *
            cameraDoJogo.aspect;


        // Descobre a borda esquerda visível.
        float limiteEsquerdoCamera =
            cameraDoJogo.transform.position.x -
            metadeLarguraCamera;


        // Descobre a borda direita visível.
        float limiteDireitoCamera =
            cameraDoJogo.transform.position.x +
            metadeLarguraCamera;


        // Verifica cada painel separadamente.
        foreach (SpriteRenderer painel in paineis)
        {
            RepetirPainel(
                painel,
                limiteEsquerdoCamera,
                limiteDireitoCamera
            );
        }
    }


    private void RepetirPainel(
        SpriteRenderer painel,
        float limiteEsquerdoCamera,
        float limiteDireitoCamera
    )
    {
        // Distância total ocupada por todos
        // os painéis lado a lado.
        float distanciaReposicionamento =
            larguraPainel *
            quantidadePaineis;


        // Se a borda direita do painel estiver
        // à esquerda da câmera,
        // significa que ele saiu completamente da tela.
        while (painel.bounds.max.x < limiteEsquerdoCamera)
        {
            painel.transform.position +=
                Vector3.right *
                distanciaReposicionamento;
        }


        // Se a borda esquerda do painel estiver
        // à direita da câmera,
        // significa que ele saiu completamente da tela.
        while (painel.bounds.min.x > limiteDireitoCamera)
        {
            painel.transform.position -=
                Vector3.right *
                distanciaReposicionamento;
        }
    }
}