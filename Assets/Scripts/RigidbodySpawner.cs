


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using UnityEngine.UI;


public class CubeSpawner : MonoBehaviour
{
    public Alteruna.Avatar avatar;
    private Spawner _spawner;
    public float spawnInterval = 1.5f; // Intervalo máximo de tiempo entre ejecuciones (en segundos)
    private float lastSpawnTime = 0f; // Tiempo del último spawn
    public bool change = true;
    public bool visibility = false;
    
    private Image image2; 
    private Image image3; 

    [SerializeField] private int indexToSpawn = 0;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 moveDirection = Vector3.forward;
    [SerializeField] private LayerMask despawnLayer;

    private Camera mainCamera;
    private List<GameObject> spawnedCubes = new List<GameObject>(); // Lista para rastrear los cubos

    private void Awake()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        _spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        mainCamera = Camera.main;
    }
    
    void Start()
    {
        // Buscar las imágenes por sus tags
        GameObject image2Object = GameObject.FindGameObjectWithTag("Image2");
        GameObject image3Object = GameObject.FindGameObjectWithTag("Image3");

        if (image2Object != null)
            image2 = image2Object.GetComponent<Image>();
        else
            Debug.LogError("No se encontró un objeto con el tag 'Image2'.");

        if (image3Object != null)
            image3 = image3Object.GetComponent<Image>();
        else
            Debug.LogError("No se encontró un objeto con el tag 'Image3'.");
    }

    private void Update()
    {
        if (!avatar.IsMe)
            return;

        if (Input.GetKeyDown(KeyCode.F) && Time.time - lastSpawnTime >= spawnInterval)
        {
            indexToSpawn = 0;
            SpawnCube();
            lastSpawnTime = Time.time;
        }
        
        if (change) {
        	indexToSpawn = Random.value < 0.5f ? 2 : 3;
        	// Alternar la visibilidad de las imágenes según el índice
		    if (indexToSpawn == 2 && visibility == true)
		    {
		        image2.enabled = false;
		        image3.enabled = true;
		    }
		    else if (indexToSpawn == 3 && visibility == true)
		    {
		        image2.enabled = true;
		        image3.enabled = false;
		    }
        	change = false;
        }
        
        
        if (Input.GetKeyDown(KeyCode.G) && Time.time - lastSpawnTime >= spawnInterval)
	{   
	    SpawnCube();
	    lastSpawnTime = Time.time;
	    change = true;
	    visibility = true;
	}

    }

    void SpawnCube()
    {
        Vector3 spawnDirection = GetSpawnDirection();
        Vector3 spawnPosition = avatar.transform.position + spawnDirection * 5f; // Calcular posición de spawn según dirección

        GameObject newCube = _spawner.Spawn(indexToSpawn, spawnPosition);

        Rigidbody rb = newCube.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = newCube.AddComponent<Rigidbody>();
        }

        spawnedCubes.Add(newCube); // Agregar el cubo a la lista
        StartCoroutine(DespawnCubeAfterDelay(newCube, 3f));
    }

    Vector3 GetSpawnDirection()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector3.right;
        }

        // Si no se presiona ninguna flecha, usa la dirección por defecto hacia adelante
        if (direction == Vector3.zero)
        {
            direction = Vector3.forward;
        }

        return direction.normalized; // Asegurar que la dirección esté normalizada
    }

    void DespawnCube(GameObject cube)
    {
        if (cube != null)
        {
            _spawner.Despawn(cube);
            spawnedCubes.Remove(cube); // Remover el cubo de la lista
        }
    }

    private bool IsVisibleFrom(Camera camera, GameObject cube)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, cube.GetComponent<Collider>().bounds);
    }

    private IEnumerator DespawnCubeAfterDelay(GameObject cube, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (spawnedCubes.Contains(cube)) // Verificar si el cubo sigue activo
        {
            DespawnCube(cube);
        }
    }
}


