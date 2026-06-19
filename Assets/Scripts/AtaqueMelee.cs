using UnityEngine;
using System.Collections;

public class AtaqueMelee : MonoBehaviour
{
    [Header("Referências")]
    public Animator anim;                 // Animator do Player (opcional)
    public BoxCollider2D colisorAtaque;   // O BoxCollider2D do objeto AreaAtaque

    [Header("Configuração do Ataque")]
    public int dano = 1;
    public float tempoEntreAtaques = 0.3f;
    public float duracaoDoAtaque = 0.15f;

    private PlayerControle controle;
    private bool atacando = false;        // o golpe está acontecendo agora?
    private float proximoAtaque = 0f;     // controla o tempo entre golpes

    private void Awake()
    {
        controle = new PlayerControle();

        // Começa com a área de ataque DESLIGADA.
        // Ela só liga durante o golpe.
        if (colisorAtaque != null)
        {
            colisorAtaque.enabled = false;
        }
    }

    private void OnEnable() { 
        controle.Enable();
    }
    private void OnDisable() { 
        controle.Disable(); 
    }

    private void Update()
    {
        if (controle.Player.Attack.WasPressedThisFrame())
        {
            TentarAtacar();
        }
    }

    private void TentarAtacar()
    {
        // Ainda está no tempo de espera entre golpes?
        if (Time.time < proximoAtaque) return;

        // Já está atacando?
        if (atacando == true) return;

        StartCoroutine(Atacar());
        proximoAtaque = Time.time + tempoEntreAtaques;
    }

    private IEnumerator Atacar()
    {
        atacando = true;

        if (anim != null)
        {
            anim.SetTrigger("atacar");
        }

        // LIGA a área de ataque (agora ela pode detectar inimigos)
        if (colisorAtaque != null)
        {
            colisorAtaque.enabled = true;
        }

        // Espera a duração do golpe
        yield return new WaitForSeconds(duracaoDoAtaque);

        // DESLIGA a área de ataque
        if (colisorAtaque != null)
        {
            colisorAtaque.enabled = false;
        }

        atacando = false;
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        Debug.Log("Colidiu com: " + outro.name);
        // Só causa dano em quem tem a tag "inimigo"
        if (outro.CompareTag("inimigo"))
        {
            Debug.Log("Colidiu com: " + outro.name);
            // Busca a vida DO INIMIGO atingido (não a do Player)
            VidaInimigo2 vidaInimigo = outro.GetComponent<VidaInimigo2>();

            if (vidaInimigo != null)
            {
                vidaInimigo.TomarDano(dano, transform.position);
                Debug.Log("Acertei o inimigo: " + outro.name);
            }
        }
    }
}