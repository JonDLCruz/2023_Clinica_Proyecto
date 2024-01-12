using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espejo : MonoBehaviour
{
    public Transform player;
    public Transform espejoCamara;
    public Transform espejoPlano;

    private Vector2 distancia;
    float cameraRotacion;
    float planoRotacion;

    // Start is called before the first frame update
    void Start()
    {
        cameraRotacion = espejoCamara.eulerAngles.y;
        planoRotacion = espejoPlano.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        distancia = new Vector2(player.position.x - transform.position.x, player.position.z - transform.position.z);
        espejoCamara.eulerAngles = new Vector3(
            espejoCamara.eulerAngles.x,
            cameraRotacion + (planoRotacion - Angulo(distancia)),
            espejoCamara.eulerAngles.z);
    }

    private float Angulo(Vector2 vector2)
    {
        if(vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }
    }
}
