using UnityEngine;

public class PegarObj : MonoBehaviour
{
    [Header("Configuração da Mão")]
    public Transform posicaoDaMao;

    private PlayerControle controle;

    private GameObject objetoProximo;
    private GameObject objetoNaMao;

    private bool segurandoObjeto = false;

    private void Awake()
    {
        controle = new PlayerControle();

        if (posicaoDaMao == null)
        {
            posicaoDaMao = transform;
        }
    }

    private void OnEnable()
    {
        controle.Enable();
    }

    private void OnDisable()
    {
        controle.Disable();
    }

    private void Update()
    {
        if (controle.Player.Interact.WasPressedThisFrame())
        {
            if (segurandoObjeto)
            {
                SoltarObjeto();
            }
            else
            {
                Pegar();
            }
        }
    }

    private void Pegar()
    {
        if (objetoProximo != null)
        {
            objetoNaMao = objetoProximo;
            segurandoObjeto = true;

            objetoNaMao.transform.SetParent(posicaoDaMao);
            objetoNaMao.transform.localPosition = Vector3.zero;

            Rigidbody2D rigObjeto = objetoNaMao.GetComponent<Rigidbody2D>();

            if (rigObjeto != null)
            {
                rigObjeto.linearVelocity = Vector2.zero;
                rigObjeto.angularVelocity = 0f;
                rigObjeto.simulated = false;
            }
        }
    }

    private void SoltarObjeto()
    {
        if (objetoNaMao != null)
        {
            objetoNaMao.transform.SetParent(null);

            Rigidbody2D rigObjeto = objetoNaMao.GetComponent<Rigidbody2D>();

            if (rigObjeto != null)
            {
                rigObjeto.simulated = true;
            }

            objetoNaMao = null;
            segurandoObjeto = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Pegar"))
        {
            objetoProximo = outro.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D outro)
    {
        if (outro.CompareTag("Pegar"))
        {
            if (outro.gameObject == objetoProximo)
            {
                objetoProximo = null;
            }
        }
    }
}