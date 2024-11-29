using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class MainMenu : MonoBehaviour
{
    private string serverIP = "127.0.0.1"; // Dirección IP del servidor Python
    private int serverPort = 65432;        // Puerto de comunicación para enviar
    private int listenPort = 65434;        // Puerto para escuchar mensajes

    private UdpClient udpClient;
    private bool isRunning = true;

    private void Start()
    {
        // Iniciar el cliente UDP para escuchar mensajes
        udpClient = new UdpClient(listenPort);
        StartListening();
    }

    private void OnDestroy()
    {
        isRunning = false;
        udpClient.Close();
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialMenu");
    }

    public void TutorialVideoGAME()
    {
        SceneManager.LoadScene("TutorialGame");
    }

    public void TutorialVideoJOIN()
    {
        SceneManager.LoadScene("TutorialJOIN");
    }

    public void TutorialVideoPLAYER1()
    {
        SceneManager.LoadScene("TutorialPLAYER1");
    }

    public void TutorialVideoPLAYER2()
    {
        SceneManager.LoadScene("TutorialPLAYER2");
    }

    public void PlayGame()
    {
        SendStartGame();
        SceneManager.LoadScene("Juego");
    }

    public void CalibrateGame()
    {
        SendCalibrationMessage();
        SceneManager.LoadScene("CalibrateGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void SendCalibrationMessage()
    {
        SendMessageToServer("activar-calibracion");
    }

    private void SendStartGame()
    {
        SendMessageToServer("iniciar-juego");
    }

    private void SendMessageToServer(string message)
    {
        try
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(serverIP, serverPort);

            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length);

            udpClient.Close();
        }
        catch (SocketException ex)
        {
            Debug.LogError("Error al enviar el mensaje: " + ex.Message);
        }
    }

    private async void StartListening()
    {
        while (isRunning)
        {
            try
            {
                UdpReceiveResult result = await udpClient.ReceiveAsync();
                string receivedMessage = Encoding.UTF8.GetString(result.Buffer);
                HandleReceivedMessage(receivedMessage);
            }
            catch (SocketException ex)
            {
                Debug.LogError("Error al recibir el mensaje: " + ex.Message);
            }
        }
    }

    private void HandleReceivedMessage(string message)
    {
        Debug.Log($"Mensaje recibido: {message}");

        switch (message)
        {
            case "play":
                PlayGame();
                break;
            case "tutorial":
                Tutorial();
                break;
            case "game":
                TutorialVideoGAME();
                break;
            case "join game":
                TutorialVideoJOIN();
                break;
            case "player one":
                TutorialVideoPLAYER1();
                break;
            case "player two":
                TutorialVideoPLAYER2();
                break;
                
            case "calibrate":
                CalibrateGame();
                break;
            case "quit":
                QuitGame();
                break;
            case "back":
                BackButton();
                break;
            default:
                Debug.Log("Comando no reconocido.");
                break;
        }
    }
}

