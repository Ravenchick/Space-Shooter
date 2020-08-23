using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHurtBox : MonoBehaviour
{
    private Player _player;
    private Boss _boss;

    private void Awake()
    {
        _boss = GameObject.Find("Boss").GetComponent<Boss>();
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player.Damage();
        }

        if (collision.gameObject.CompareTag("Laser"))
        {
            _boss.damage();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Power Up"))
        {
            Destroy(collision.gameObject);
        }

    }
}
