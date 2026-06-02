using UnityEngine;

public class Fantasma : MonoBehaviour
{
    // quanto tempo o fantasma leva para sumir (em segundos)
    public float tempoParaSumir = 0.3f;

    SpriteRenderer sr;   // o desenho (sprite) deste fantasma
    float tempoVivo;     // ha quanto tempo este fantasma existe
    Color cor;           // guarda a cor para mexer na transparencia

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cor = sr.color;   // pega a cor inicial do fantasma
    }

    void Update()
    {
        // soma o tempo do quadro ao tempo de vida do fantasma
        tempoVivo += Time.deltaTime;

        // calcula a transparencia: vai de 1 (opaco) ate 0 (invisivel)
        // Lerp = mistura suave entre dois valores
        float alpha = Mathf.Lerp(1f, 0f, tempoVivo / tempoParaSumir);

        cor.a = alpha;     // 'a' eh a transparencia (alpha) da cor
        sr.color = cor;    // aplica a nova transparencia no sprite

        // quando some totalmente, destroi o fantasma
        if (tempoVivo >= tempoParaSumir)
        {
            Destroy(gameObject);
        }
    }
}
