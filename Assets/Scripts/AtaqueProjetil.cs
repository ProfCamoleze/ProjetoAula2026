using UnityEngine;
using UnityEngine.Rendering;

public class AtaqueProjetil : MonoBehaviour
{
    public Transform pontoDisparo;
    public GameObject prefabProjetil;
    public float tempoEntreTiros = 0.3f;

    private PlayerControle controle;
    private float tempoProximoTiro = 0f;

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

        Instantiate(prefabProjetil, pontoDisparo.position, pontoDisparo.rotation);

        tempoProximoTiro = Time.time + tempoEntreTiros;
    }
}