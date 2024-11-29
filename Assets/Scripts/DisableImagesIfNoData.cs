using UnityEngine;

public class DisableImagesIfNoData : MonoBehaviour
{
    public string targetTag = "ImageTag"; // Tag de las imágenes a desactivar

    void Start()
    {
        // Desactivar las imágenes al inicio
        DisableImages();
    }

    void DisableImages()
    {
        // Buscar todos los objetos con el tag especificado
        GameObject[] images = GameObject.FindGameObjectsWithTag(targetTag);

        // Desactivar todas las imágenes
        foreach (GameObject image in images)
        {
            image.SetActive(true);
        }

        Debug.Log("Todas las imágenes con tag 'ImageTag' han sido desactivadas.");
    }
}

