using UnityEngine;
using System.Collections;

public class CamaraFollow : MonoBehaviour
{
    [SerializeField]
    private float xMin, xMax, yMin, yMax;
    public bool hasLost = false;

    Transform target;
    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (hasLost)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(target.position.y, yMin, yMax), transform.position.z);
        }
        else
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(target.position.y, yMin, transform.position.y), transform.position.z);
    }
}
