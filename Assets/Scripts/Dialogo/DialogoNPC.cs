using UnityEngine;

public class DialogoNPC : MonoBehaviour
{
    [Header("Diálogo")]
    public DadosDialogo dialogo;

    [Header("Gerenciador")]
    public GerenciadorDialogo gerenciadorDialogo;

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            gerenciadorDialogo.IniciarDialogo(dialogo);
        }
    }
}
