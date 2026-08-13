using UnityEngine;

public class PortalFase : MonoBehaviour
{
    [Header("Gerenciador de fases")]

    [Tooltip("Arraste aqui o GerenciadorFases.")]
    [SerializeField] private GerenciadorFases gerenciadorFases;


    [Header("Destino")]

    [Tooltip("Índice da fase de destino. Fase01 = 0, Fase02 = 1, Fase03 = 2...")]
    [SerializeField] private int indiceFaseDestino = 1;


    private void OnTriggerEnter2D(
        Collider2D outro
    )
    {
        // Verifica se foi o Player
        // que entrou no Portal.
        if (outro.CompareTag("Player"))
        {
            // Solicita ao gerenciador
            // a troca para a fase escolhida.
            gerenciadorFases.IrParaFase(
                indiceFaseDestino
            );
        }
    }
}