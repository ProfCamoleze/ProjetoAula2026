using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    Rigidbody2D rig;
    Vector2 mover;
    PlayerControle controles;

    [Header("Pulo")]
    public float forcaPulo = 6f;
    public int maximoPulos = 2;
    public int pulosRestantes;

    [Header("Chão (Ground Check)")]
    public Transform groundCheck;
    public float raioGroundCheck = 0.2f;
    public LayerMask layerGround;
    private bool estaNoChao;

    Animator anim;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        controles = new PlayerControle();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        controles.Enable();
    }

    void OnDisable()
    {
        controles.Disable();
    }

    void Update()
    {
        mover = controles.Player.Move.ReadValue<Vector2>();

        estaNoChao = Physics2D.OverlapCircle(
            groundCheck.position,
            raioGroundCheck,
            layerGround
        );

        if (controles.Player.Jump.WasPressedThisFrame())
        {
            Pular();
        }

        if (estaNoChao)
        {
            pulosRestantes = maximoPulos;
        }

        if (mover.x > 0.01f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (mover.x < -0.01f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        AtualizarAnimacoes();
    }

    private void FixedUpdate()
    {
        rig.linearVelocityX = mover.x * velocidade;
    }

    private void Pular()
    {
        pulosRestantes--;

        if (pulosRestantes > 0)
        {
            rig.linearVelocityY = 0f;
            rig.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
        }
    }

    private void AtualizarAnimacoes()
    {
        anim.SetFloat("andar", Mathf.Abs(rig.linearVelocity.x));

        if (estaNoChao  )
        {
            anim.SetBool("pular", false);
            anim.SetBool("cair", false);
        }
        else
        {
            if (rig.linearVelocity.y > 0.1f)
            {
                anim.SetBool("pular", true);
                anim.SetBool("cair", false);
            }
            else if (rig.linearVelocity.y < -0.1f)
            {
                anim.SetBool("pular", false);
                anim.SetBool("cair", true);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, raioGroundCheck);
    }
}
