using UnityEngine;

public class PlataformaMovel : MonoBehaviour
{

    //Definir a posição dos pontos A e B
    public Transform pontoA;
    public Transform pontoB;
    public bool entrei=true;

    //Definir a velocidade de movimento da plataforma
    public float velocidade = 2f;

    Transform posAtual; //guarda o ponto de destino Atual

    Rigidbody2D rig; //fisica da plataforma, será usada para mover a plataforma
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        posAtual = pontoB; //definir o ponto de destino inicial como o ponto B
    }


    void FixedUpdate() // como vamos usar a fisica para mover é melhor usar o FixedUpdate
    {
        Vector2 novaPosicao = Vector2.MoveTowards(rig.position, posAtual.position, velocidade * Time.deltaTime);

        rig.MovePosition(novaPosicao); // faz a fisica se mover

        if (Vector2.Distance(novaPosicao, posAtual.position) < 0.05f) // verifica se chegou  ao destino
        {
            if (posAtual == pontoA) //esse if verifica em qual ponto esta e define o novo destino
            {
                posAtual = pontoB;
            }
            else
            {
                posAtual = pontoA;
            }
        }
    }
    //faz o player "colar na Plataforma
    private void OnTriggerEnter2D(Collider2D colidir)
    {
        if (colidir.gameObject.CompareTag("Pe"))
        {
            entrei = true;
            colidir.transform.SetParent(transform);

        }
    }
    //faz "descolar" da plataforma
    private void OnTriggerExit2D(Collider2D colidir)
    {
        if (colidir.gameObject.CompareTag("Pe"))
        {
            entrei = true;
            colidir.transform.SetParent(null);
        }

    }

}
