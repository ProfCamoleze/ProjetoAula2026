using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarCenaComLoad : MonoBehaviour
{
    [Header("Configuração das Cenas")]
    public string nomeProximaCena = "Jogo";
    public string nomeCenaLoad = "Load";

    public void Carregar()
    {
        PlayerPrefs.SetString("ProximaCena", nomeProximaCena);
        SceneManager.LoadScene(nomeCenaLoad);
    }
}