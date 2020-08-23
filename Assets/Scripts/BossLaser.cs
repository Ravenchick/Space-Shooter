using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Vector3 _direction;

    private Boss _boss;
    private Transform _bossTransform;

    private void Awake()
    {
        _boss = GameObject.Find("Boss").GetComponent<Boss>();
        _bossTransform = GameObject.Find("Boss").GetComponent<Transform>();
    }
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);
    }
}
