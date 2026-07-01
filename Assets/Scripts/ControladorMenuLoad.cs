using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorMenuLoad : MonoBehaviour
{
    [Header("Painel de Abertura")]
    public CanvasGroup painelAbertura;
    public Image imagemAbertura;
    public TextMeshProUGUI textoAbertura;

    public Sprite[] imagensAbertura;

    [TextArea]
    public string[] textosAbertura;

    public float tempoPorImagem = 2f;

    [Header("Painel do Menu")]
    public CanvasGroup painelMenu;

    [Header("Fade")]
    public CanvasGroup painelFadePreto;
    public float velocidadeFade = 1.5f;

    [Header("Botão Pular")]
    public Button botaoPularAbertura;

    [Header("Ícone Girando")]
    public Transform iconeGirando;
    public float velocidadeGiro = 180f;

    [Header("Sons")]
    public AudioSource fonteAudio;
    public AudioClip somAbertura;
    public AudioClip somTrocaImagem;
    public AudioClip somMenuAparecendo;

    [Header("Animação do Logo")]
    public Animator animacaoLogo;
    public string nomeTriggerAparecer = "Aparecer";

    private bool pulouAbertura = false;
    private bool podeGirar = false;

    private void Start()
    {
        Time.timeScale = 1f;

        PrepararPainel(painelAbertura, true);
        PrepararPainel(painelMenu, false);
        PrepararPainel(painelFadePreto, false);

        if (botaoPularAbertura != null)
        {
            botaoPularAbertura.onClick.AddListener(PularAbertura);
        }

        TocarSom(somAbertura);

        StartCoroutine(RotinaAberturaMenu());
    }

    private void Update()
    {
        if (podeGirar && iconeGirando != null)
        {
            iconeGirando.Rotate(0f, 0f, -velocidadeGiro * Time.deltaTime);
        }
    }

    private IEnumerator RotinaAberturaMenu()
    {
        podeGirar = true;

        int quantidade = Mathf.Max(imagensAbertura.Length, textosAbertura.Length);

        for (int i = 0; i < quantidade; i++)
        {
            if (imagemAbertura != null && i < imagensAbertura.Length)
            {
                imagemAbertura.sprite = imagensAbertura[i];
            }

            if (textoAbertura != null && i < textosAbertura.Length)
            {
                textoAbertura.text = textosAbertura[i];
            }

            TocarSom(somTrocaImagem);

            float tempoAtual = 0f;

            while (tempoAtual < tempoPorImagem && pulouAbertura == false)
            {
                tempoAtual += Time.deltaTime;
                yield return null;
            }

            if (pulouAbertura)
            {
                break;
            }
        }

        yield return StartCoroutine(FazerFade(painelAbertura, 1f, 0f));

        PrepararPainel(painelAbertura, false);

        PrepararPainel(painelMenu, true);
        painelMenu.alpha = 0f;

        TocarSom(somMenuAparecendo);

        yield return StartCoroutine(FazerFade(painelMenu, 0f, 1f));

        TocarAnimacaoLogo();

        podeGirar = false;
    }

    public void PularAbertura()
    {
        pulouAbertura = true;
    }

    private void TocarAnimacaoLogo()
    {
        if (animacaoLogo != null)
        {
            animacaoLogo.SetTrigger(nomeTriggerAparecer);
        }
    }

    private void TocarSom(AudioClip som)
    {
        if (fonteAudio != null && som != null)
        {
            fonteAudio.PlayOneShot(som);
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