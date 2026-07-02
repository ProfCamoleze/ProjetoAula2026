using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ControladorLoad : MonoBehaviour
{
    [Header("Painéis")]
    public CanvasGroup painelLoad;
    public CanvasGroup painelFadePreto;

    [Header("Conteúdo do Load")]
    public Image imagemLoad;
    public TextMeshProUGUI textoLoad;

    public Sprite[] imagensLoad;

    [TextArea]
    public string[] textosLoad;

    [Header("Barra de Progresso")]
    public Slider barraProgresso;

    [Header("Configurações")]
    public float tempoMinimoDeLoad = 3f;
    public float velocidadeFade = 2f;

    private void Start()
    {
        PrepararPainel(painelLoad, true);
        PrepararPainel(painelFadePreto, true);

        painelFadePreto.alpha = 0f;

        StartCoroutine(RotinaCarregarCena());
    }

    private IEnumerator RotinaCarregarCena()
    {
        string nomeProximaCena = PlayerPrefs.GetString("ProximaCena", "Jogo");

        MostrarConteudoAleatorio();

        AsyncOperation carregamento = SceneManager.LoadSceneAsync(nomeProximaCena);

        if (carregamento == null)
        {
            Debug.LogError("Cena não encontrada: " + nomeProximaCena);
            yield break;
        }

        carregamento.allowSceneActivation = false;

        float tempoAtual = 0f;

        while (tempoAtual < tempoMinimoDeLoad || carregamento.progress < 0.9f)
        {
            tempoAtual += Time.deltaTime;

            if (barraProgresso != null)
            {
                barraProgresso.value = Mathf.Clamp01(carregamento.progress / 0.9f);
            }

            yield return null;
        }

        if (barraProgresso != null)
        {
            barraProgresso.value = 1f;
        }

        yield return StartCoroutine(FazerFade(painelFadePreto, 0f, 1f));

        carregamento.allowSceneActivation = true;
    }

    private void MostrarConteudoAleatorio()
    {
        if (imagemLoad != null && imagensLoad.Length > 0)
        {
            int indiceImagem = Random.Range(0, imagensLoad.Length);
            imagemLoad.sprite = imagensLoad[indiceImagem];
        }

        if (textoLoad != null && textosLoad.Length > 0)
        {
            int indiceTexto = Random.Range(0, textosLoad.Length);
            textoLoad.text = textosLoad[indiceTexto];
        }
    }

    private IEnumerator FazerFade(CanvasGroup painel, float inicio, float fim)
    {
        if (painel == null)
        {
            yield break;
        }

        float tempo = 0f;
        painel.alpha = inicio;

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * velocidadeFade;
            painel.alpha = Mathf.Lerp(inicio, fim, tempo);

            yield return null;
        }

        painel.alpha = fim;
    }

    private void PrepararPainel(CanvasGroup painel, bool mostrar)
    {
        if (painel == null)
        {
            return;
        }

        if (mostrar)
        {
            painel.alpha = 1f;
            painel.interactable = true;
            painel.blocksRaycasts = true;
        }
        else
        {
            painel.alpha = 0f;
            painel.interactable = false;
            painel.blocksRaycasts = false;
        }
    }
}