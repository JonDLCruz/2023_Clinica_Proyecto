using UnityEngine;



public class Dialogo : MonoBehaviour
{
    public Introduccion intro;
    public LoginManager userlogin;

    private void Update()
    {
        string usuarioActivo = userlogin.logUser;
        Debug.Log("Valor de usuario: " + usuarioActivo);
        intro = new Introduccion();
        intro.lines = new string[]
        {  //Saludos entrenador pokemon soy el profesor oak.
            "Bienvenido " + usuarioActivo,
            "Soy el profesor Piñitos",
            "Estas apunto de aventurarte en una fantastica aventura",
            "de reparacion de dientes y caries",
            "acompañame a la clinica."
        };
    }

    


}
