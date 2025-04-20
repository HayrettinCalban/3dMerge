using UnityEngine;

public class BallController : MonoBehaviour
{
    public float forceMultiplier = 2f;
    public LineRenderer lineRenderer;
    public LayerMask groundLayer;
    private Vector3 dragStartPoint;
    private Vector3 dragEndPoint;
    private bool isDragging = false;
    private Rigidbody rb;
    public bool isOnSpawnPoint = true;
    private bool hasThrown = false;
    private SpawnPoint spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.positionCount = 2;
        }
        lineRenderer.enabled = false;
        if (spawnPoint == null)
        {
            spawnPoint = FindAnyObjectByType<SpawnPoint>();
        }
    }

    void Update()
    {
        if (!isOnSpawnPoint || hasThrown) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                dragStartPoint = hit.point;
                isDragging = true;
                lineRenderer.enabled = true;
            }
        }
        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                dragEndPoint = hit.point;
                lineRenderer.SetPosition(0, transform.position);
                Vector3 direction = dragStartPoint - dragEndPoint;
                Vector3 targetPoint = transform.position + new Vector3(direction.x, 0, direction.z);
                lineRenderer.SetPosition(1, targetPoint);
            }
        }
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Vector3 direction = dragStartPoint - dragEndPoint;
            direction.y = 0;
            float distance = direction.magnitude;
            if (distance > 0.1f)
            {
                Vector3 force = direction.normalized * distance * forceMultiplier;
                rb.AddForce(force, ForceMode.Impulse);
                hasThrown = true;
                rb.useGravity = true;
                if (spawnPoint != null)
                {
                    spawnPoint.OnBallLeftSpawn();
                }
            }
            isDragging = false;
            lineRenderer.enabled = false;
        }
    }

    public void SetSpawnState(bool onSpawn)
    {
        isOnSpawnPoint = onSpawn;
        if (!onSpawn)
        {
            lineRenderer.enabled = false;
        }
    }

    public void ResetThrow()
    {
        hasThrown = false;
    }
}