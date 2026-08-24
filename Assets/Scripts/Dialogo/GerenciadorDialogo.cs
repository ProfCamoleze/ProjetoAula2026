using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorDialogo : MonoBehaviour
{
    [Header("Interface")]
    public GameObject painelDialogo;
    public TMP_Text txtNome;
    public TMP_Text txtFala;
    public Image imgRetrato;

    [Header("Player")]
    public Player player;

    private DadosDialogo dialogoAtual;
    private int indiceFala;

    private void Start()
    {
        painelDialogo.SetActive(false);
    }

    public void IniciarDialogo(DadosDialogo dialogo)
    {
        dialogoAtual = dialogo;
        indiceFala = 0;

       player.bloquearMovimento();
        painelDialogo.SetActive(true);

        MostrarFala();
    }

    private void MostrarFala()
    {
        FalaDialogo fala = dialogoAtual.falas[indiceFala];

        txtNome.text = fala.nomePersonagem;
        txtFala.text = fala.texto;
        imgRetrato.sprite = fala.retrato;
    }

    public void ContinuarDialogo()
    {
        indiceFala++;

        if (indiceFala < dialogoAtual.falas.Length)
        {
            MostrarFala();
        }
        else
        {
            FecharDialogo();
        }
    }

    public void FecharDialogo()
    {
        painelDialogo.SetActive(false);
       player.liberarMovimento();
    }
}
