using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textScore;
    [SerializeField]
    private TextMeshProUGUI _textAmmo;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _LivesImg;
    private GameObject _gameOver;
    private GameObject _powerUp;
    [SerializeField]
    private float _gameOverAnimationRatio = 0.5f;
    private GameObject _TryAgain;
    private bool _isGameOver;

    private void Awake()
    {
        _gameOver = GameObject.Find("Game over_text");
        _TryAgain = GameObject.Find("Reset_text");
        
    }
    void Start()
    {
        _textScore.text = "Score: 0";
        _gameOver.SetActive(false);
        _TryAgain.SetActive(false);

        if(_gameOver == null)
        {
            Debug.LogError("No se ha encontrado el game over");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r") && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }
       

    }

    public void score(int playerScore)
    {
        _textScore.text = "Score: " + playerScore;
    }

    public void ammo(int totalAmmo)
    {
        _textAmmo.text = "Ammo: " + totalAmmo;
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOver()
    {
        StartCoroutine(GameOverAnimation());
        _TryAgain.SetActive(true);
        _isGameOver = true;
    }

    IEnumerator GameOverAnimation()
    {
        while (true)
        {
            _gameOver.SetActive(true);
            yield return new WaitForSeconds(_gameOverAnimationRatio);
            _gameOver.SetActive(false);
            yield return new WaitForSeconds(_gameOverAnimationRatio);
          
        }
    }
}
