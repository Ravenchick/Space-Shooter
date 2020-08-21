using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private Player _player;

    private void Awake()
    {
        _player = gameObject.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player.laserDamage(collision);
    }
}
