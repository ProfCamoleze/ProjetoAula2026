using UnityEngine;

public class PortalFase : MonoBehaviour
{
    [Header("Gerenciador de fases")]
    public GerenciadorFases gerenciadorFases;

    private void OnTriggerEnter2D(Collider2D outro)
    {
        // Verifica se quem entrou no portal foi o Player.
        if (outro.CompareTag("Player"))
        {
            // Chama a troca para a Fase02.
            gerenciadorFases.IrParaFase02();
        }
    }
}