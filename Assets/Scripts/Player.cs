using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private GameObject _TripleLaser;
    [SerializeField]
    private float _sprintSpeed = 15f;
    private float _speedBackUp;

    private Transform _shootingPoint;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _live = 3;
    private SpawnManager _spawnManager;

    private bool TripleShootReady = false;
    [SerializeField]
    private float TripleShootFireRate;
    [SerializeField]
    private float TripleShootDuration;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private float _speedBoost;
    [SerializeField]
    private float _speedBoostDuration;
    [SerializeField]
    private bool _isShieldActivate = false;
    

    private UiManager _UiManager;

    private GameObject Shield;

    [SerializeField]
    private int Score;

    [SerializeField]
    private GameObject _speedEffect;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject[] _damageEffect;
    
    //To randomize the damage effect order
    private int A = 0;
    private int B = 2;

    private AudioSource _audio;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioClip _powerUpSound;

    private void Awake()
    {
        Shield = GameObject.Find("Shield");
        _shootingPoint = transform.Find("Shooting Point");

        _spawnManager = GameObject.Find("Spawner").GetComponent<SpawnManager>();

        _UiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

        _audio = GetComponent<AudioSource>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null");
        }

        Shield.SetActive(false);
        
        _speedBackUp = speed;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
        }

        
    }

    void CalculateMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontal, vertical, 0f);

        transform.Translate(direction * speed * Time.deltaTime);

        //limitar el movimiento en la Y
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.35f, 5.42f), 0);

        if (transform.position.x >= 13.36f)
        {
            transform.position = new Vector3(-12.35f, transform.position.y, 0);
        }
        else if (transform.position.x <= -12.35f)
        {
            transform.position = new Vector3(13.36f, transform.position.y, 0);
        }

        

        if (Input.GetKey(KeyCode.LeftShift) && _isSpeedBoostActive == false)
        {
            speed = _sprintSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && _isSpeedBoostActive == false)
        {
            speed = _speedBackUp;
        }
    }

    

    void ShootLaser()
    {
        _canFire = Time.time + _fireRate;

        if (TripleShootReady == true)
        {
            Instantiate(_TripleLaser, _shootingPoint.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laser, _shootingPoint.position, Quaternion.identity);
        }
        PlayLaserSound();

    }

    public void Damage()
    {
        if (_isShieldActivate == true)
        {
            _isShieldActivate = false;
            Shield.SetActive(false);
            return;
        }

        _live--;
        _UiManager.UpdateLives(_live);
        DamageEffect();

        if (_live < 1)
        {
            PlayExplosionSound();
            Destroy(gameObject);
            _UiManager.GameOver();
            _spawnManager.OnPLayersDead();
        }

    }

    public void TripleShootPowerUp()
    {
        TripleShootReady = true;
        StartCoroutine(TripleShootPower());
    }

    IEnumerator TripleShootPower()
    {
        PlayPowerUpSound();
        float fireRateBackUp = _fireRate;

        _fireRate = TripleShootFireRate;

        yield return new WaitForSeconds(TripleShootDuration);

        TripleShootReady = false;

        _fireRate = fireRateBackUp;


    }

    public void SpeedPower()
    {
        StartCoroutine(SpeedBoostPower());
    }

    IEnumerator SpeedBoostPower()
    {
        _isSpeedBoostActive = true;
        PlayPowerUpSound();
        _speedEffect.gameObject.SetActive(true);
        _thruster.gameObject.SetActive(false);
        
        
        speed = speed + _speedBoost;

        yield return new WaitForSeconds(_speedBoostDuration);

        speed = _speedBackUp;
        _speedEffect.gameObject.SetActive(false);
        _thruster.gameObject.SetActive(true);
        _isSpeedBoostActive = false;

    }

    public void ActivateShield()
    {
        PlayPowerUpSound();
        _isShieldActivate = true;
        Shield.SetActive(true);
    }

    public void AddScore(int points)
    {
        Score += points;
        _UiManager.score(Score);
    }

    void DamageEffect()
    {        
        int Randomizer = Random.Range(A, B);
        Debug.Log(Randomizer);
        _damageEffect[Randomizer].gameObject.SetActive(true);

        if(Randomizer == 0)
        {
            A++;
        }
        else 
        {
            B--;
        }

        
    }

    void PlayLaserSound()
    {
        _audio.clip = _laserSound;
        _audio.Play();
    }
    
    void PlayExplosionSound()
    {
        _audio.clip = _explosionSound;
        _audio.Play();
    }

    void PlayPowerUpSound()
    {
        _audio.clip = _powerUpSound;
        _audio.Play();
    }
}
