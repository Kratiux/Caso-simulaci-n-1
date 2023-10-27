using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField]
    float force;

    [SerializeField]
    [Tooltip("Define el tiempo de vida del laser despues de disparado (Por defecto es 5)")]
    float LaserTTL = 5.0F;
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * force;
        StartCoroutine(VidaLaser());
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    IEnumerator VidaLaser()
    {
        yield return new WaitForSeconds(LaserTTL);
        Destroy(gameObject);
    }
}
