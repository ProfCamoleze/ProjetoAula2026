using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorLoad : MonoBehaviour
{

    [Header("Imagens")]
    public Image imgLoadAtual;
    public Image imgLoadProxima;

    [Header("Transparência das Imagens")]
    public CanvasGroup grupoAtual;
    public CanvasGroup grupoProxima;

    [Header("Imagens da Sequência")]
    public Sprite[] imagensLoad;

    [Header("Barra de Progresso")]
    public Slider barraProgresso;

    [Header("Fade Final")]
    public CanvasGroup painelFade;

    [Header("Configurações")]
    public string nomeCenaJogo = "Jogo";
    public float tempoEntreImagens = 2f;
    public float velocidadeTransicao = 2f;
    public float velocidadeFade = 2f;

    private void Start()
    {
        Time.timeScale = 1f;

        PrepararTela();

        StartCoroutine(CarregarCena());
    }

    private void PrepararTela()
    {
        if (grupoAtual != null)
            grupoAtual.alpha = 1f;

        if (grupoProxima != null)
            grupoProxima.alpha = 0f;

        if (painelFade != null)
            painelFade.alpha = 0f;

        if (barraProgresso != null)
            barraProgresso.value = 0f;

        if (imagensLoad.Length > 0 && imgLoadAtual != null)
            imgLoadAtual.sprite = imagensLoad[0];
    }

    private IEnumerator CarregarCena()
    {
        AsyncOperation carregamento = SceneManager.LoadSceneAsync(nomeCenaJogo);

        carregamento.allowSceneActivation = false;

        yield return StartCoroutine(MostrarImagens());

        while (carregamento.progress < 0.9f)
        {
            yield return null;
        }

        if (barraProgresso != null)
            barraProgresso.value = 1f;

        yield return StartCoroutine(FazerFadeFinal());

        carregamento.allowSceneActivation = true;
    }

    private IEnumerator MostrarImagens()
    {
        int totalImagens = imagensLoad.Length;

        if (totalImagens == 0)
        {
            yield return new WaitForSeconds(1f);
            yield break;
        }

        for (int i = 0; i < totalImagens; i++)
        {
            if (i == 0)
            {
                imgLoadAtual.sprite = imagensLoad[i];
            }
            else
            {
                imgLoadProxima.sprite = imagensLoad[i];

                grupoProxima.alpha = 0f;

                yield return StartCoroutine(FazerTransicaoImagem());

                imgLoadAtual.sprite = imagensLoad[i];

                grupoAtual.alpha = 1f;
                grupoProxima.alpha = 0f;
            }

            if (barraProgresso != null)
                barraProgresso.value = (float)(i + 1) / totalImagens;

            yield return new WaitForSeconds(tempoEntreImagens);
        }
    }

    private IEnumerator FazerTransicaoImagem()
    {
        float tempo = 0f;

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * velocidadeTransicao;

            grupoProxima.alpha = Mathf.Lerp(0f, 1f, tempo);

            yield return null;
        }

        grupoProxima.alpha = 1f;
    }

    private IEnumerator FazerFadeFinal()
    {
        float tempo = 0f;

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * velocidadeFade;

            painelFade.alpha = Mathf.Lerp(0f, 1f, tempo);

            yield return null;
        }

        painelFade.alpha = 1f;
    }
}