using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class GerenciadorFases : MonoBehaviour
{
    [Header("Objetos das fases")]
    public GameObject fase01;
    public GameObject fase02;

    [Header("Player")]
    public Transform player;

    [Header("Pontos de entrada")]
    public Transform pontoEntradaFase01;
    public Transform pontoEntradaFase02;

    [Header("Câmeras Cinemachine")]
    public CinemachineCamera cameraFase01;
    public CinemachineCamera cameraFase02;

    [Header("Fade da tela")]
    public CanvasGroup painelFade;
    public float velocidadeFade = 2f;

    [Header("Prioridade das câmeras")]
    public int prioridadeAlta = 20;
    public int prioridadeBaixa = 0;

    private bool trocandoFase = false;

    private void Start()
    {
        // Garante que o jogo comece na Fase01.
        fase01.SetActive(true);
        fase02.SetActive(false);

        // Define a câmera inicial.
        cameraFase01.Priority = prioridadeAlta;
        cameraFase02.Priority = prioridadeBaixa;

        // Garante que a tela comece clara.
        if (painelFade != null)
        {
            painelFade.alpha = 0;
        }
    }

    public void IrParaFase02()
    {
        // Evita que a troca de fase aconteça várias vezes ao mesmo tempo.
        if (trocandoFase == false)
        {
            StartCoroutine(TrocarParaFase02());
        }
    }

    private IEnumerator TrocarParaFase02()
    {
        trocandoFase = true;

        // Escurece a tela antes da troca.
        yield return StartCoroutine(FazerFade(1));

        // Ativa a Fase02 antes de mover o Player.
        fase02.SetActive(true);

        // Move o Player para o ponto inicial da Fase02.
        player.position = pontoEntradaFase02.position;

        // Troca a prioridade das câmeras.
        cameraFase01.Priority = prioridadeBaixa;
        cameraFase02.Priority = prioridadeAlta;

        // Desativa a Fase01 depois que a Fase02 já está pronta.
        fase01.SetActive(false);

        // Clareia a tela depois da troca.
        yield return StartCoroutine(FazerFade(0));

        trocandoFase = false;
    }

    private IEnumerator FazerFade(float valorFinal)
    {
        // Enquanto o painel não chega no valor desejado...
        while (Mathf.Abs(painelFade.alpha - valorFinal) > 0.01f)
        {
            // Move o alpha suavemente até o valor final.
            painelFade.alpha = Mathf.MoveTowards(
                painelFade.alpha,
                valorFinal,
                velocidadeFade * Time.deltaTime
            );

            yield return null;
        }

        // Garante o valor exato no final.
        painelFade.alpha = valorFinal;
    }
}