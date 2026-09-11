using UnityEngine;

public class AcompanhaPlayer : MonoBehaviour
{
    public Transform pontoDialogo;

    void LateUpdate()
    {
        transform.position = pontoDialogo.position;

        // Mantém o diálogo sem virar junto com o Player.
        transform.rotation = Quaternion.identity;
    }
}
