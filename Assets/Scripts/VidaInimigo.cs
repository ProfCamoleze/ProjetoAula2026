using UnityEngine;

public class VidaInimigo : MonoBehaviour
{
    public int vidaMaxima = 1;
    int vidaAtual;
       // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaAtual = vidaMaxima;
    }
    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void Morrer()
    {
               Destroy(gameObject);
    }
}
