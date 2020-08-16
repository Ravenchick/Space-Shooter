using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float speed;
            
    private Vector3 direction;

    [SerializeField]
    private int PowerUpId;

    private Transform _player;

    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();
    }


    // Start is called before the first frame update
    void Start()
    {
        

        if (transform.position.x > 0)
        {
            direction = new Vector3(-1f, 0f, 0f);
        }
        else if (transform.position.x < 0)
        {
            direction = new Vector3(1f, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement();

        if (_player == null)
        {
            Destroy(gameObject);
        }

        if(transform.position.x > 11.50f || transform.position.x < -11.50f)
        {
            Destroy(gameObject);
        }
    }

    void movement()
    {

        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);

            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                switch (PowerUpId)
                {
                    case 0:
                        player.TripleShootPowerUp();
                        break;
                    case 1:
                        player.SpeedPower();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    case 3:
                        player.reloadAmmo();
                        break;

                }
            }
        }

        if (collision.gameObject.tag == "Clear")
        {
            gameObject.SetActive(false);
        }
    }

}

