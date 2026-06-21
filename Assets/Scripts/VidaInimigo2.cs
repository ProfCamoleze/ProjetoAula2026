using UnityEngine;
using System.Collections;

public class VidaInimigo2 : MonoBehaviour
{
    [Header("Configuração de Vida")]
    public int vidaMaxima = 3;

    [Header("Configuração do Knockback")]
    public float forcaKnockback = 6f;
    public float tempoKnockback = 0.25f;

    private int vidaAtual;
    private Rigidbody2D rig;
    private Inimigo movimentoInimigo;
    private bool recebendoKnockback = false;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        movimentoInimigo = GetComponent<Inimigo>();
    }

    private void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void TomarDano(int dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    public void TomarDano(int dano, Vector2 posicaoDoAtaque)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            Morrer();
            return;
        }

        AplicarKnockback(posicaoDoAtaque);
    }

    private void AplicarKnockback(Vector2 posicaoDoAtaque)
    {
        if (rig == null)
        {
            return;
        }

        if (recebendoKnockback == true)
        {
            return;
        }

        Vector2 direcaoKnockback = ((Vector2)transform.position - posicaoDoAtaque).normalized;
        direcaoKnockback.y = 0.2f;

        StartCoroutine(Knockback(direcaoKnockback));
    }

    private IEnumerator Knockback(Vector2 direcaoKnockback)
    {
        recebendoKnockback = true;

        if (movimentoInimigo != null)
        {
            movimentoInimigo.enabled = false;
        }

        rig.linearVelocity = Vector2.zero;
        rig.AddForce(direcaoKnockback * forcaKnockback, ForceMode2D.Impulse);

        yield return new WaitForSeconds(tempoKnockback);

        rig.linearVelocityX = 0f;

        if (movimentoInimigo != null)
        {
            movimentoInimigo.enabled = true;
        }

        recebendoKnockback = false;
    }

    private void Morrer()
    {
        Destroy(gameObject);
    }
}