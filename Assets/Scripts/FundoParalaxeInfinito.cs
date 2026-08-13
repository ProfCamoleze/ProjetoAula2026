using UnityEngine;

// Coloque este script no objeto PAI
// de cada camada de fundo.
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

    [Tooltip("Arraste os três painéis desta camada.")]
    [SerializeField] private SpriteRenderer[] paineis;


    [Header("Paralaxe Horizontal")]

    [Tooltip("Quanto maior o valor, mais distante a camada parece.")]
    [Range(0f, 1f)]
    [SerializeField] private float fatorX = 0.8f;


    [Header("Paralaxe Vertical")]

    [Tooltip("Ative caso queira paralaxe também no eixo Y.")]
    [SerializeField] private bool usarParalaxeY = false;

    [Range(0f, 1f)]
    [SerializeField] private float fatorY = 0.8f;


    // Referência da câmera usada no cálculo do paralaxe.
    private Vector3 posicaoInicialCamera;

    // Posição inicial desta camada.
    private Vector3 posicaoInicialCamada;

    // Guarda a posição local original da camada.
    private Vector3 posicaoLocalOriginalCamada;

    // Guarda as posições locais originais dos painéis.
    private Vector3[] posicoesLocaisOriginaisPaineis;

    // Largura de um painel.
    private float larguraPainel;

    // Quantidade de painéis.
    private int quantidadePaineis;

    // Indica se o sistema já foi preparado.
    private bool preparado = false;


    private void Awake()
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

        // Guarda a posição LOCAL criada no Editor.
        posicaoLocalOriginalCamada = transform.localPosition;

        // Cria espaço para guardar a posição original dos painéis.
        posicoesLocaisOriginaisPaineis =
            new Vector3[paineis.Length];

        // Guarda a posição LOCAL original de cada painel.
        for (int i = 0; i < paineis.Length; i++)
        {
            posicoesLocaisOriginaisPaineis[i] =
                paineis[i].transform.localPosition;
        }

        preparado = true;
    }


    private void Start()
    {
        // Na fase inicial, apenas cria a referência normal.
        ReiniciarReferencia(false);
    }


    private void PrepararCamera()
    {
        // Caso nenhuma câmera tenha sido arrastada,
        // procura automaticamente pela Main Camera.
        if (cameraDoJogo == null)
        {
            cameraDoJogo = Camera.main;
        }

        if (cameraDoJogo == null)
        {
            Debug.LogError(
                "FundoParalaxeInfinito: nenhuma Main Camera foi encontrada.",
                this
            );

            enabled = false;

            return;
        }

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
        if (paineis == null || paineis.Length < 3)
        {
            Debug.LogError(
                "FundoParalaxeInfinito: configure pelo menos três painéis.",
                this
            );

            enabled = false;

            return;
        }

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

        // Descobre automaticamente a largura real do primeiro painel.
        larguraPainel = paineis[0].bounds.size.x;

        quantidadePaineis = paineis.Length;
    }


    private void LateUpdate()
    {
        if (!preparado)
        {
            return;
        }

        AtualizarParalaxe();

        AtualizarFundoInfinito();
    }


    private void AtualizarParalaxe()
    {
        // Calcula quanto a câmera se deslocou
        // desde a última referência.
        Vector3 deslocamentoCamera =
            cameraDoJogo.transform.position -
            posicaoInicialCamera;


        float movimentoX =
            deslocamentoCamera.x * fatorX;


        float movimentoY = 0f;

        if (usarParalaxeY)
        {
            movimentoY =
                deslocamentoCamera.y * fatorY;
        }


        // Move a camada a partir da referência da fase atual.
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
        // Metade da altura que a câmera enxerga.
        float metadeAlturaCamera =
            cameraDoJogo.orthographicSize;


        // Metade da largura visível da câmera.
        float metadeLarguraCamera =
            metadeAlturaCamera *
            cameraDoJogo.aspect;


        // Limite esquerdo da câmera.
        float limiteEsquerdoCamera =
            cameraDoJogo.transform.position.x -
            metadeLarguraCamera;


        // Limite direito da câmera.
        float limiteDireitoCamera =
            cameraDoJogo.transform.position.x +
            metadeLarguraCamera;


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
        float distanciaReposicionamento =
            larguraPainel *
            quantidadePaineis;


        // Saiu completamente pela esquerda.
        while (painel.bounds.max.x < limiteEsquerdoCamera)
        {
            painel.transform.position +=
                Vector3.right *
                distanciaReposicionamento;
        }


        // Saiu completamente pela direita.
        while (painel.bounds.min.x > limiteDireitoCamera)
        {
            painel.transform.position -=
                Vector3.right *
                distanciaReposicionamento;
        }
    }


    // Chamado pelo GerenciadorFases
    // quando o Player muda para outra região da mesma Scene.
    public void ReiniciarReferencia(bool restaurarPosicoes = true)
    {
        if (!preparado)
        {
            return;
        }


        // Ao trocar de fase, restaura a camada
        // para a posição configurada originalmente no Editor.
        if (restaurarPosicoes)
        {
            transform.localPosition =
                posicaoLocalOriginalCamada;


            // Restaura também os três painéis.
            for (int i = 0; i < paineis.Length; i++)
            {
                paineis[i].transform.localPosition =
                    posicoesLocaisOriginaisPaineis[i];
            }
        }


        // A posição ATUAL da câmera passa a ser
        // o novo ponto zero do paralaxe.
        posicaoInicialCamera =
            cameraDoJogo.transform.position;


        // Guarda onde a camada está agora.
        posicaoInicialCamada =
            transform.position;
    }
}