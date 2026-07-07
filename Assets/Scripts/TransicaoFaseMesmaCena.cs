using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TransicaoFaseMesmaCena : MonoBehaviour
{
  /*  [Header("Pain�is")]
    public CanvasGroup painelTransicao;
    public CanvasGroup painelFadePreto;

    [Header("Conte�do da transi��o")]
    public Image imagemTransicao;
    public Sprite[] imagensTransicao;

    public TextMeshProUGUI textoTransicao;

    [TextArea]
    public string[] textosTransicao;

    public TextMeshProUGUI textoDica;

    [TextArea]
    public string[] dicas;

    [Header("Tempos")]
    public float tempoMostrandoTransicao = 3f;
    public float velocidadeFade = 1.5f;
    [Header("Som")]
    public AudioSource fonteAudio;
    public AudioClip somTransicao;
    public AudioClip somFimTransicao;


    private void Start()
    {
        PrepararPainel(painelTransicao, false);
        PrepararPainel(painelFadePreto, false);
    }

    public void IniciarTransicao(Transform player, Transform pontoDestino)
    {
        StartCoroutine(RotinaTrocarFase(player, pontoDestino));
    }

    private IEnumerator RotinaTrocarFase(Transform player, Transform pontoDestino)
    {
        TocarSom(somTransicao);

        MostrarImagemAleatoria();
        MostrarTextoAleatorio();
        MostrarDicaAleatoria();

        PrepararPainel(painelTransicao, true);
        painelTransicao.alpha = 0f;

        yield return StartCoroutine(FazerFade(painelTransicao, 0f, 1f));

        yield return new WaitForSeconds(tempoMostrandoTransicao);

        PrepararPainel(painelFadePreto, true);
        painelFadePreto.alpha = 0f;

        yield return StartCoroutine(FazerFade(painelFadePreto, 0f, 1f));

        if (player != null && pontoDestino != null)
        {
            player.position = pontoDestino.position;
        }

        TocarSom(somFimTransicao);

        yield return StartCoroutine(FazerFade(painelTransicao, 1f, 0f));
        PrepararPainel(painelTransicao, false);

        yield return StartCoroutine(FazerFade(painelFadePreto, 1f, 0f));
        PrepararPainel(painelFadePreto, false);
    }

    private void MostrarImagemAleatoria()
    {
        if (imagemTransicao != null && imagensTransicao.Length > 0)
        {
            int indice = Random.Range(0, imagensTransicao.Length);
            imagemTransicao.sprite = imagensTransicao[indice];
        }
    }

    private void MostrarTextoAleatorio()
    {
        if (textoTransicao != null && textosTransicao.Length > 0)
        {
            int indice = Random.Range(0, textosTransicao.Length);
            textoTransicao.text = textosTransicao[indice];
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (textoDica != null && dicas.Length > 0)
        {
            int indice = Random.Range(0, dicas.Length);
            textoDica.text = dicas[indice];
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
  
    // Update is called once per frame
    void Update()
    {
        if (fonteAudio != null && som != null)
        {
            fonteAudio.PlayOneShot(som);
        }
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

    */
}


