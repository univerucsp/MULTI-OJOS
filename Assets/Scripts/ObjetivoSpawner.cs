/*
using System.Collections;
using UnityEngine.SceneManagement;
using Alteruna;
using UnityEngine;
using Alteruna.Trinity;  // Es posible que ITransportStreamReader esté aquí
using TMPro; // Necesario para TextMeshProUGUI


public class ObjetivoSpawner : MonoBehaviour
{
    private Alteruna.Avatar _avatar;
    private Spawner _spawner;
    private Multiplayer _multiplayer;  // Referencia a la instancia de Multiplayer

    [SerializeField] private int indexToSpawn = 1;
    private GameObject spawnedObjetivo;
    private float respawnTime = 5.0f; // Tiempo de respawn en segundos
    private float timer;
    private float elapsedTime = 0f; // Tiempo transcurrido desde que se activa el spawneo

    private bool isSpawningActive = false; 
    public int points = 0;  // Puntos del jugador
    private const int victoryPoints = 15;  // Puntaje necesario para ganar
    private const float maxGameTime = 30f;

    // Delegado para el procedimiento remoto
    private RemoteProcedure notifyVictoryProcedure;

    [SerializeField] private AudioClip objetivoSound;  // Clip de sonido para el objetivo

    private void Awake()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
        _spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        _multiplayer = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Multiplayer>();  // Obtener la referencia
    }
    
    // Método RPC para notificar a todos los jugadores sobre la victoria
    private void NotifyVictory(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
    	int playerScore = parameters.Get("playerScore", 0);
        string playerName = parameters.Get("playerName", "Unknown");
        
        Debug.Log($"¡{playerName} ha alcanzado los puntos de victoria!");
        ChangeScene(playerName, playerScore);
    }
    
    private void ChangeScene(string playerName, int playerScore)
	{
	    // Puedes guardar estos datos en una clase estática o un script de administración para usarlos en la nueva escena
	    PlayerPrefs.SetString("WinnerName", playerName);
	    PlayerPrefs.SetInt("WinnerScore", playerScore);
	    
	    // Aquí puedes poner el nombre de la escena a la que deseas cambiar
	    string sceneName = "ScoreScene";  // Nombre de la escena a cargar
	    
	    // Cargar la escena
	    SceneManager.LoadScene(sceneName);
	}
    
    private void Start()
    {
        timer = respawnTime;

        // Registrar RPC al iniciar
        notifyVictoryProcedure = new RemoteProcedure(NotifyVictory);  // Crear el delegado de RemoteProcedure
        _multiplayer.RegisterRemoteProcedure("NotifyVictory", notifyVictoryProcedure);  // Registrar el RPC
    }

    private void Update()
    {
        if (!_avatar.IsMe)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSpawningActive = true;
        }

        // Solo ejecutar la lógica de spawneo si está activa
        if (isSpawningActive)
        {
            // Incrementa el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            // Verifica si el tiempo ha expirado
            if (elapsedTime >= maxGameTime)
            {    
                ProcedureParameters parameters = new ProcedureParameters();
                parameters.Set("playerName", "Ganaste");  // Puedes personalizar el mensaje
                parameters.Set("playerScore", points);   // Puntaje del jugador
                
                _multiplayer.InvokeRemoteProcedure("NotifyVictory", UserId.All, parameters);
                // Cargar la escena
                ChangeScene("Perdiste", points);
            }
            
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (spawnedObjetivo != null)
                {
                    _spawner.Despawn(spawnedObjetivo);
                }
                SpawnObjetivo();
                timer = respawnTime;
            }
        }
    }

    void SpawnObjetivo()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), _avatar.transform.position.y, Random.Range(-8f, 8f));
        spawnedObjetivo = _spawner.Spawn(indexToSpawn, spawnPosition, Quaternion.identity, new Vector3(3f, 3f, 3f));

        // Añadir componente de colisión si no está ya presente
        if (spawnedObjetivo.GetComponent<Collider>() == null)
        {
            spawnedObjetivo.AddComponent<BoxCollider>().isTrigger = true;
        } 

        // Añadir componente AudioSource
        AudioSource audioSource = spawnedObjetivo.AddComponent<AudioSource>();
        audioSource.clip = objetivoSound;
        audioSource.playOnAwake = false;  // No reproducir inmediatamente

        // Añadir script de comportamiento de colisión
        ObjetivoBehaviour behaviour = spawnedObjetivo.AddComponent<ObjetivoBehaviour>();
        behaviour.onObjetivoTouched += () => TeleportObjetivo(audioSource);  // Pasamos el audioSource al manejador
    }

    void TeleportObjetivo(AudioSource audioSource)
    {
        if (spawnedObjetivo != null)
        {
            _spawner.Despawn(spawnedObjetivo);
            SpawnObjetivo();
            points++;
            Debug.Log("Puntos: " + points);

            // Reproducir sonido cuando el objetivo es tocado
            audioSource.Play();

            // Verificar si alcanzó los puntos de victoria
            if (points >= victoryPoints)
            {
                // Notificar a todos los jugadores sobre la victoria
                ProcedureParameters parameters = new ProcedureParameters();
                parameters.Set("playerName", "Perdiste");  // Puedes personalizar el mensaje
                parameters.Set("playerScore", points);   // Puntaje del jugador
                
                _multiplayer.InvokeRemoteProcedure("NotifyVictory", UserId.All, parameters);
                // Cargar la escena
                ChangeScene("Ganaste", points);
            }
        }
    }
}

public class ObjetivoBehaviour : MonoBehaviour
{
    public delegate void ObjetivoTouchedHandler();
    public event ObjetivoTouchedHandler onObjetivoTouched;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            onObjetivoTouched?.Invoke();
        }
    }
}
*/

