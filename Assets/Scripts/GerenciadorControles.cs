using System.Runtime.InteropServices;
using UnityEngine;

public class GerenciadorControles : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField]
    private GameObject controlesMobile;

    [Header("Teste no Editor")]
    [SerializeField]
    private bool testarControlesMobile = false;

#if UNITY_WEBGL && !UNITY_EDITOR

    // Função JavaScript que existe no arquivo
    // DetectorDispositivo.jslib.
    [DllImport("__Internal")]
    private static extern int EhNavegadorMobile();

#endif

    private void Start()
    {
        VerificarDispositivo();
    }

    private void VerificarDispositivo()
    {
        bool navegadorMobile = false;

#if UNITY_WEBGL && !UNITY_EDITOR

        // Na Build Web, perguntamos diretamente
        // ao navegador se ele é mobile.
        navegadorMobile = EhNavegadorMobile() == 1;

#elif UNITY_EDITOR

        // Dentro do Editor usamos esta opção
        // apenas para facilitar os testes.
        navegadorMobile = testarControlesMobile;

#else

        // Caso futuramente seja criada uma Build
        // Android ou iOS nativa.
        navegadorMobile = Application.isMobilePlatform;

#endif

        controlesMobile.SetActive(navegadorMobile);
    }
}