using UnityEngine;

public class PlayerTop : MonoBehaviour
{

    [Header("Conf movimento")]
    public float velocidade;

    Rigidbody2D rig;
    PlayerControle controle;
    Vector2 mover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        controle = new PlayerControle();
        rig = GetComponent<Rigidbody2D>();
    }

    //ligar e desligar os controles
    private void OnEnable()
    {
        controle.Enable();
    }
    private void OnDisable()
    {
        controle.Disable();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mover=controle.Player.Move.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        rig.linearVelocity = mover * velocidade;
    }
}