/*
using System.Collections;
using UnityEngine.SceneManagement;
using Alteruna;
using UnityEngine;
using Alteruna.Trinity;  // Es posible que ITransportStreamReader esté aquí
using TMPro; // Necesario para TextMeshProUGUI

public class ObjetivoSpawner : MonoBehaviour
{
    private Alteruna.Avatar _avatar;
    private Spawner _spawner;
    private Multiplayer _multiplayer;  // Referencia a la instancia de Multiplayer

    [SerializeField] private int indexToSpawn = 1;
    private GameObject spawnedObjetivo;
    private float respawnTime = 5.0f; // Tiempo de respawn en segundos
    private float timer;
    private float elapsedTime = 0f; // Tiempo transcurrido desde que se activa el spawneo

    private bool isSpawningActive = false; 
    public int points = 0;  // Puntos del jugador
    private const int victoryPoints = 15;  // Puntaje necesario para ganar
    private const float maxGameTime = 30f;

    // Delegado para el procedimiento remoto
    private RemoteProcedure notifyVictoryProcedure;

    [SerializeField] private AudioClip objetivoSound;  // Clip de sonido para el objetivo

    private TextMeshProUGUI scoreText;  // Referencia al texto que muestra el puntaje

    private void Awake()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
        _spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        _multiplayer = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Multiplayer>();  // Obtener la referencia

        // Buscar el Canvas por Tag
        GameObject scoreCanvas = GameObject.FindGameObjectWithTag("ScoreCanvas");
        if (scoreCanvas != null)
        {
            // Buscar el componente TextMeshProUGUI dentro del Canvas
            scoreText = scoreCanvas.GetComponentInChildren<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Canvas de puntaje no encontrado. Asegúrate de asignar el Tag 'ScoreCanvas' al Canvas.");
        }
    }
    
    // Método RPC para notificar a todos los jugadores sobre la victoria
    private void NotifyVictory(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        int playerScore = parameters.Get("playerScore", 0);
        string playerName = parameters.Get("playerName", "Unknown");
        
        Debug.Log($"¡{playerName} ha alcanzado los puntos de victoria!");
        ChangeScene(playerName, playerScore);
    }
    
    private void ChangeScene(string playerName, int playerScore)
    {
        // Puedes guardar estos datos en una clase estática o un script de administración para usarlos en la nueva escena
        PlayerPrefs.SetString("WinnerName", playerName);
        PlayerPrefs.SetInt("WinnerScore", playerScore);
        
        // Aquí puedes poner el nombre de la escena a la que deseas cambiar
        string sceneName = "ScoreScene";  // Nombre de la escena a cargar
        
        // Cargar la escena
        SceneManager.LoadScene(sceneName);
    }
    
    private void Start()
    {
        timer = respawnTime;

        // Registrar RPC al iniciar
        notifyVictoryProcedure = new RemoteProcedure(NotifyVictory);  // Crear el delegado de RemoteProcedure
        _multiplayer.RegisterRemoteProcedure("NotifyVictory", notifyVictoryProcedure);  // Registrar el RPC
    }

    private void Update()
    {
        if (!_avatar.IsMe)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSpawningActive = true;
        }

        // Solo ejecutar la lógica de spawneo si está activa
        if (isSpawningActive)
        {
            // Incrementa el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            // Verifica si el tiempo ha expirado
            if (elapsedTime >= maxGameTime)
            {    
                ProcedureParameters parameters = new ProcedureParameters();
                parameters.Set("playerName", "Ganaste");  // Puedes personalizar el mensaje
                parameters.Set("playerScore", points);   // Puntaje del jugador
                
                _multiplayer.InvokeRemoteProcedure("NotifyVictory", UserId.All, parameters);
                // Cargar la escena
                ChangeScene("Perdiste", points);
            }
            
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (spawnedObjetivo != null)
                {
                    _spawner.Despawn(spawnedObjetivo);
                }
                SpawnObjetivo();
                timer = respawnTime;
            }
        }

        // Actualizar el puntaje en el texto del Canvas
        if (scoreText != null)
        {
            scoreText.text = "Puntaje: " + points.ToString();
        }
    }

    void SpawnObjetivo()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), _avatar.transform.position.y, Random.Range(-8f, 8f));
        spawnedObjetivo = _spawner.Spawn(indexToSpawn, spawnPosition, Quaternion.identity, new Vector3(3f, 3f, 3f));

        // Añadir componente de colisión si no está ya presente
        if (spawnedObjetivo.GetComponent<Collider>() == null)
        {
            spawnedObjetivo.AddComponent<BoxCollider>().isTrigger = true;
        } 

        // Añadir componente AudioSource
        AudioSource audioSource = spawnedObjetivo.AddComponent<AudioSource>();
        audioSource.clip = objetivoSound;
        audioSource.playOnAwake = false;  // No reproducir inmediatamente

        // Añadir script de comportamiento de colisión
        ObjetivoBehaviour behaviour = spawnedObjetivo.AddComponent<ObjetivoBehaviour>();
        behaviour.onObjetivoTouched += () => TeleportObjetivo(audioSource);  // Pasamos el audioSource al manejador
    }

    void TeleportObjetivo(AudioSource audioSource)
    {
        if (spawnedObjetivo != null)
        {
            _spawner.Despawn(spawnedObjetivo);
            SpawnObjetivo();
            points++;
            Debug.Log("Puntos: " + points);

            // Reproducir sonido cuando el objetivo es tocado
            audioSource.Play();

            // Verificar si alcanzó los puntos de victoria
            if (points >= victoryPoints)
            {
                // Notificar a todos los jugadores sobre la victoria
                ProcedureParameters parameters = new ProcedureParameters();
                parameters.Set("playerName", "Perdiste");  // Puedes personalizar el mensaje
                parameters.Set("playerScore", points);   // Puntaje del jugador
                
                _multiplayer.InvokeRemoteProcedure("NotifyVictory", UserId.All, parameters);
                // Cargar la escena
                ChangeScene("Ganaste", points);
            }
        }
    }
}

public class ObjetivoBehaviour : MonoBehaviour
{
    public delegate void ObjetivoTouchedHandler();
    public event ObjetivoTouchedHandler onObjetivoTouched;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            onObjetivoTouched?.Invoke();
        }
    }
}
*/

