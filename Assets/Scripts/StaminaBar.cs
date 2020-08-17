using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider _staminaBar;

    private int maxStamina = 100;
    public int _currentStamina;

    public static StaminaBar instance;

    private void Awake()
    {
        instance = this;
        _staminaBar = GetComponent<Slider>();
        
    }
    private void Start()
    {
        _currentStamina = maxStamina;
        _staminaBar.value = maxStamina;
        _staminaBar.maxValue = maxStamina;
        
    }
    
    public void ConsumeStamina(int amount)
    {
        if (_currentStamina - amount >= 0)
        {
            _currentStamina -= amount;
            _staminaBar.value = _currentStamina;
            
        }
        else
        {
            Debug.Log("Not enought stamina");
        }
    }
    
    public void RecoverStamina(int amount)
    {
        if (_currentStamina <= maxStamina)
        {
            _currentStamina += amount;
            _staminaBar.value = _currentStamina;
        }
    }
}
