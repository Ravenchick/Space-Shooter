using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject _laser;       
    private Transform _shootingPoint;

    private GameObject _radar;
    private GameObject _hitBox;
    private GameObject _hurtBox;

    private Animator _animator;

    private bool _laserSpin = false;

    [SerializeField]
    private int _lives;

    [SerializeField]
    private GameObject _explosion;

    //audio
    private AudioSource _audio;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioClip _laserSound;
        
    private void Awake()
    {        
        _shootingPoint = GameObject.Find("Boss Shooting Point").GetComponent<Transform>();
        _radar = GameObject.Find("Radar");
        _hitBox = GameObject.Find("HitBox");
        _animator = GetComponent<Animator>();
        _hurtBox = GameObject.Find("Boss Hurt Box");
        _audio = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        transform.position = new Vector3(0, 11, 0);
        _laserSpin = false;
        _hitBox.SetActive(false);
        _radar.SetActive(false);
        StartCoroutine(movement());
    }
    
    IEnumerator movement()
    {
        //moving down the screen
        while (transform.position.y > 1.45f)
        {
            transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _radar.SetActive(true);
        _hitBox.SetActive(true);
        
    }

    private void FixedUpdate()
    {
        if (_laserSpin == true)
        {
            transform.Rotate(0, 0, 1);
        }
    }
    public void startAgression()
    {
        StartCoroutine(playerFound());
    }
   public IEnumerator playerFound()
    {
        int activateLaser = Random.Range(1, 10);
        _radar.SetActive(false);
        _hitBox.SetActive(false);
        Shoot();        
        yield return new WaitForSeconds(0.75f);
        if (activateLaser > 5)
        {

            _animator.SetBool("Laser", true);
            yield return new WaitForSeconds(1.5f);
            _laserSpin = true;
            yield return new WaitForSeconds(2.4f);
            _laserSpin = false;
            _animator.SetBool("Laser", false);
        }
        yield return new WaitForSeconds(1f);
        _radar.SetActive(true);
        _hitBox.SetActive(true);
    }
    public void Shoot()
    {        
        Instantiate(_laser, _shootingPoint.position, transform.rotation);
        shootSound();
    }

    public void rotateClock()
    {
        transform.Rotate(0, 0, -2f);
    }

    public void rotateAgainstClock()
    {
        transform.Rotate(0, 0, 2f);
    }

    public void damage()
    {
        _lives--;

        if(_lives <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(destruction());
        }
    }

    IEnumerator destruction()
    {
        StopCoroutine(playerFound());
        _hurtBox.SetActive(false);
        _animator.SetBool("Laser", false);
        _laserSpin = false;
        _hitBox.SetActive(false);
        _radar.SetActive(false);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        explosionSound();
        yield return new WaitForSeconds(2);        
        Instantiate(_explosion, new Vector3(transform.position.x + 0.25f, transform.position.y + 0.15f, transform.position.z), Quaternion.identity);
        explosionSound();
        yield return new WaitForSeconds(1f);
        Instantiate(_explosion, new Vector3(transform.position.x - 0.2f, transform.position.y - 0.15f, transform.position.z), Quaternion.identity);        
        yield return new WaitForSeconds(1f);
        explosionSound();
        Destroy(gameObject);

    }

    void shootSound()
    {
        _audio.clip = _laserSound;
        _audio.Play();
    }

    void explosionSound()
    {
        _audio.clip = _explosionSound;
        _audio.Play();
    }
}
