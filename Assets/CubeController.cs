using System;
using UnityEngine;
using Alteruna;

public class CubeController : MonoBehaviour
{
    private Multiplayer multiplayer;
    public float speed = 5.0f;

    void Start()
    {
        multiplayer = GetComponent<Multiplayer>();
        
        // Asegurarse de que solo el segundo jugador controle el cubo
        if (multiplayer.Me.Index != 1)
        {
            gameObject.SetActive(false); // Desactivar el cubo en la primera máquina
        }
    }

    void Update()
    {
        if (multiplayer.Me.Index == 1)
        {
            // Control de movimiento con las teclas de flecha
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            
            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical) * speed * Time.deltaTime;
            transform.Translate(movement);
        }
    }
}

