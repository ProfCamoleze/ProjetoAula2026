using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movimento")] //cabeçalho
    public float velocidade = 5f;
    Rigidbody2D rig;
    Vector2 mover;
    PlayerControle controle;

    [Header("Pulo")]
    public float forcaPulo = 6f;

    bool ehChao; //gUarda SE ESTOU OU NÃO NO CHÃO
    public Transform ehPe; //guardar a posição do pé do personagem
    public LayerMask chao; //guarda a camada do chão
    public float raioPe = 0.2f; //guarda o raio do pé do personagem

    Animator anime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
       rig= GetComponent<Rigidbody2D>();
       anime= GetComponent<Animator>();
    }
    void Awake()
    {
        controle = new PlayerControle();
    }
    void OnEnable()
    {
        controle.Enable();
    }
    void OnDisable()
    {
        controle.Disable();
    }

    // Update is called once per frame
    void Update() //executa a cada frame
    {
        mover = controle.Player.Move.ReadValue<Vector2>();
        ehChao= Physics2D.OverlapCircle(ehPe.position, raioPe, chao);
      
        if (controle.Player.Jump.WasPressedThisFrame() && ehChao)
        {
           rig.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
        }

       

        if (mover.x > 0)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (mover.x < 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);  
        animar();
    }
    private void FixedUpdate() //executa em um valor fixo, 50 X / segundo
    {
        rig.linearVelocityX=mover.x * velocidade;
    }

    void animar()
    {
        anime.SetFloat("andar",Mathf.Abs(rig.linearVelocityX));
        if (ehChao)
        {
            anime.SetBool("pular", false);
            anime.SetBool("cair", false);
        }
        else
            if (rig.linearVelocity.y > 0.1f) 
            {
               anime.SetBool("pular", true);
                anime.SetBool("cair", false);
            }
            else if (rig.linearVelocity.y < -0.1f)
            {
                anime.SetBool("pular", false);
                anime.SetBool("cair", true);
            }
     }
    private void OnDrawGizmos()
    {
   
        // Linha azul do Player até o ponto do pé
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, ehPe.position);

        // Círculo amarelo no ponto do pé
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(ehPe.position, raioPe);
    }
}
