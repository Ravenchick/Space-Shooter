using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField]
    private EnemyController _enemyController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemyController.BodyCollision(collision);
    }

}
