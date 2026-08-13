using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;


// Esta classe guarda as informações
// necessárias para cada fase.
//
// Ela aparecerá dentro da lista
// no Inspector da Unity.
[System.Serializable]
public class DadosFase
{
    [Tooltip("Nome apenas para facilitar a identificação no Inspector.")]
    public string nomeFase;

    [Tooltip("GameObject que contém os elementos desta fase.")]
    public GameObject objetoFase;

    [Tooltip("Local onde o Player aparecerá ao entrar nesta fase.")]
    public Transform pontoEntrada;

    [Tooltip("Cinemachine Camera utilizada nesta fase.")]
    public CinemachineCamera cameraFase;
}


public class GerenciadorFases : MonoBehaviour
{
    [Header("Lista de fases")]

    [Tooltip("Adicione aqui todas as fases existentes na cena.")]
    [SerializeField]
    private List<DadosFase> fases =
        new List<DadosFase>();


    [Tooltip("Índice da fase onde o jogo começa. A primeira fase é 0.")]
    [SerializeField] private int faseInicial = 0;


    [Header("Player")]

    [Tooltip("Arraste aqui o Player.")]
    [SerializeField] private Transform player;


    [Header("Fade da tela")]

    [Tooltip("CanvasGroup responsável pelo fade.")]
    [SerializeField] private CanvasGroup painelFade;

    [Tooltip("Velocidade para escurecer e clarear a tela.")]
    [SerializeField] private float velocidadeFade = 2f;

    [Tooltip("Tempo que a tela ficará totalmente escura durante a troca de fase.")]
    [SerializeField] private float tempoTelaEscura = 1f;


    [Header("Prioridade das câmeras")]

    [Tooltip("Prioridade da câmera da fase ativa.")]
    [SerializeField] private int prioridadeAlta = 20;

    [Tooltip("Prioridade das câmeras das fases inativas.")]
    [SerializeField] private int prioridadeBaixa = 0;


    // Guarda qual fase está ativa atualmente.
    private int faseAtual;

    // Evita duas trocas ao mesmo tempo.
    private bool trocandoFase = false;


    private void Start()
    {
        // Verifica se alguma fase foi cadastrada.
        if (fases == null || fases.Count == 0)
        {
            Debug.LogError(
                "GerenciadorFases: nenhuma fase foi adicionada na lista.",
                this
            );

            return;
        }


        // Verifica se o número da fase inicial é válido.
        if (faseInicial < 0 || faseInicial >= fases.Count)
        {
            Debug.LogError(
                "GerenciadorFases: o índice da fase inicial é inválido.",
                this
            );

            return;
        }


        // Guarda qual é a fase inicial.
        faseAtual = faseInicial;


        // Percorre todas as fases cadastradas.
        for (int i = 0; i < fases.Count; i++)
        {
            // A fase inicial ficará ativa.
            bool deveFicarAtiva =
                i == faseInicial;


            // Ativa somente a fase inicial.
            if (fases[i].objetoFase != null)
            {
                fases[i].objetoFase.SetActive(
                    deveFicarAtiva
                );
            }


            // Configura a prioridade das câmeras.
            if (fases[i].cameraFase != null)
            {
                if (deveFicarAtiva)
                {
                    fases[i].cameraFase.Priority =
                        prioridadeAlta;
                }
                else
                {
                    fases[i].cameraFase.Priority =
                        prioridadeBaixa;
                }
            }
        }


        // Garante que a tela comece transparente.
        if (painelFade != null)
        {
            painelFade.alpha = 0f;
        }
    }


    // Este método poderá ser chamado pelos portais.
    //
    // Exemplos:
    //
    // IrParaFase(1) = Fase02
    // IrParaFase(2) = Fase03
    // IrParaFase(3) = Fase04
    //
    // Lembre-se:
    // a lista começa no índice 0.
    public void IrParaFase(int indiceDestino)
    {
        // Não permite iniciar outra troca
        // enquanto uma já estiver acontecendo.
        if (trocandoFase)
        {
            return;
        }


        // Verifica se a fase solicitada existe.
        if (
            indiceDestino < 0 ||
            indiceDestino >= fases.Count
        )
        {
            Debug.LogError(
                "GerenciadorFases: índice da fase de destino inválido.",
                this
            );

            return;
        }


        // Se já estamos nessa fase,
        // não precisamos fazer nada.
        if (indiceDestino == faseAtual)
        {
            return;
        }


        // Inicia a troca.
        StartCoroutine(
            TrocarFase(indiceDestino)
        );
    }


