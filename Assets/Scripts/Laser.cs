using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float Speed;
    
    

    
    void Start()
    {
        
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
        Vector3 direction = new Vector3(0, 1, 0);

        transform.Translate(direction * Speed * Time.deltaTime);

        
    }

   

}
