using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Personaje", menuName = "Sistema de Dialogo/Nuevo Personaje")]
public class Personaje : ScriptableObject
{
    public string nombre;
    public Sprite imagen;
}
