/*
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public Vector3 moveDirection = Vector3.forward; // Dirección en la que se mueve el obstáculo

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Obtiene la cámara principal
    }

    void Update()
    {
        // Mover el obstáculo en la dirección indicada
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Comprobar si el obstáculo está fuera de la vista de la cámara
        if (!IsVisibleFrom(mainCamera))
        {
            Destroy(gameObject); // Destruir el objeto si está fuera de la vista
        }
    }

    // Método que verifica si el objeto está dentro del campo de visión de la cámara
    private bool IsVisibleFrom(Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);
    }
}
*/


/*
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    private Vector3 moveDirection; // Dirección fija del movimiento del obstáculo

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Obtiene la cámara principal
        moveDirection = GetInitialMoveDirection(); // Establece la dirección inicial al crearse el objeto
    }

    void Update()
    {
        // Mover el obstáculo en la dirección inicial fija
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Comprobar si el obstáculo está fuera de la vista de la cámara
        if (!IsVisibleFrom(mainCamera))
        {
            Destroy(gameObject); // Destruir el objeto si está fuera de la vista
        }
    }

    private Vector3 GetInitialMoveDirection()
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

        // Si no se presiona ninguna tecla, se mueve hacia adelante por defecto
        if (direction == Vector3.zero)
        {
            direction = Vector3.forward;
        }

        return direction.normalized; // Devuelve la dirección inicial normalizada
    }

    // Método que verifica si el objeto está dentro del campo de visión de la cámara
    private bool IsVisibleFrom(Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);
    }
}
*/
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    private Vector3 moveDirection; // Dirección fija del movimiento del obstáculo

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Obtiene la cámara principal
        moveDirection = GetInitialMoveDirection(); // Establece la dirección inicial al crearse el objeto
    }

    void Update()
    {
        // Mover el obstáculo solo si tiene una dirección válida
        if (moveDirection != Vector3.zero)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        // Comprobar si el obstáculo está fuera de la vista de la cámara
        //if (!IsVisibleFrom(mainCamera))
        //{
        //    Destroy(gameObject); // Destruir el objeto si está fuera de la vista
        //}
    }

    private Vector3 GetInitialMoveDirection()
    {
        Vector3 direction = Vector3.zero;

        // Detectar las teclas presionadas para determinar la dirección
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

        // Si no se presiona ninguna tecla, la dirección permanece en cero
        return direction.normalized; // Devuelve la dirección inicial normalizada
    }

    // Método que verifica si el objeto está dentro del campo de visión de la cámara
    private bool IsVisibleFrom(Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);
    }
}

