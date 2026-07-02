using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    [Header("Abertura")]
    public CanvasGroup grupoPainelAbertura;
    public CanvasGroup grupoImagemAbertura;
    public Image imagemAbertura;
    public Button botaoPularAbertura;

    [Header("Imagens da Abertura")]
    public Sprite[] imagensAbertura;

    [Header("Tempos da Abertura")]
    public float tempoImagemVisivel = 2f;
    public float velocidadeFadeImagem = 2f;
    public float velocidadeFadePainel = 2f;

    [Header("Menu Principal")]
    public CanvasGroup grupoPainelMenu;
    public Button botaoJogar;

    [Header("Cena que será carregada")]
    public string nomeCenaLoad = "Load";

    private bool pulouAbertura = false;
    private bool clicouJogar = false;

    private void Start()
    {
        // Garante que o jogo começa com o tempo normal.
        Time.timeScale = 1f;

        // Deixa a abertura visível no começo.
        PrepararPainel(grupoPainelAbertura, true);

        // Deixa o menu invisível no começo.
        PrepararPainel(grupoPainelMenu, false);

        // Garante que a imagem começa visível.
        if (grupoImagemAbertura != null)
        {
            grupoImagemAbertura.alpha = 1f;
        }

        // Liga o botão Pular ao método PularAbertura.
        if (botaoPularAbertura != null)
        {
            botaoPularAbertura.onClick.AddListener(PularAbertura);
        }

        // Liga o botão Jogar ao método Jogar.
        if (botaoJogar != null)
        {
            botaoJogar.onClick.AddListener(Jogar);
        }

        // Começa a sequência de imagens.
        StartCoroutine(RotinaAbertura());
    }

    public void PularAbertura()
    {
        // Quando o jogador clicar em Pular, essa variável muda para true.
        pulouAbertura = true;
    }

    public void Jogar()
    {
        // Evita clicar várias vezes no botão Jogar.
        if (clicouJogar == true)
        {
            return;
        }

        clicouJogar = true;

        // Carrega a cena Load.
        SceneManager.LoadScene(nomeCenaLoad);
    }

    private IEnumerator RotinaAbertura()
    {
        // Se não houver imagens, pula direto para o menu.
        if (imagensAbertura.Length == 0)
        {
            yield return StartCoroutine(MostrarMenu());
            yield break;
        }

        // Passa por todas as imagens cadastradas no Inspector.
        for (int i = 0; i < imagensAbertura.Length; i++)
        {
            // Troca a imagem atual.
            if (imagemAbertura != null)
            {
                imagemAbertura.sprite = imagensAbertura[i];
            }

            // Faz a imagem aparecer suavemente.
            yield return StartCoroutine(FazerFadeImagem(0f, 1f));

            // Mantém a imagem visível por alguns segundos.
            float tempoAtual = 0f;

            while (tempoAtual < tempoImagemVisivel && pulouAbertura == false)
            {
                tempoAtual += Time.deltaTime;
                yield return null;
            }

            // Se o jogador clicou em Pular, sai da repetição.
            if (pulouAbertura == true)
            {
                break;
            }

            // Faz a imagem apagar suavemente antes da próxima.
            yield return StartCoroutine(FazerFadeImagem(1f, 0f));
        }

        // Quando a abertura termina, mostra o menu.
        yield return StartCoroutine(MostrarMenu());
    }

    private IEnumerator MostrarMenu()
    {
        // Ativa o painel do menu ANTES de apagar a abertura.
        // Assim, quando a abertura ficar transparente, o menu já estará atrás dela.
        PrepararPainel(grupoPainelMenu, true);

        // Deixa o menu totalmente visível.
        grupoPainelMenu.alpha = 1f;

        // Por segurança, bloqueia os cliques no menu enquanto a abertura ainda está sumindo.
        grupoPainelMenu.interactable = false;
        grupoPainelMenu.blocksRaycasts = false;

        // Agora apagamos apenas o painel da abertura.
        yield return StartCoroutine(FazerFadePainel(grupoPainelAbertura, 1f, 0f));

        // Depois que a abertura sumiu, desativamos sua interação.
        PrepararPainel(grupoPainelAbertura, false);

        // Agora liberamos os cliques no menu.
        grupoPainelMenu.interactable = true;
        grupoPainelMenu.blocksRaycasts = true;
    }

    private IEnumerator FazerFadeImagem(float inicio, float fim)
    {
        if (grupoImagemAbertura == null)
        {
            yield break;
        }

        float tempo = 0f;
        grupoImagemAbertura.alpha = inicio;

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * velocidadeFadeImagem;
            grupoImagemAbertura.alpha = Mathf.Lerp(inicio, fim, tempo);

            yield return null;
        }

        grupoImagemAbertura.alpha = fim;
    }

    private IEnumerator FazerFadePainel(CanvasGroup painel, float inicio, float fim)
    {
        if (painel == null)
        {
            yield break;
        }

        float tempo = 0f;
        painel.alpha = inicio;

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * velocidadeFadePainel;
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

        if (mostrar == true)
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