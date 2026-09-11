using UnityEngine;

public class Mensagem : MonoBehaviour
{

    public GameObject mensagem;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mensagem.SetActive(true);
        }
    }

}
