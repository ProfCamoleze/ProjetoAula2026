using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidade = 5f;
    Vector2 mover;
    Rigidbody2D rig;
    PlayerControle controle;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();  
        controle = new PlayerControle();
    }

    private void OnEnable()
    {
        controle.Enable();
    }
    private void OnDisable()
    {
        controle.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mover = controle.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rig.linearVelocityX=mover.x * velocidade;
    }
}
