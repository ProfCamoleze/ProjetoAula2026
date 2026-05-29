using UnityEngine;

public class PlataformaRotativa : MonoBehaviour
{
    [Header("Centro do movimento circular")]
    public Transform centro;

    [Header("Movimento circular")]
    public float velocidadeAngular = 50f;
    public bool sentidoHorario = false;

    private Quaternion rotacaoInicial;

    void Start()
    {
        rotacaoInicial = transform.rotation;
    }

    void Update()
    {
        // Define se a plataforma gira para um lado ou para o outro
        float direcao = sentidoHorario ? -1f : 1f;

        // Faz a plataforma girar ao redor do objeto Centro
        transform.RotateAround(
            centro.position,
            Vector3.forward,
            velocidadeAngular * direcao * Time.deltaTime
        );
            transform.rotation = rotacaoInicial;
    }

    private void OnTriggerEnter2D(Collider2D colidir)
    {
        if (colidir.CompareTag("Pe"))
        {
            Transform player = colidir.transform.parent;
            player.SetParent(transform, true);
        }
    }

    private void OnTriggerExit2D(Collider2D colidir)
    {
        if (colidir.CompareTag("Pe"))
        {
            Transform player = colidir.transform.parent;
            player.SetParent(null, true);
        }
    }
}