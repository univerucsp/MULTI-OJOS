/*
using UnityEngine;
using System.Collections;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al texto donde mostrarás el puntaje

    private void Start()
    {

    	Colission.totalScore *= 10;
        // Establecer el puntaje a 5 al inicio
        SetFinalScore(Colission.totalScore);
    }

    // Este método se llamará para establecer el puntaje final
    public void SetFinalScore(int score)
    {
        UpdateScoreDisplay(score);
    }

    private void UpdateScoreDisplay(int score)
    {
        scoreText.text = score.ToString();
    }
}

*/

using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;   // Texto para el puntaje
    public TextMeshProUGUI winnerText; // Texto para el nombre del ganador

    private void Start()
    {
        // Recuperar el nombre del ganador y su puntaje desde PlayerPrefs
        string winnerName = PlayerPrefs.GetString("WinnerName", "Desconocido");
        int winnerScore = PlayerPrefs.GetInt("WinnerScore", 0);

        // Actualizar la UI con los datos del ganador
        UpdateScoreDisplay(winnerName, winnerScore);
    }

    private void UpdateScoreDisplay(string name, int score)
    {
        // Actualizar los textos en la UI
        winnerText.text = $"{name}";
        scoreText.text = $"Puntaje: {score}";
    }
}

