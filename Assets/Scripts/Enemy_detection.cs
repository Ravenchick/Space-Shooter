using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_detection : MonoBehaviour
{
    [SerializeField]
    private EnemyController _enemy;    
    

    private void Awake()
    {
        //_enemy = GetComponent<EnemyController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemy.playerDetected(collision);
    }
}
