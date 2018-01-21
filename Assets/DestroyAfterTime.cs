using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    public IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.025f);
        Destroy(gameObject);
    }
}
