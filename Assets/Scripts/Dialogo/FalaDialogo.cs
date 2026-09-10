using UnityEngine;

// Representa uma única fala do diálogo.
[System.Serializable]
public class FalaDialogo
{
    public string nomePersonagem;
    public Sprite retrato;

    [TextArea(3, 6)]
    public string texto;
}