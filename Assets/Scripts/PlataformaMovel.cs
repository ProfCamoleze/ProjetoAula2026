using UnityEngine;

public class PlataformaMover : MonoBehaviour
{
    [Header("Pontos da plataforma")]
    public Transform pontoA;
    public Transform pontoB;

    [Header("Movimento")]
    public float velocidade = 2f;

    private Vector3 destino;

    void Start()
    {
        destino = pontoB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            destino,
            velocidade * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, pontoB.position) < 0.05f)
        {
            destino = pontoA.position;
        }
        else if (Vector3.Distance(transform.position, pontoA.position) < 0.05f)
        {
            destino = pontoB.position;
        }
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
            // Pega o pai do objeto Pe, que continua sendo o Player
            Transform player = colidir.transform.parent;
             player.SetParent(null, true);
            
        }
    }
}