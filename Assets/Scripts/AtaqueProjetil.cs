using UnityEngine;
using UnityEngine.Rendering;

public class AtaqueProjetil : MonoBehaviour
{
    public Transform pontoDisparo;
    public GameObject prefabProjetil;
    public float tempoEntreTiros = 0.3f;
    PlayerControle controle;
    float tempoProximoTiro = 0f;
    void Awake()
    {
        controle = new PlayerControle();
    }
    private void OnEnable()
    {
        controle.Enable();
    }
    void OnDisable()
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
            if (controle.Player.Attack.WasPressedThisFrame())
            {
               TentarAtirar();
        }
      
    }

    void TentarAtirar()
    {
        if (Time.time < tempoProximoTiro)
        {
            return;
        }
            Atirar();
        tempoProximoTiro= Time.time + tempoEntreTiros;

    }
    void Atirar()
    {

        GameObject projetil = Instantiate(prefabProjetil, pontoDisparo.position, pontoDisparo.rotation);
        Projetil scriptProjetil= projetil.GetComponent<Projetil>();
        if (scriptProjetil != null)
        {
            Vector2 direcaoTiro=pontoDisparo.right;
            scriptProjetil.Atirar(direcaoTiro);
        }
    }
}
