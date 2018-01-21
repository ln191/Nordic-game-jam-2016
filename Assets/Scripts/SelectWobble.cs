using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectWobble : MonoBehaviour
{
    private float wobbleTime;
    private Vector3 scale;

    // Use this for initialization
    void Start()
    {
        wobbleTime = Random.Range(0, 1.5f);
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
            wobbleTime += Time.deltaTime;
            
            Vector3 newScale = scale;
            newScale += scale * Mathf.Sin(wobbleTime * 10) * 0.03f;

            transform.localScale = newScale;
    }
}
