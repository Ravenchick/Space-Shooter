using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationRate = 50f;
    private Player _player;   
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private float speed = 5f;
    private Vector3 direction;
    private AudioSource _audio;
    [SerializeField]
    private AudioClip _explosionSound;
    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audio = GetComponent<AudioSource>();
    }
    void Start()
    {
        if (transform.position.x > 0)
        {
            direction = new Vector3(-1f, -0.7f, 0f);
        }
        else if (transform.position.x < 0)
        {
            direction = new Vector3(1f, -0.7f, 0f);
        }
        _audio.clip = _explosionSound;
    }

    
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationRate * Time.deltaTime);

        movement();

        if (_player == null)
        {
            Destroy(gameObject);
        }

        if (transform.position.x > 11.50f || transform.position.x < -11.50f)
        {
            Destroy(gameObject);
        }
    }

    void movement()
    {

        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            _audio.Play();
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);        
            
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            _player.Damage();          
            
        }
        if (collision.gameObject.tag == "Clear")
        {
            gameObject.SetActive(false);
        }
    }
    
    
}