using System.Collections;
using UnityEngine.SceneManagement;
using Alteruna;
using UnityEngine;
using Alteruna.Trinity;  // Es posible que ITransportStreamReader esté aquí
using TMPro; // Necesario para TextMeshProUGUI

public class ObjetivoSpawner : MonoBehaviour
{
    private Alteruna.Avatar _avatar;
    private Spawner _spawner;
    private Multiplayer _multiplayer;  // Referencia a la instancia de Multiplayer

    [SerializeField] private int indexToSpawn = 1;
    private GameObject spawnedObjetivo;
    private float respawnTime = 5.0f; // Tiempo de respawn en segundos
    private float timer;
    private float elapsedTime = 0f; // Tiempo transcurrido desde que se activa el spawneo

    private bool isSpawningActive = false; 
    public int points = 0;  // Puntos del jugador
    private const int victoryPoints = 15;  // Puntaje necesario para ganar
    private const float maxGameTime = 30f;

    // Delegado para el procedimiento remoto
    private RemoteProcedure notifyVictoryProcedure;

    [SerializeField] private AudioClip objetivoSound;  // Clip de sonido para el objetivo

    private TextMeshProUGUI scoreText;  // Referencia al texto que muestra el puntaje
    private TextMeshProUGUI timeText;   // Referencia al texto que muestra el tiempo

    private void Awake()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
        _spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        _multiplayer = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Multiplayer>();  // Obtener la referencia

        // Buscar el Canvas por Tag
        GameObject scoreCanvas = GameObject.FindGameObjectWithTag("ScoreCanvas");
        if (scoreCanvas != null)
        {
            // Buscar el componente TextMeshProUGUI dentro del Canvas
            scoreText = scoreCanvas.GetComponentInChildren<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Canvas de puntaje no encontrado. Asegúrate de asignar el Tag 'ScoreCanvas' al Canvas.");
        }

