﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private float initialSpeed;
    [SerializeField]
    private GameObject _laser;
    
    [SerializeField]
    private GameObject _TripleLaser;
    [SerializeField]
    private GameObject _protonLaser;
    private bool _isProtonLaserActive;
    [SerializeField]
    private float _protonLaserDuration = 5f;
    [SerializeField]
    private GameObject _raLaser;
    [SerializeField]
    private float _raLaserDuration = 9f;
    private bool _isRaLaserActive = false;

    [SerializeField]
    private float _sprintSpeed = 15f;
    private float _speedBackUp;
    private bool _isSprinting = false;
    private bool _canSprint = true;

    private Transform _shootingPoint;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    private float initalFireRate;
    [SerializeField]
    private int _live = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _ammoAmount = 15;

    private bool TripleShootReady = false;
    [SerializeField]
    private float TripleShootFireRate;
    [SerializeField]
    private float raShootFireRate;
    [SerializeField]
    private float TripleShootDuration;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private float _speedBoost;
    [SerializeField]
    private float _speedBoostDuration;
    [SerializeField]
    private bool _isShieldActivate = false;    
    private int _shieldLives = 3;
    

    private UiManager _UiManager;

    private GameObject Shield;

    private shield _shieldscript;

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
    [SerializeField]
    private AudioClip _notAmmo;

    private CameraShaking _shaking;

    private void Awake()
    {
        Shield = GameObject.Find("Shield");
        _shootingPoint = transform.Find("Shooting Point");

        _spawnManager = GameObject.Find("Spawner").GetComponent<SpawnManager>();

        _UiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

        _audio = GetComponent<AudioSource>();

        _shieldscript = GameObject.Find("Shield").GetComponent<shield>();

        _shaking = GameObject.Find("Main Camera").GetComponent<CameraShaking>();

        
        
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

        StaminaBar.instance.ConsumeStamina(1);

        initialSpeed = speed;
        initalFireRate = _fireRate;
    }

    
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoAmount> 0)
        {
            ShootLaser();
            if (_isRaLaserActive == false)
            {
                _ammoAmount--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoAmount <= 0)
        {
            PlayNoAmmoSound();
        }        

        //Max ammo amount
        if (_ammoAmount > 40)
        {
            _ammoAmount = 40;
        }


        ammoAmount();

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

        

        if (Input.GetKey(KeyCode.LeftShift) && _isSpeedBoostActive == false && _canSprint == true)
        {
            speed = _sprintSpeed;
            StaminaBar.instance.ConsumeStamina(1);
            _isSprinting = true;
            StopCoroutine(RecoverStamina());
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && _isSpeedBoostActive == false && _canSprint == true)
        {
            speed = _speedBackUp;            
            StartCoroutine(RecoverStamina());
            _isSprinting = false;
        }

        
        if(StaminaBar.instance._currentStamina >= 100)
        {
            StopCoroutine(RecoverStamina());
            _canSprint = true;
        }
        if(StaminaBar.instance._currentStamina <= 0)
        {
            speed = _speedBackUp;
            _isSprinting = false;
            _canSprint = false;
            StaminaBar.instance._currentStamina = 1;
            StartCoroutine(RecoverStamina());
        }
    }

    private IEnumerator RecoverStamina()
    {
        yield return new WaitForSeconds(1.5f);
        while(_isSprinting == false)
        {
            StaminaBar.instance.RecoverStamina(1);
            yield return new WaitForSeconds(0.1f);
        }
    }    

    void ShootLaser()
    {        
        _canFire = Time.time + _fireRate;

        if (TripleShootReady == true && _isProtonLaserActive == false && _isRaLaserActive == false)
        {
            Instantiate(_TripleLaser, _shootingPoint.position, Quaternion.identity);
            

            
        }
        else if(_isProtonLaserActive == true && _isRaLaserActive == false)
        {
            Instantiate(_protonLaser, _shootingPoint.position, Quaternion.identity);
            

        }
        else if(_isRaLaserActive == true)
        {
            Instantiate(_raLaser, _shootingPoint.position, Quaternion.identity);
        }

        else
        {
            Instantiate(_laser, _shootingPoint.position, Quaternion.identity);
            
            

        }       
        PlayLaserSound();

    }

    public void Damage()
    {
        if (_isShieldActivate == true && _shieldLives > 1)
        {            
            _shieldLives--;
            _shieldscript.Damage(_shieldLives);
            return;
        }
        else if (_isShieldActivate == true && _shieldLives <= 1)
        {
            _isShieldActivate = false;
            Shield.SetActive(false);
            _shieldLives = 3;
            return;
        }

        _live--;
        StartCoroutine(_shaking.Shaking(0.15f, 0.4f));
        _UiManager.UpdateLives(_live);
        DamageEffect();

        if (_live < 1)
        {
            PlayExplosionSound();
            Destroy(gameObject);
            _UiManager.GameOver();
            _spawnManager.OnPLayersDead();
            _shaking.RestarPosition();
        }

    }

    public void laserDamage(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy Laser"))
        {
            Damage();
            Destroy(collision.gameObject);
        }
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        
      
        Transform _collisionTransform = collision.GetComponent<Transform>();
        if (collision.gameObject.tag == "Power Up" && Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(movingToPlayer(_collisionTransform));            
        }

      
    }

    IEnumerator movingToPlayer(Transform _powerUpPosition)
    {
        while (true)
        {
            if (_powerUpPosition != null)
            {
                _powerUpPosition.transform.position = Vector3.Lerp(_powerUpPosition.transform.position, transform.position, 5f * Time.deltaTime);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void TripleShootPowerUp()
    {
        TripleShootReady = true;
        StartCoroutine(TripleShootPower());
        _ammoAmount += 10;
    }

    public IEnumerator TripleShootPower()
    {
        PlayPowerUpSound();        
        TripleShootReady = true;
        _ammoAmount += 10;
        _fireRate = TripleShootFireRate;

        yield return new WaitForSeconds(TripleShootDuration);

        TripleShootReady = false;

        _fireRate = initalFireRate;


    }

    public void activateProtonLaser()
    {
        PlayPowerUpSound();
        StartCoroutine(ProtonLaserPower());
        _ammoAmount += 10;
    }

    IEnumerator ProtonLaserPower()
    {
        _isProtonLaserActive = true;
        yield return new WaitForSeconds(_protonLaserDuration);
        _isProtonLaserActive = false;
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

        speed = initialSpeed;
        _speedEffect.gameObject.SetActive(false);
        _thruster.gameObject.SetActive(true);
        _isSpeedBoostActive = false;

    }

    public void ActivateShield()
    {
        PlayPowerUpSound();
        _shieldLives = 3;
        _shieldscript.shieldReset();
        _isShieldActivate = true;
        Shield.SetActive(true);
    }

    public void startRaPower()
    {
        StartCoroutine(raPowerActive());
    }

    [SerializeField]
    Color _color;
    SpriteRenderer _sprite;
    IEnumerator raPowerActive()
    {     
                
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = _color;

        PlayPowerUpSound();        
        _fireRate = raShootFireRate;

        _isRaLaserActive = true;

        yield return new WaitForSeconds(_raLaserDuration);
        

        _isRaLaserActive = false;

        if (TripleShootReady == true)
        {
            _fireRate = TripleShootFireRate;
        }
        else
        {
            _fireRate = initalFireRate;
        }

        Damage();

        _sprite.color = Color.white;
    }

    void ammoAmount()
    {
        _UiManager.ammo(_ammoAmount);
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

    void DamageRepair()
    {
        if(_damageEffect[0].activeSelf == true)
        {
            _damageEffect[0].gameObject.SetActive(false);
            A--;
        }
        else 
        {
            _damageEffect[1].gameObject.SetActive(false);
            B++;
        }
    }

    public void reloadAmmo()
    {
        _ammoAmount += 15;
        PlayPowerUpSound();

    }

    public void recoverHealth()
    {
        PlayPowerUpSound();
        if (_live < 3)
        {
            _live++;
            _UiManager.UpdateLives(_live);
            DamageRepair();
        }
        if (_live == 3)
        {
            A = 0;
            B = 2;              
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

    void PlayNoAmmoSound()
    {
        _audio.clip = _notAmmo;
        _audio.Play();
    }
}
