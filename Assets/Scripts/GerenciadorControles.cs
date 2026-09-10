using UnityEngine;
using System.Runtime.InteropServices;

public class GerenciadorControles : MonoBehaviour
{

 public GameObject controlesMobile;


 bool testarControlesMobile;


#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern int EhNavegadorMobile();

#endif


    private void Start()
    {
        bool mostrarControles = testarControlesMobile;


#if UNITY_WEBGL && !UNITY_EDITOR

        mostrarControles = EhNavegadorMobile() == 1;

#endif


        controlesMobile.SetActive(mostrarControles);
    }
}