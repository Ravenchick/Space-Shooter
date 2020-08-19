using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float Speed;

    [SerializeField]
    private int _shooterTag;

    [SerializeField]
    private bool _zigZagBeam = false;

    private float _xSpeed = 0f;
    void Start()
    {
        if (_zigZagBeam == true)
        {
            Speed = 6f;
            StartCoroutine(zigZagLaser());
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement();

        if (transform.position.y > 8.81f)
        {
            Destroy(gameObject);
        }

       
    }

    void movement()
    {
        int _direction;
        if(_shooterTag == 0)
        {
            _direction = 1;
        }
        else
        {
            _direction = -1;
        }
                
        Vector3 direction = new Vector3(_xSpeed, _direction, 0);

        transform.Translate(direction * Speed * Time.deltaTime);

        
    }


    IEnumerator zigZagLaser()
    {
        _xSpeed = 0.75f;
        yield return new WaitForSeconds(0.25f);

        while (true)
        {            
            _xSpeed *= -1;
            yield return new WaitForSeconds(0.5f);
        }
    }

}
