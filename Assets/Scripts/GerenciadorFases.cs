using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;


[System.Serializable]
public class DadosFase
{
    public GameObject objetoFase;
    public Transform pontoEntrada;
    public CinemachineCamera cameraFase;
}

public class GerenciadorFases : MonoBehaviour
{
    [Header("Fases")]
    [SerializeField] private List<DadosFase> fases = new List<DadosFase>();
    [SerializeField] private int faseInicial = 0;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Fade")]
    [SerializeField] private CanvasGroup painelFade;
    [SerializeField] private float velocidadeFade = 2f;
    [SerializeField] private float tempoTelaEscura = 1f;

    [Header("Câmeras")]
    [SerializeField] private int prioridadeAlta = 20;
    [SerializeField] private int prioridadeBaixa = 0;

    private int faseAtual;
    private bool trocandoFase;


    private void Start()
    {
        faseAtual = faseInicial;

        for (int i = 0; i < fases.Count; i++)
        {
            if (i == faseInicial)
            {
                fases[i].objetoFase.SetActive(true);
                fases[i].cameraFase.Priority = prioridadeAlta;
            }
            else
            {
                fases[i].objetoFase.SetActive(false);
                fases[i].cameraFase.Priority = prioridadeBaixa;
            }
        }
        painelFade.alpha = 0f;
    }


    public void IrParaFase(int indiceDestino)
    {
        if (trocandoFase)
        {
            return;
        }
        if (indiceDestino == faseAtual)
        {
            return;
        }
        StartCoroutine(TrocarFase(indiceDestino));
    }


    private IEnumerator TrocarFase(int indiceDestino)
    {
        trocandoFase = true;
        DadosFase faseAnterior = fases[faseAtual];
        DadosFase faseDestino = fases[indiceDestino];
        yield return StartCoroutine(FazerFade(1f));
        faseDestino.objetoFase.SetActive(true);
        Vector3 posicaoAnteriorPlayer = player.position;
        player.position = faseDestino.pontoEntrada.position;
        Vector3 deslocamentoPlayer = player.position - posicaoAnteriorPlayer;
        CinemachineCore.OnTargetObjectWarped(player, deslocamentoPlayer);

        for (int i = 0; i < fases.Count; i++)
        {
            if (i == indiceDestino)
            {
                fases[i].cameraFase.Priority = prioridadeAlta;
            }
            else
            {
                fases[i].cameraFase.Priority = prioridadeBaixa;
            }
        }
        faseAnterior.objetoFase.SetActive(false);
        faseAtual = indiceDestino;
        yield return null;
        FundoParalaxeInfinito[] fundos = faseDestino.objetoFase.GetComponentsInChildren<FundoParalaxeInfinito>(true);

        foreach (FundoParalaxeInfinito fundo in fundos)
        {
            fundo.ReiniciarReferencia();
        }
        yield return null;
        if (tempoTelaEscura > 0f)
        {
            yield return new WaitForSeconds(tempoTelaEscura);
        }
        yield return StartCoroutine(FazerFade(0f));
        trocandoFase = false;
    }


    private IEnumerator FazerFade(float valorFinal)
    {
        while (Mathf.Abs(painelFade.alpha - valorFinal) > 0.01f)
        {
            painelFade.alpha = Mathf.MoveTowards(painelFade.alpha, valorFinal, velocidadeFade * Time.deltaTime);
            yield return null;
        }
        painelFade.alpha = valorFinal;
    }
}