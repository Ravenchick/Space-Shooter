using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftDetector : MonoBehaviour
{
    private Boss _boss;

    private void Awake()
    {
        _boss = GameObject.Find("Boss").GetComponent<Boss>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _boss.rotateAgainstClock();
        }

    }
}
