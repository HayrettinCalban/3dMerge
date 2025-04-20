using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            BallDisableZone zone = FindAnyObjectByType<BallDisableZone>();
            if (zone != null)
            {
                zone.GameOver();
            }
        }
        // Eğer zone tag'ına girerse BallController'ı kapat
        if (other.CompareTag("Zone"))
        {
            BallController ballController = other.GetComponent<BallController>();
            if (ballController != null)
            {
                ballController.enabled = false;
            }
        }
    }
}
