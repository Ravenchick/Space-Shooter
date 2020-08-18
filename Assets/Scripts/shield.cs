using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    SpriteRenderer _spritecolor;
    [SerializeField]
    private Color _damageColor;
    [SerializeField]
    private Color _dangerColor;
    private Color _base = new Color(1, 1, 1, 1);    
    
    private void Awake()
    {
        _spritecolor = GetComponent<SpriteRenderer>();
        
    }
    
    public void Damage(int lives)
    {
        
        if(lives == 2)
        {
            _spritecolor.color = _damageColor;
        }
        else if(lives == 1)
        {
            _spritecolor.color = _dangerColor;
        }
    }

    public void shieldReset()
    {
        _spritecolor.color = _base;
    }

    
}
