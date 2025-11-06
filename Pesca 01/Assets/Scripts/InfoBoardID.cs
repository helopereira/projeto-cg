using UnityEngine;

/// <summary>
/// Anexado à placa 3D para definir qual painel de UI ela deve ativar.
/// </summary>
public class InfoBoardID : MonoBehaviour
{
    [Tooltip("ID único que corresponde ao índice do painel no InfoPanelController (Começa em 1).")]
    public int panelID = 1;
}