using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [Header("Movement")]


    [SerializeField]
    [Tooltip("Z Axis")]
    float forwardSpeed = 30.0F;

    [SerializeField]
    [Tooltip("X Axis")]
    float strafeSpeed = 8.0F;

    [SerializeField]
    [Tooltip("Y Axis")]
    float hoverSpeed = 3.5F;

    [Header("Acceleration")]

    [SerializeField]
    float forwardAccelaration = 2.5F;

    [SerializeField]
    float strafeAccelaration = 2.0F;

    [SerializeField]
    float hoverAccelaration = 2.0F;

    [Header("Roll")]
    float _rollSpeed = 85.0F;

    [SerializeField]
    float rollAcceleration = 3.5F;

    [SerializeField]
    float idleSpeed = 20.0F;

    [Header("Shooting")]

    [SerializeField]
    GameObject LaserPrefab;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    float fireTime;

    float _currentTime;
    /*float ultimo_disparo = 0.0F; */

    [Header("Condicion GameOver")]
    public GameObject PantallaGO;



    Rigidbody _rb;

    float _activeForwardSpeed;
    float _activeStrafeSpeed;
    float _activeHoverSpeed;


    float _lookRateSpeed = 75.0F;

    float _rollInput;

    float pauseTime = 2.0F;



    bool canShoot = true;

    Vector2 _lookInput;
    Vector2 _screenCenter;
    Vector2 _mouseDistance;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();    
    }

    void Start()
    {
        _screenCenter = new Vector2(Screen.width * 0.5F, Screen.height * 0.5F);    
    }

    void Update()
    {
        
        HandleInputs();

    }

    void FixedUpdate()
    {
        transform.Rotate(-_mouseDistance.y *_lookRateSpeed * Time.fixedDeltaTime, _mouseDistance.x * _lookRateSpeed * Time.fixedDeltaTime, _rollInput * _rollSpeed * Time.fixedDeltaTime, Space.Self);

        _rb.position += transform.forward * _activeForwardSpeed * Time.fixedDeltaTime;
        _rb.position += transform.right * _activeStrafeSpeed * Time.fixedDeltaTime;
        _rb.position += transform.up * _activeHoverSpeed * Time.fixedDeltaTime;
    }

    void HandleInputs()
    {
        if (Input.GetAxisRaw("Fire1") == 1 && canShoot)
        {
            StartCoroutine(FireLaser());
            canShoot = false;
            StartCoroutine(WaitPauseShoot());
        }

        _lookInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        _mouseDistance = new Vector2((_lookInput.x - _screenCenter.x) / _screenCenter.x, (_lookInput.y - _screenCenter.y) / _screenCenter.y);

        _mouseDistance = Vector2.ClampMagnitude( _mouseDistance, 1.0F);

        _rollInput = Mathf.Lerp(_rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

        float currentForwardSpeed = Input.GetAxisRaw("Forward") * forwardSpeed;
        _activeForwardSpeed = Mathf.Lerp(_activeForwardSpeed, currentForwardSpeed, forwardAccelaration * Time.deltaTime);

        float currentStrafeSpeed = Input.GetAxisRaw("Horizontal") * strafeSpeed;
        _activeStrafeSpeed = Mathf.Lerp(_activeStrafeSpeed, currentStrafeSpeed, strafeAccelaration * Time.deltaTime);

        float currentHoverSpeed = Input.GetAxisRaw("Hover") * hoverSpeed;
        _activeHoverSpeed = Mathf.Lerp(_activeHoverSpeed, currentHoverSpeed, hoverAccelaration * Time.deltaTime);


        if (Input.GetAxisRaw("Forward") == 0)
        {
            _activeForwardSpeed = idleSpeed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            PantallaGO.SetActive(true);
            canShoot = false;
            Time.timeScale = 0.0f;
        }

    }

    IEnumerator WaitPauseShoot()
    {
        yield return new WaitForSeconds(pauseTime);
        canShoot = true;

    }

    IEnumerator FireLaser()
    {
        for (int i = 0; i < 2; i++)
        {
            _currentTime += Time.deltaTime;
            Instantiate(LaserPrefab, firePoint.position, firePoint.rotation);
            yield return new WaitForSeconds(0.1F);
        }
    }
}