        // Buscar el Canvas por el Tag "TimeCanvas" para el tiempo
        GameObject timeCanvas = GameObject.FindGameObjectWithTag("TimeCanvas");
        if (timeCanvas != null)
        {
            // Buscar el componente TextMeshProUGUI dentro del Canvas
            timeText = timeCanvas.GetComponentInChildren<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Canvas de tiempo no encontrado. Asegúrate de asignar el Tag 'TimeCanvas' al Canvas.");
        }
    }
    
    // Método RPC para notificar a todos los jugadores sobre la victoria
    private void NotifyVictory(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        int playerScore = parameters.Get("playerScore", 0);
        string playerName = parameters.Get("playerName", "Unknown");
        
        Debug.Log($"¡{playerName} ha alcanzado los puntos de victoria!");
        ChangeScene(playerName, playerScore);
    }
    
    private void ChangeScene(string playerName, int playerScore)
    {
        // Puedes guardar estos datos en una clase estática o un script de administración para usarlos en la nueva escena
        PlayerPrefs.SetString("WinnerName", playerName);
        PlayerPrefs.SetInt("WinnerScore", playerScore);
        
        // Aquí puedes poner el nombre de la escena a la que deseas cambiar
        string sceneName = "ScoreScene";  // Nombre de la escena a cargar
        
        // Cargar la escena
        SceneManager.LoadScene(sceneName);
    }
    
    private void Start()
    {
        timer = respawnTime;

        // Registrar RPC al iniciar
        notifyVictoryProcedure = new RemoteProcedure(NotifyVictory);  // Crear el delegado de RemoteProcedure
        _multiplayer.RegisterRemoteProcedure("NotifyVictory", notifyVictoryProcedure);  // Registrar el RPC
    }

    private void Update()
    {
        if (!_avatar.IsMe)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSpawningActive = true;
        }

        // Solo ejecutar la lógica de spawneo si está activa
        if (isSpawningActive)
        {
            // Incrementa el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            // Verifica si el tiempo ha expirado
            if (elapsedTime >= maxGameTime)
            {    
                ProcedureParameters parameters = new ProcedureParameters();
                parameters.Set("playerName", "Ganaste");  // Puedes personalizar el mensaje
                parameters.Set("playerScore", points);   // Puntaje del jugador
                
                _multiplayer.InvokeRemoteProcedure("NotifyVictory", UserId.All, parameters);
                // Cargar la escena
                ChangeScene("Perdiste", points);
            }
            
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (spawnedObjetivo != null)
                {
                    _spawner.Despawn(spawnedObjetivo);
                }
                SpawnObjetivo();
                timer = respawnTime;
            }

            // Actualizar el tiempo en el texto del Canvas
            if (timeText != null)
            {
                float remainingTime = Mathf.Max(0f, maxGameTime - elapsedTime);
                timeText.text = "Tiempo: " + remainingTime.ToString("F2") + " s";  // Mostrar el tiempo restante
            }
        }

        // Actualizar el puntaje en el texto del Canvas
        if (scoreText != null)
        {
            scoreText.text = "Puntaje: " + points.ToString();
        }
    }

    void SpawnObjetivo()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), _avatar.transform.position.y, Random.Range(-8f, 8f));
        spawnedObjetivo = _spawner.Spawn(indexToSpawn, spawnPosition, Quaternion.identity, new Vector3(3f, 3f, 3f));

        // Añadir componente de colisión si no está ya presente
        if (spawnedObjetivo.GetComponent<Collider>() == null)
        {
            spawnedObjetivo.AddComponent<BoxCollider>().isTrigger = true;
        } 

        // Añadir componente AudioSource
        AudioSource audioSource = spawnedObjetivo.AddComponent<AudioSource>();
        audioSource.clip = objetivoSound;
        audioSource.playOnAwake = false;  // No reproducir inmediatamente

        // Añadir script de comportamiento de colisión
        ObjetivoBehaviour behaviour = spawnedObjetivo.AddComponent<ObjetivoBehaviour>();
        behaviour.onObjetivoTouched += () => TeleportObjetivo(audioSource);  // Pasamos el audioSource al manejador
    }

    void TeleportObjetivo(AudioSource audioSource)
    {
        if (spawnedObjetivo != null)
        {
            _spawner.Despawn(spawnedObjetivo);
            SpawnObjetivo();
            points++;
            Debug.Log("Puntos: " + points);

            // Reproducir sonido cuando el objetivo es tocado
            audioSource.Play();

            // Verificar si alcanzó los puntos de victoria
            if (points >= victoryPoints)
            {
                // Notificar a todos los jugadores sobre la victoria
                ProcedureParameters parameters = new ProcedureParameters();
                parameters.Set("playerName", "Perdiste");  // Puedes personalizar el mensaje
                parameters.Set("playerScore", points);   // Puntaje del jugador
                
                _multiplayer.InvokeRemoteProcedure("NotifyVictory", UserId.All, parameters);
                // Cargar la escena
                ChangeScene("Ganaste", points);
            }
        }
    }
}

public class ObjetivoBehaviour : MonoBehaviour
{
    public delegate void ObjetivoTouchedHandler();
    public event ObjetivoTouchedHandler onObjetivoTouched;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            onObjetivoTouched?.Invoke();
        }
    }
}

