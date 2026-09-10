using UnityEngine;


// Permite criar arquivos de diálogo no Project.
[CreateAssetMenu(
    fileName = "NovoDialogo",
    menuName = "Dialogo/Novo Dialogo"
)]
public class DadosDialogo : ScriptableObject
{
    public FalaDialogo[] falas;
}
