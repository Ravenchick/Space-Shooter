﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    private Player _player;

    [SerializeField]
    private int points=15;

    private Animator _animator;

    private Collider2D _collider;

    private AudioSource _audio;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioClip _shieldDown;

    [SerializeField]
    private GameObject _shield;
    private bool _isShieldActive = false;

    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _audio = GetComponent<AudioSource>();
        //_shield = GetComponent<GameObject>();
    }
    void Start()
    {
        transform.position = new Vector3(Random.Range(-7.42f, 7.75f), 6.49f, 0f);
        StartCoroutine(scoring());
        _audio.clip = _explosionSound;

        float gotshield = Random.Range(0f, 10f);

        if(gotshield > 7.5)
        {
            _shield.SetActive(true);
            _isShieldActive = true;
        }
        else
        {
            _shield.SetActive(false);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);

        //para moverlo a un punto aleatorio en la parte de arriba
        if(transform.position.y < -6.63f)
        {
            transform.position = new Vector3(Random.Range(-7.42f, 7.75f), 6.49f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Laser")
        {      
            if (_isShieldActive == false)
            {
                _audio.clip = _explosionSound;
                _audio.Play();
                speed = 0;
                _animator.SetTrigger("Explotion");
                Destroy(gameObject, 2.38f);
                Destroy(other.gameObject);
                _collider.enabled = false;

                if (_player != null)
                {
                    _player.AddScore(points);
                }
            }
            else
            {
                _audio.clip = _shieldDown;
                _audio.Play();
                _shield.SetActive(false);
                _isShieldActive = false;
                Destroy(other.gameObject);
            }
        }
        
        if (other.gameObject.tag == "ProtonLaser")
        {
            _audio.clip = _explosionSound;
            _audio.Play();
            speed = 0;
            _animator.SetTrigger("Explotion");
            Destroy(gameObject, 2.38f);            
            _collider.enabled = false;
            _shield.SetActive(false);

            if (_player != null)
            {
                _player.AddScore(points);
            }
        }

        if (other.gameObject.tag == "Player")
        {
            _audio.clip = _explosionSound;
            _audio.Play();
            _animator.SetTrigger("Explotion");
            speed = 0;
            Destroy(gameObject, 2.38f);
            _collider.enabled = false;
            _shield.SetActive(false);

            if (_player != null)
            {
                _player.Damage();
            }
        }
        
        if (other.gameObject.tag == "Clear")
        {
            gameObject.SetActive(false);
        }

    }
    IEnumerator scoring()
    {
        points--;

        yield return new WaitForSeconds(1f);

        StartCoroutine(scoring());
    }

    
}
