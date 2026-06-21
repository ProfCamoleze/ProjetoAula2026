using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float velocidade = 10f;
    public float tempoDeVida= 3f;
    public int dano = 1;
    Rigidbody2D rig;
    Vector2 direcao;
     void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Atirar(Vector2 novaDirecao)
    {
        direcao = novaDirecao.normalized;
        rig.linearVelocity = direcao * velocidade;
        if (direcao.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else
        {
              transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
     private void OnTriggerEnter2D(Collider2D colidir)
    {
        if (colidir.CompareTag("Player"))
        {
            return;
        }
        if (colidir.CompareTag("inimigo"))
        {
            VidaInimigo vidaInimigo=colidir.GetComponent<VidaInimigo>();
            if (vidaInimigo != null)
            {
                vidaInimigo.ReceberDano(dano);
            }
            Destroy(gameObject);
        }
    }
}
