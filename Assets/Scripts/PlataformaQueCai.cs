using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlataformaQueCai : MonoBehaviour
{
    [Header("Tempos")]
    public float tempoAntesDeCair = 2f;
    public float tempoParaVoltar = 10f;

    [Header("Física da Queda")]
    public float gravidadeDuranteQueda = 3f;

    [Header("Camadas / Layers")]
    public string layerNormal = "Default";
    public string layerCaindo = "PlataformaCaindo";

    private Rigidbody2D rig;
    private Animator anim;
    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;

    private bool jaAtivou = false;
    private int layerNormalID;
    private int layerCaindoID;

    //private Player player;
    string nomeDaLayer;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        posicaoInicial = transform.position;
        rotacaoInicial = transform.rotation;
        layerNormalID = LayerMask.NameToLayer(layerNormal);
        layerCaindoID = LayerMask.NameToLayer(layerCaindo);
        gameObject.layer= layerNormalID;

        if (rig == null)
        {
            Debug.LogError("A plataforma precisa de um Rigidbody2D.");
            enabled = false;
            return;
        }

        // Estado inicial da plataforma.
        rig.bodyType = RigidbodyType2D.Kinematic;
        rig.gravityScale = 0f;
        rig.linearVelocity = Vector2.zero;
        rig.angularVelocity = 0f;
    }

    private void OnTriggerEnter2D(Collider2D colisao)
    {
         // O objeto Pe do Player precisa estar com a tag "Pe".
        if (colisao.CompareTag("Pe") && !jaAtivou)
        {

           // player.layerGround = LayerMask.NameToLayer("chao");
            StartCoroutine(CairEVoltar());
        }
    }

    IEnumerator CairEVoltar()
    {
        jaAtivou = true;

        // Para a animação em loop da plataforma.
        if (anim != null)
        {
            anim.speed = 0f;
        }

        // Espera antes de cair.
        yield return new WaitForSeconds(tempoAntesDeCair);

        // Troca para a layer que ignora o Tilemap.
        if (layerCaindoID != -1)
        {
            gameObject.layer = layerCaindoID;
        }
        else
        {
            Debug.LogWarning("A layer PlataformaCaindo não existe. Crie essa layer no Inspector.");
        }

        // Agora a plataforma passa a cair pela física.
        rig.bodyType = RigidbodyType2D.Dynamic;
        rig.gravityScale = gravidadeDuranteQueda;
        rig.linearVelocity = Vector2.zero;
        rig.angularVelocity = 0f;

        // Espera 10 segundos antes de voltar.
        yield return new WaitForSeconds(tempoParaVoltar);

        // Para a física antes de reposicionar.
        rig.linearVelocity = Vector2.zero;
        rig.angularVelocity = 0f;
        rig.gravityScale = 0f;
        rig.bodyType = RigidbodyType2D.Kinematic;

        // Volta para a posição inicial.
        transform.position = posicaoInicial;
        transform.rotation = rotacaoInicial;

        // Volta para a layer normal.
        if (layerNormalID != -1)
        {
            gameObject.layer = layerNormalID;
        }

        // Reativa a animação.
        if (anim != null)
        {
            anim.speed = 1f;
        }

        jaAtivou = false;
    }
}