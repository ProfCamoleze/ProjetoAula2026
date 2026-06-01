using UnityEngine;
using UnityEngine.InputSystem;

public class SegurarObjeto : MonoBehaviour
{
    [Header("Configurações de Input")]
    [SerializeField] private InputActionAsset assetDeInput;
    [SerializeField] private string nomeMapaAcao = "Player";
    [SerializeField] private string nomeAcao = "Interact";

    [Header("Mecânica de Segurar")]
    [SerializeField] private Transform posicaoDaMao; // Ponto onde o objeto vai ficar preso

    private InputAction acaoInteragir;
    private GameObject objetoTocado; // Guarda o objeto em que o player está encostado
    private GameObject objetoSegurado; // Guarda o objeto que o player já está carregando
    private bool estaSegurando = false;

    private void Awake()
    {
        // Localiza a ação dentro do asset
        var mapaAcao = assetDeInput.FindActionMap(nomeMapaAcao);
        if (mapaAcao != null)
        {
            acaoInteragir = mapaAcao.FindAction(nomeAcao);
        }
    }

    private void OnEnable()
    {
        // Ativa a ação diretamente, sem usar += ou -=
        if (acaoInteragir != null)
        {
            acaoInteragir.Enable();
        }
    }

    private void OnDisable()
    {
        // Desativa a ação diretamente, sem usar += ou -=
        if (acaoInteragir != null)
        {
            acaoInteragir.Disable();
        }
    }

    private void Update()
    {
        // Verifica se o botão foi pressionado neste frame
        if (acaoInteragir != null && acaoInteragir.WasPressedThisFrame())
        {
            if (estaSegurando)
            {
                SoltarOObjeto();
            }
            else
            {
                TentarSegurarOObjeto();
            }
        }
    }

    private void TentarSegurarOObjeto()
    {
        // Só tenta segurar se o player estiver encostado em algum objeto válido
        if (objetoTocado != null)
        {
            objetoSegurado = objetoTocado;
            estaSegurando = true;

            // Faz o objeto virar "filho" da mão do player para acompanhar o movimento
            objetoSegurado.transform.SetParent(posicaoDaMao);
            objetoSegurado.transform.localPosition = Vector3.zero;

            // Desativa a física para o objeto não colidir com o próprio player enquanto é carregado
            if (objetoSegurado.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.simulated = false;
            }
        }
    }

    private void SoltarOObjeto()
    {
        if (objetoSegurado != null)
        {
            // Remove o vínculo de "filho", devolvendo o objeto para o cenário
            objetoSegurado.transform.SetParent(null);

            // Reativa a física do objeto para ele voltar a cair e interagir com o mundo
            if (objetoSegurado.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.simulated = true;
            }

            objetoSegurado = null;
            estaSegurando = false;
        }
    }

    // Detecta quando o Player encosta no colisor do objeto
    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("pegar"))
        {
            objetoTocado = outro.gameObject;
        }
    }

    // Detecta quando o Player se afasta e para de encostar no objeto
    private void OnTriggerExit2D(Collider2D outro)
    {
        if (outro.CompareTag("pegar"))
        {
            // Se o objeto que nos afastamos era o que estávamos tocando, limpa a referência
            if (outro.gameObject == objetoTocado)
            {
                objetoTocado = null;
            }
        }
    }
}