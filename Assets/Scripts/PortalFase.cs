using UnityEngine;

public class PortalFase : MonoBehaviour
{
    [Header("Gerenciador de fases")]

    [SerializeField] private GerenciadorFases gerenciadorFases;


    [Header("Destino")]

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
            gerenciadorFases.IrParaFase(indiceFaseDestino);
        }
    }
}