using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float velocidade = 10f;
    public float tempoDeVida = 0.3f;
    public int dano = 1;

    private Rigidbody2D rig;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rig.linearVelocity = transform.right * velocidade;
        Destroy(gameObject, tempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D colidir)
    {
        if (colidir.CompareTag("Player"))
        {
            return;
        }

        if (colidir.CompareTag("inimigo"))
        {
            VidaInimigo vidaInimigo = colidir.GetComponent<VidaInimigo>();

            if (vidaInimigo != null)
            {
                vidaInimigo.ReceberDano(dano);
            }

            Destroy(gameObject);
        }
    }
}