    private IEnumerator TrocarFase(
        int indiceDestino
    )
    {
        // Informa que uma troca começou.
        trocandoFase = true;


        // Guarda as informações da fase
        // para onde o Player vai.
        DadosFase faseDestino =
            fases[indiceDestino];


        // Guarda também a fase atual.
        DadosFase faseAnterior =
            fases[faseAtual];


        // --------------------------------
        // 1. ESCURECE A TELA
        // --------------------------------

        yield return StartCoroutine(
            FazerFade(1f)
        );


        // --------------------------------
        // 2. ATIVA A NOVA FASE
        // --------------------------------

        if (faseDestino.objetoFase != null)
        {
            faseDestino.objetoFase.SetActive(true);
        }


        // --------------------------------
        // 3. GUARDA A POSIÇÃO DO PLAYER
        // --------------------------------

        Vector3 posicaoAnteriorPlayer =
            player.position;


        // --------------------------------
        // 4. TELEPORTA O PLAYER
        // --------------------------------

        if (faseDestino.pontoEntrada != null)
        {
            player.position =
                faseDestino.pontoEntrada.position;
        }


        // --------------------------------
        // 5. CALCULA O TELEPORTE
        // --------------------------------

        Vector3 deslocamentoPlayer =
            player.position -
            posicaoAnteriorPlayer;


        // --------------------------------
        // 6. AVISA O CINEMACHINE
        // --------------------------------

        CinemachineCore.OnTargetObjectWarped(
            player,
            deslocamentoPlayer
        );


        // --------------------------------
        // 7. MUDA AS PRIORIDADES
        // DAS CÂMERAS
        // --------------------------------

        for (int i = 0; i < fases.Count; i++)
        {
            if (fases[i].cameraFase == null)
            {
                continue;
            }


            // A câmera da nova fase
            // recebe prioridade alta.
            if (i == indiceDestino)
            {
                fases[i].cameraFase.Priority =
                    prioridadeAlta;
            }

            // As outras recebem prioridade baixa.
            else
            {
                fases[i].cameraFase.Priority =
                    prioridadeBaixa;
            }
        }


        // --------------------------------
        // 8. DESATIVA A FASE ANTERIOR
        // --------------------------------

        if (faseAnterior.objetoFase != null)
        {
            faseAnterior.objetoFase.SetActive(false);
        }


        // --------------------------------
        // 9. ATUALIZA A FASE ATUAL
        // --------------------------------

        faseAtual =
            indiceDestino;


        // --------------------------------
        // 10. ESPERA UM FRAME
        // --------------------------------

        // Dá tempo para o Cinemachine
        // atualizar a posição da Main Camera.
        yield return null;


        // --------------------------------
        // 11. REINICIA O PARALAXE
        // --------------------------------

        if (faseDestino.objetoFase != null)
        {
            FundoParalaxeInfinito[] fundos =
                faseDestino.objetoFase
                .GetComponentsInChildren
                <FundoParalaxeInfinito>(true);


            foreach (
                FundoParalaxeInfinito fundo
                in fundos
            )
            {
                fundo.ReiniciarReferencia();
            }
        }


        // --------------------------------
        // 12. ESPERA MAIS UM FRAME
        // --------------------------------

        yield return null;


        // --------------------------------
        // 13. MANTÉM A TELA ESCURA
        // --------------------------------

        if (tempoTelaEscura > 0f)
        {
            yield return new WaitForSeconds(
                tempoTelaEscura
            );
        }


        // --------------------------------
        // 14. CLAREIA A TELA
        // --------------------------------

        yield return StartCoroutine(
            FazerFade(0f)
        );


        // A troca terminou.
        trocandoFase = false;
    }


    private IEnumerator FazerFade(
        float valorFinal
    )
    {
        // Se nenhum painel foi configurado,
        // não tenta executar o fade.
        if (painelFade == null)
        {
            yield break;
        }


        // Continua enquanto o alpha
        // não chegar ao valor desejado.
        while (
            Mathf.Abs(
                painelFade.alpha -
                valorFinal
            ) > 0.01f
        )
        {
            // Aproxima o alpha
            // do valor desejado.
            painelFade.alpha =
                Mathf.MoveTowards(
                    painelFade.alpha,
                    valorFinal,
                    velocidadeFade *
                    Time.deltaTime
                );


            // Espera o próximo frame.
            yield return null;
        }


        // Garante o valor exato.
        painelFade.alpha =
            valorFinal;
    }
}