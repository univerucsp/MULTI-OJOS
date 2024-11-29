using UnityEngine;
using UnityEngine.UI; // Necesario para acceder al componente Image

public class ImageVisibility : MonoBehaviour
{
    public Image imageToDisable; // Asigna la imagen desde el Inspector

    // Este método se ejecuta al inicio
    void Start()
    {
        // Deshabilitar la imagen al inicio
        imageToDisable.enabled = false;
    }
}

