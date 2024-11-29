using UnityEngine;

public class ShowCanvasOnEnter : MonoBehaviour
{
    public Canvas canvas;  // Referencia al Canvas

    void Start()
    {
        // Asegurarse de que el Canvas esté desactivado al inicio
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Detectar si la tecla Enter es presionada
        if (Input.GetKeyDown(KeyCode.Return)) // O KeyCode.Enter
        {
            // Alternar la visibilidad del Canvas
            if (canvas != null)
            {
                canvas.gameObject.SetActive(true);
            }
        }
    }
}

