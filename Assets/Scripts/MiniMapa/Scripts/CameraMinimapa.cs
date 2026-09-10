using UnityEngine;

public class CameraMinimapa : MonoBehaviour
{ /*versão simples
    [Header("Referências")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera cameraMinimapa;
    [SerializeField] private PolygonCollider2D limiteCam;

    [Header("Configuração")]
    [SerializeField] private float posicaoZ = -10f;

    private void LateUpdate()
    {
        // Pega os limites do PolygonCollider2D.
        Bounds limites = limiteCam.bounds;

        // Calcula metade da altura visível da câmera.
        float metadeAltura = cameraMinimapa.orthographicSize;

        // Calcula metade da largura visível da câmera.
        float metadeLargura =
            metadeAltura * cameraMinimapa.aspect;

        // Calcula até onde o centro da câmera pode ir.
        float minimoX = limites.min.x + metadeLargura;
        float maximoX = limites.max.x - metadeLargura;

        float minimoY = limites.min.y + metadeAltura;
        float maximoY = limites.max.y - metadeAltura;

        // Limita a posição do Player dentro da área permitida.
        float posicaoX = Mathf.Clamp(player.position.x, minimoX, maximoX);

        float posicaoY = Mathf.Clamp(player.position.y, minimoY, maximoY);

        // Move a câmera.
        transform.position = new Vector3(posicaoX, posicaoY, posicaoZ);
    }
} */

    [Header("Referências")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera cameraMinimapa;

    [Header("Limites da Câmera")]
    [SerializeField] private PolygonCollider2D[] limitesCamera;

    [Header("Configuração")]
    [SerializeField] private float posicaoZ = -10f;

    private PolygonCollider2D limiteAtual;

    private void LateUpdate()
    {
        // Procura qual limite está ativo atualmente.
        EncontrarLimiteAtivo();

        // Se nenhum limite estiver ativo, não continua os cálculos.
        if (limiteAtual == null)
        {
            return;
        }

        // Pega os limites do PolygonCollider2D ativo.
        Bounds limites = limiteAtual.bounds;

        // Metade da altura visível da câmera.
        float metadeAltura = cameraMinimapa.orthographicSize;

        // Metade da largura visível da câmera.
        float metadeLargura =
            metadeAltura * cameraMinimapa.aspect;

        // Calcula até onde o centro da câmera pode ir.
        float minimoX = limites.min.x + metadeLargura;

        float maximoX = limites.max.x - metadeLargura;

        float minimoY = limites.min.y + metadeAltura;

        float maximoY = limites.max.y - metadeAltura;

        // Limita a posição da câmera
        // dentro da área permitida.
        float posicaoX = Mathf.Clamp(player.position.x, minimoX, maximoX);

        float posicaoY = Mathf.Clamp(player.position.y, minimoY, maximoY);

        // Move a câmera.
        transform.position = new Vector3(posicaoX, posicaoY, posicaoZ);
    }

    private void EncontrarLimiteAtivo()
    {
        // Começamos considerando que
        // nenhum limite foi encontrado.
        limiteAtual = null;

        // Percorre todos os limites cadastrados.
        foreach (PolygonCollider2D limite in limitesCamera)
        {
            // Verifica se o GameObject do limite
            // está ativo na Hierarchy.
            if (limite.gameObject.activeInHierarchy)
            {
                limiteAtual = limite;

                // Encontramos o limite.
                // Não precisamos continuar procurando.
                break;
            }
        }
    }
}