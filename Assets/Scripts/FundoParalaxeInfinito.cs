using UnityEngine;

public class FundoParalaxeInfinito : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Camera cameraDoJogo;
    [SerializeField] private SpriteRenderer[] paineis;

    [Header("Paralaxe Horizontal")]
    [Range(0f, 1f)]
    [SerializeField] private float fatorX = 0.8f;

    [Header("Paralaxe Vertical")]
    [SerializeField] private bool usarParalaxeY = false;

    [Range(0f, 1f)]
    [SerializeField] private float fatorY = 0.8f;

    private Vector3 posicaoInicialCamera;
    private Vector3 posicaoInicialCamada;
    private Vector3 posicaoLocalOriginalCamada;
    private Vector3[] posicoesLocaisOriginaisPaineis;

    private float distanciaReposicionamento;
    private void Awake()
    {
        posicaoLocalOriginalCamada = transform.localPosition;

        posicoesLocaisOriginaisPaineis = new Vector3[paineis.Length];

        for (int i = 0; i < paineis.Length; i++)
        {
            posicoesLocaisOriginaisPaineis[i] = paineis[i].transform.localPosition;
        }
        distanciaReposicionamento = paineis[0].bounds.size.x * paineis.Length;
    }

    private void Start()
    {
        ReiniciarReferencia(false);
    }

    private void LateUpdate()
    {
        Vector3 deslocamentoCamera = cameraDoJogo.transform.position - posicaoInicialCamera;
        float movimentoX = deslocamentoCamera.x * fatorX;
        float movimentoY = 0f;

        if (usarParalaxeY)
        {
            movimentoY = deslocamentoCamera.y * fatorY;
        }

        transform.position = posicaoInicialCamada + new Vector3(movimentoX, movimentoY, 0f);

        float metadeLarguraCamera = cameraDoJogo.orthographicSize * cameraDoJogo.aspect;

        float limiteEsquerdoCamera = cameraDoJogo.transform.position.x - metadeLarguraCamera;

        float limiteDireitoCamera = cameraDoJogo.transform.position.x + metadeLarguraCamera;

        foreach (SpriteRenderer painel in paineis)
        {
            while (painel.bounds.max.x < limiteEsquerdoCamera)
            {
                painel.transform.position += Vector3.right * distanciaReposicionamento;
            }
            while (painel.bounds.min.x > limiteDireitoCamera)
            {
                painel.transform.position -= Vector3.right * distanciaReposicionamento;
            }
        }
    }

    public void ReiniciarReferencia(bool restaurarPosicoes = true)
    {
        if (restaurarPosicoes)
        {
            transform.localPosition = posicaoLocalOriginalCamada;

            for (int i = 0; i < paineis.Length; i++)
            {
                paineis[i].transform.localPosition = posicoesLocaisOriginaisPaineis[i];
            }
        }
        posicaoInicialCamera = cameraDoJogo.transform.position;
        posicaoInicialCamada = transform.position;
    }
}