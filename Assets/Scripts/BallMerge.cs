using UnityEngine;

public class BallMerge : MonoBehaviour
{
    public GameObject[] mergePrefabs;
    public int prefabIndex = 0;
    private bool isMerging = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isMerging) return;
        BallMerge other = collision.gameObject.GetComponent<BallMerge>();
        if (other == null || other.isMerging) return;
        if (other.prefabIndex != prefabIndex) return;

        int nextIndex = prefabIndex + 1;
        if (nextIndex < mergePrefabs.Length)
        {
            isMerging = true;
            other.isMerging = true;
            Vector3 pos = (transform.position + collision.transform.position) / 2f;

            Destroy(other.gameObject);
            Destroy(gameObject);
            GameObject newObj = Instantiate(mergePrefabs[nextIndex], pos, Quaternion.Euler(0, 180, 0));
            BallMerge mergeScript = newObj.GetComponent<BallMerge>();
            if (mergeScript != null)
            {
                mergeScript.mergePrefabs = mergePrefabs;
                mergeScript.prefabIndex = nextIndex;
            }

            if (ScoreUI.Instance != null)
            {
                int scoreToAdd = (prefabIndex + 1) * 10;
                ScoreUI.Instance.AddScore(scoreToAdd);
            }
        }
    }
}
