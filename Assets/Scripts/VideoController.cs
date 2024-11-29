using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public string mainMenuSceneName = "TutorialMenu"; // Nombre de la escena del menú principal
    private VideoPlayer videoPlayer;

    private void Start()
    {
        // Obtén el componente VideoPlayer del objeto
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            // Asigna el método que será llamado al terminar el video
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("No se encontró un componente VideoPlayer en este objeto.");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Cambiar a la escena del menú principal
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

