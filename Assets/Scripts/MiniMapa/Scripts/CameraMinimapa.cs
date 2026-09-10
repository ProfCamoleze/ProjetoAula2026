using UnityEngine;

public class CameraMinimapa : MonoBehaviour
{
    [Header("Referência")][SerializeField] private Transform player;
    [Header("Configuração")][SerializeField] private float posicaoZ = -10f;
    private void LateUpdate()
    {
        // Guarda a posição atual do Player.
        Vector3 novaPosicao = player.position;
        // Mantém a câmera afastada no eixo Z.
        novaPosicao.z = posicaoZ;
        // Move a câmera para a posição calculada.
        transform.position = novaPosicao;
    }
}