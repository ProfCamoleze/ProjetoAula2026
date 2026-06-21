using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class VidaPlayer : MonoBehaviour
{
    [Header("Configuração da Vida")]
    public int vidaMaxima = 3;
    public int vidaAtual;

    [Header("Imagens dos Corações")]
    public Image[] imagensCoracoes;

    [Header("Sprites dos Corações")]
    public Sprite coracaoCheio;
    public Sprite coracaoVazio;

    [Header("Configuração de Dano")]
    public float tempoInvencivel = 1f;

    private bool estaInvencivel = false;

    [Header("Efeito Visual Opcional")]
    public SpriteRenderer spritePlayer;

    [Header("Game OVER")]
    public GameObject gameOver;

    private void Start()
    {
        vidaAtual = vidaMaxima;
        AtualizarCorações();
        Time.timeScale = 1f; // Certifique-se de que o tempo está normal no início do jogo
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("armadilha") )
        {
            TomarDano(1);
        }
        // detectar poção ganha via
         if (collision.CompareTag("pocao"))
            {
                 GanharVida(1, collision.gameObject);
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("armadilha") || collision.gameObject.CompareTag("inimigo"))
        {
            TomarDano(1);
        }
    }

    public void TomarDano(int dano)
    {
        if (estaInvencivel == true)
        {
            return;
        }

        vidaAtual -= dano;

        if (vidaAtual < 0)
        {
            vidaAtual = 0;
        }

        AtualizarCorações();

        if (vidaAtual <= 0)
        {
            Morrer();
        }
        else
        {
            StartCoroutine(InvencibilidadeTemporaria());
        }
    }

    private void AtualizarCorações()
    {
        for (int i = 0; i < imagensCoracoes.Length; i++)
        {
            if (i < vidaAtual)
            {
                imagensCoracoes[i].sprite = coracaoCheio;
            }
            else
            {
                imagensCoracoes[i].sprite = coracaoVazio;
            }
        }
    }

    private IEnumerator InvencibilidadeTemporaria()
    {
        estaInvencivel = true;

        float tempoPiscando = 0f;

        while (tempoPiscando < tempoInvencivel)
        {
            if (spritePlayer != null)
            {
                spritePlayer.enabled = false;
            }

            yield return new WaitForSeconds(0.1f);

            if (spritePlayer != null)
            {
                spritePlayer.enabled = true;
            }

            yield return new WaitForSeconds(0.1f);

            tempoPiscando += 0.2f;
        }

        if (spritePlayer != null)
        {
            spritePlayer.enabled = true;
        }

        estaInvencivel = false;
    }

    private void Morrer()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f; // Pausa o jogo
    }

    public void restart()
    {
        Time.timeScale = 1f; // Reinicia o tempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarrega a cena atual
    }

    // Ganhar vida
     public void GanharVida(int quantidade, GameObject pocao)
    {
         if (vidaAtual >= vidaMaxima)
             {
               return;
             }

        vidaAtual += quantidade;

         if (vidaAtual > vidaMaxima)
            {
                vidaAtual +=1;
            }

     AtualizarCorações();

     Destroy(pocao);
   }

    
    }