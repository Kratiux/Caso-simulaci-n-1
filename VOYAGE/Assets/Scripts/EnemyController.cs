using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]


    [SerializeField]
    [Tooltip("Z Axis")]
    float forwardSpeed = 30.0F;

    [SerializeField]
    float forwardAccelaration = 2.5F;

    [SerializeField]
    float idleSpeed = 5.0F;

    float VidaEnemigo = 10.0F;

    

    public Transform playerTarget;



    float _activeForwardSpeed;

    Rigidbody _rb;

    


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (VidaEnemigo <= 0)
        {
            Destroy(this.gameObject);
        }

        HandleInputs();
        transform.LookAt(new Vector3(playerTarget.position.x, playerTarget.position.y, playerTarget.position.z));
    }

    void FixedUpdate()
    {

        _rb.position += transform.forward * _activeForwardSpeed * Time.fixedDeltaTime;
 
    }

    void HandleInputs()
    {
        

        float currentForwardSpeed = Input.GetAxisRaw("Forward") * forwardSpeed;
        _activeForwardSpeed = Mathf.Lerp(_activeForwardSpeed, currentForwardSpeed, forwardAccelaration * Time.deltaTime);


        if (Input.GetAxisRaw("Forward") == 0)
        {
            _activeForwardSpeed = idleSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            VidaEnemigo--;
            Debug.Log("A un enemigo le quedan " + VidaEnemigo + " de vida.");
            StartCoroutine(DamageCooldown());
        }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.5F);
    }

}
