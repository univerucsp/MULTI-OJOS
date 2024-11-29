using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public GameObject projectilePrefab; // Prefab del objeto que se lanzará
    public float launchForce = 20.0f; // Fuerza con la que se lanzará el objeto
    public float smoothMoveSpeed = 5.0f; // Velocidad de interpolación

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetPosition;
    private bool useUDP = true; // Variable para controlar si se debe usar UDP

    [HideInInspector]
    public bool canMove = true;

    private Alteruna.Avatar _avatar;
    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;

    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();

        if (_avatar == null)
        {
            Debug.LogError("No se encontró el componente Avatar.");
            return;
        }

        if (!_avatar.IsMe)
        {
            Debug.Log("Este avatar no es del jugador local, saliendo del Start.");
            return;
        }

        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("No se encontró el componente CharacterController.");
        }
        
        // Asignar una luz con color aleatorio al avatar
        Light light = GetComponent<Light>();
        if (light == null)
        {
            light = gameObject.AddComponent<Light>(); // Añadir una luz si no existe
        }
        light.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        light.type = LightType.Point; // Configuración opcional: puede cambiar a otro tipo si es necesario
        light.intensity = 3.0f; // Ajustar la intensidad según sea necesario
        
        try
        {
            // Intentar inicializar la escucha de UDP en el puerto 65433
            udpClient = new UdpClient(65433);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            StartCoroutine(ReceivePosition());
        }
        catch (SocketException e)
        {
            // Si ocurre un error, desactivar el uso de UDP
            useUDP = false;
            Debug.LogWarning($"No se pudo escuchar en el puerto 65433: {e.Message}. Usando solo teclado para moverse.");
        }

        // Inicializar targetPosition con la posición actual
        targetPosition = transform.position;
    }

    private bool receivedPosition = false; // Nueva variable para verificar si se recibieron datos

    void Update()
    {
        if (!_avatar.IsMe)
            return;

        // Movimiento con teclas
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        moveDirection = Vector3.zero;

        if (canMove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection -= transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection -= transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += transform.right;
            }

            moveDirection = moveDirection.normalized * (isRunning ? runningSpeed : walkingSpeed);

        }
        // Aseguramos que el eje Y sea fijo en 2 mientras movemos en X y Z
	Vector3 moveWithFixedY = moveDirection * Time.deltaTime;
	moveWithFixedY.y = -2f; // No permitimos movimiento en Y
	Vector3 cPosition = transform.position;
	if(cPosition.y < 2f) {
		moveWithFixedY.y = 2f;
	} 
	
	// Usamos el CharacterController para aplicar el movimiento
	CollisionFlags flags = characterController.Move(moveWithFixedY);
        
        
        // Mover el controlador con el movimiento del teclado
        //CollisionFlags flags = characterController.Move(moveDirection * Time.deltaTime);
        
        

        // Solo interpolar la posición objetivo si se han recibido datos válidos y no hay colisión
        if (useUDP && receivedPosition)
        {
            Vector3 currentPosition = transform.position;
            Vector3 interpolatedPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * smoothMoveSpeed);
             
        
            Vector3 moveDelta = interpolatedPosition - currentPosition;

            // Verificar si el personaje está colisionando con un obstáculo y ajustar el movimiento
            flags = characterController.Move(moveDelta);
            if ((flags & CollisionFlags.Sides) != 0)
            {
                // Si hay colisión lateral, reducir el movimiento o mantener al objeto "pegado" al obstáculo
                Debug.Log("Colisión detectada. Ajustando el movimiento.");
                receivedPosition = false; // Detener la interpolación hasta que se libere la colisión
            }
        } 

        // Lógica para lanzar un objeto
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    LaunchProjectile();
       // }
    }

    private IEnumerator ReceivePosition()
    {
        while (useUDP)
        {
            IAsyncResult result = udpClient.BeginReceive(null, null);
            bool dataReceived = result.AsyncWaitHandle.WaitOne(100);

            if (dataReceived)
            {
                try
                {
                    byte[] data = udpClient.EndReceive(result, ref remoteEndPoint);
                    string message = Encoding.UTF8.GetString(data);
                    string[] positionStrings = message.Split(',');

                    if (positionStrings.Length == 2 &&
                        float.TryParse(positionStrings[0], out float x) &&
                        float.TryParse(positionStrings[1], out float z))
                    {
                        targetPosition = new Vector3(-x*40, 2, -z*20);
                        //transform.position.y,
                        Debug.Log($"Posición recibida y aplicada: {targetPosition}");
                        receivedPosition = true; // Indicar que se han recibido datos válidos
                    }
                    else
                    {
                        Debug.LogWarning("Datos de posición no válidos.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error al recibir datos: {e.Message}");
                    useUDP = false;
                }
            }
            else
            {
                // No se recibieron datos, no actualizar la posición objetivo
                receivedPosition = false;
            }

            yield return null;
        }
    }

    private void LaunchProjectile()
    {
        if (projectilePrefab != null)
        {
            // Crear el proyectil en la posición del jugador
            GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, Quaternion.identity);

            // Añadir fuerza al proyectil para lanzarlo hacia adelante
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * launchForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("El prefab del proyectil necesita un componente Rigidbody.");
            }

            Debug.Log("Proyectil lanzado.");
        }
        else
        {
            Debug.LogError("El prefab del proyectil no está asignado.");
        }
    }

    private void OnApplicationQuit()
    {
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null; // Esto es opcional, pero asegura que el objeto se marque como no referenciado.
        }
    }
}

