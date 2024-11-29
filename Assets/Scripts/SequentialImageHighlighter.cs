using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SequentialImageHighlighter : MonoBehaviour
{
    public Image[] images; // Lista de imágenes en el orden deseado.
    public float displayDuration = 2f; // Duración durante la cual cada imagen se muestra.

    private void Start()
    {
        // Inicia la secuencia de mostrar imágenes
        StartCoroutine(DisplayImagesSequentially());
    }

    private IEnumerator DisplayImagesSequentially()
    {
        foreach (Image image in images)
        {
            if (image != null)
            {
                // Oculta todas las imágenes
                foreach (Image img in images)
                {
                    if (img != null)
                    {
                        img.enabled = false;
                    }
                }

                // Muestra la imagen actual
                image.enabled = true;

                // Espera el tiempo establecido
                yield return new WaitForSeconds(displayDuration);
            }
        }

        // Al finalizar, oculta todas las imágenes (opcional)
        foreach (Image img in images)
        {
            if (img != null)
            {
                img.enabled = false;
            }
        }
    }
}

