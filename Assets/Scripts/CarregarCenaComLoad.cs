using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarCenaComLoad : MonoBehaviour
{
    [Header("Configuração da Próxima Cena")]
    public string nomeProximaCena = "Jogo";
    public string nomeCenaLoad = "LoadEntreCenas";

    public void Carregar()
    {
        PlayerPrefs.SetString("ProximaCena", nomeProximaCena);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nomeCenaLoad);
    }
}