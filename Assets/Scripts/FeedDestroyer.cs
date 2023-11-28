using UnityEngine;

public class FeedDestroyer : MonoBehaviour
{
    public float destroyTime = 4f;

    private void OnEnable() => Destroy(gameObject, destroyTime);
}
