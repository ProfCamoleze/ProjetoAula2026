using UnityEngine;

public class PortalTrocarFaseMesmaCena : MonoBehaviour
{
    [Header("Destino da próxima fase")]
    public Transform pontoDestino;

    [Header("Controlador da transição")]
    public TransicaoFaseMesmaCena controladorTransicao;

    private bool jaEntrou = false;

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (jaEntrou)
        {
            return;
        }

        if (colisao.CompareTag("Player"))
        {
            jaEntrou = true;

            controladorTransicao.IniciarTransicao(colisao.transform, pontoDestino);
        }
    }
}