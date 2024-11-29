using System.Collections;
using Alteruna;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    public Alteruna.Avatar avatar; // Referencia al avatar de Alteruna
    private Spawner _spawner; // Spawner de Alteruna
    public int circlePrefabIndex; // Índice del prefab del círculo en el Spawner
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10); // Tamaño del área de aparición
    public float respawnTime = 10.0f; // Tiempo en segundos antes de que el círculo se mueva automáticamente
    private float timer; // Temporizador para el tiempo de respawn
    private GameObject currentCircle; // Referencia al círculo actual

    private void Awake()
    {
        _spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
    }

    private void Start()
    {
        if (avatar.IsMe) // Solo el jugador local crea y sincroniza el círculo
        {
            SpawnCircle();
            timer = respawnTime; // Inicializar el temporizador
        }
    }

    private void Update()
    {
        if (!avatar.IsMe)
            return;

        // Decrementar el temporizador
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            TeleportCircle();
            timer = respawnTime; // Reiniciar el temporizador
        }
    }

    private void SpawnCircle()
    {
        if (currentCircle != null)
        {
            _spawner.Despawn(currentCircle); // Despachar el círculo existente en red
        }

        // Generar una posición aleatoria dentro del área definida
        Vector3 spawnPosition = GetRandomPosition();

        // Instanciar el círculo de forma sincronizada en la red
        currentCircle = _spawner.Spawn(circlePrefabIndex, spawnPosition, Quaternion.identity);
        
        if (currentCircle != null)
        {
            CircleBehaviour behaviour = currentCircle.GetComponent<CircleBehaviour>();
            behaviour.spawnAreaSize = spawnAreaSize; // Asignar el tamaño del área al comportamiento del círculo
            behaviour.onCircleTouched += TeleportCircle; // Suscribirse al evento de teletransporte
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        return new Vector3(x, transform.position.y, z);
    }

    private void TeleportCircle()
    {
        if (currentCircle != null)
        {
            Vector3 newPosition = GetRandomPosition();
            currentCircle.transform.position = newPosition;
            Debug.Log("Círculo teletransportado a: " + newPosition);
        }
    }
}

public class CircleBehaviour : MonoBehaviour
{
    public Vector3 spawnAreaSize;
    public delegate void CircleTouchedHandler();
    public event CircleTouchedHandler onCircleTouched;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            // Invocar el evento de teletransporte cuando es tocado por un Rigidbody
            onCircleTouched?.Invoke();
            Debug.Log("Círculo tocado y teletransportado.");
        }
    }
}
