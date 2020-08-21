using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemy;
    private bool _spawning = true;

    [SerializeField]
    private GameObject[] powerBoost;

    Vector3 _position;

    [SerializeField]
    private GameObject _asteroid;
    
    //Increasing difficulty
    private float _enemyA = 1.5f;
    private float _enemyB = 3f;
    private float _boostA = 16f;
    private float _boostB = 20f;
    private float _asteroidA = 16f;
    private float _asteroidB = 20f;

    //Clearing
    [SerializeField]
    private GameObject _clear;

    private int waveNumber;

    private void Awake()
    {
        
        
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
        StartCoroutine(waveSystem());

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private float goingFaster = 0;
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(enemySpawn());        
        yield return new WaitForSeconds(7 - goingFaster);
        StartCoroutine(SpawnBasicPowerUps());
        yield return new WaitForSeconds(4f - goingFaster);
        StartCoroutine(AsteroidSpawn());
        StartCoroutine(ProtonSpawn());
        yield return new WaitForSeconds(2f);
        StartCoroutine(BoostSpawn());

        goingFaster += 1f;
        
    }

    void stopAndContinue(bool _onOff)
    {
        if (_onOff == false)
        {
            _spawning = false;
            StopCoroutine(enemySpawn());
            StopCoroutine(SpawnBasicPowerUps());
            StopCoroutine(ProtonSpawn());
            StopCoroutine(BoostSpawn());
            StopCoroutine(AsteroidSpawn());
        }
        else
        {
            _spawning = true;
            StartCoroutine(StartGame());
        }
    }

    IEnumerator enemySpawn()
    {
        while (_spawning ==true)
        {
            int enemyId = 0;
            float enemyRandomizer = Random.Range(0f, 10f);

            switch (waveNumber)
            {
                case 1:
                    if (enemyRandomizer < 7)
                    {
                        enemyId = 0;
                    }
                    else
                    {
                        enemyId = 1;
                    }
                    break;
                case 2:
                    if (enemyRandomizer < 4)
                    {
                        enemyId = 0;
                    }
                    else if (enemyRandomizer < 6)
                    {
                        enemyId = 1;
                    }
                    else if (enemyRandomizer < 8)
                    {
                        enemyId = 2;
                    }
                    else
                    {
                        enemyId = 3;
                    }
                    break;
                case 3:
                    if (enemyRandomizer < 1.8f)
                    {
                        enemyId = 5;
                    }
                    else if (enemyRandomizer < 3.5f)
                    {
                        enemyId = 1;
                    }
                    else if (enemyRandomizer < 5.5f)
                    {
                        enemyId = 2;
                    }
                    else if (enemyRandomizer < 6.5)
                    {
                        enemyId = 3;
                    }
                    else if (enemyRandomizer < 8)
                    {
                        enemyId = 4;
                    }
                    else
                    {
                        enemyId = 0;
                    }
                    break;
            }


            float _timer = Random.Range(_enemyA, _enemyB);
            Vector3 _position = new Vector3(Random.Range(-7.42f, 7.75f), 6.49f, 0f);
            GameObject newEnemy = Instantiate(_enemy[enemyId], _position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(_timer);
        }
        

    }

    public void OnPLayersDead()
    {
        _spawning = false;
        _clear.gameObject.SetActive(true);
    }

    IEnumerator BoostSpawn()
    {        
        while (_spawning == true)
        {
            float _timer = Random.Range(_boostA, _boostB);
            float _randomizerPosition = Random.Range(-1f, 1f);
            int _boostRandomizer = Random.Range(0, 3);
            float randomHigh = Random.Range(-2.73f, 4.55f);
            
            if (_randomizerPosition < 0)
            {                
                
                _position = new Vector3(-11.38f, randomHigh, 0f);
                
                              

            }
            

            else if (_randomizerPosition > 0)
            {
                
                _position = new Vector3(11.38f, randomHigh, 0f);
                
            }

            GameObject newPowerUp = Instantiate(powerBoost[_boostRandomizer], _position, Quaternion.identity);
            newPowerUp.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(_timer);

            int raBoost = Random.Range(0, 4);
            if(raBoost < 3)
            {
                GameObject newRaPowerUp = Instantiate(powerBoost[6], _position, Quaternion.identity);
                newRaPowerUp.transform.parent = gameObject.transform;
                yield return new WaitForSeconds(_timer);
            }            
        }


    }

    

    IEnumerator AsteroidSpawn()
    {
        while (_spawning == true)
        {
            float _timer = Random.Range(_asteroidA, _asteroidB);
            float _randomizerPosition = Random.Range(-1f, 1f);
            
            float randomHigh = Random.Range(-0.73f, 4.55f);

            if (_randomizerPosition < 0)
            {

                _position = new Vector3(-11.38f, randomHigh, 0f);



            }


            else if (_randomizerPosition > 0)
            {

                _position = new Vector3(11.38f, randomHigh, 0f);

            }

            GameObject newPowerUp = Instantiate(_asteroid, _position, Quaternion.identity);
            newPowerUp.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(_timer);
        }


    }

    [SerializeField]
    private float firstWaveDuration;
    [SerializeField]
    private float secondWaveDuration;
    [SerializeField]
    private float thirdWaveDuration;
    [SerializeField]
    private float waveResetTime;
    IEnumerator waveSystem()
    {
        //first wave
        waveNumber = 1;
        yield return new WaitForSeconds(firstWaveDuration);
        stopAndContinue(false);
        yield return new WaitForSeconds(waveResetTime);

        //second wave
        waveNumber = 2;
        Debug.Log("Second wave on");
        stopAndContinue(true);
        gettingHard();
        yield return new WaitForSeconds(secondWaveDuration);
        stopAndContinue(false);
        yield return new WaitForSeconds(waveResetTime);

        //third and final wave
        waveNumber = 3;
        stopAndContinue(true);
        gettingHard();
        yield return new WaitForSeconds(thirdWaveDuration);
        stopAndContinue(false);
        yield return new WaitForSeconds(waveResetTime + 3f);

        //Boss come in

        

    }
    void gettingHard()
    {
        _enemyA -= 0.3f;
        _enemyB -= 0.3f;
        _boostA -= 2f;
        _boostB -= 2f;
        _asteroidA -= 2f;
        _asteroidB -= 2f;
    }
    IEnumerator SpawnBasicPowerUps()
    {
        while (_spawning == true)
        {
            float _timer = Random.Range(7f, 10f);
            float _randomizerPosition = Random.Range(-1f, 1f);
            float _boostRandomizer = Random.Range(0, 10);
            float randomHigh = Random.Range(-2.73f, 4.55f);
            int _boostSelected;

            if (_randomizerPosition < 0)
            {

                _position = new Vector3(-11.38f, randomHigh, 0f);



            }


            else if (_randomizerPosition > 0)
            {

                _position = new Vector3(11.38f, randomHigh, 0f);

            }

            if(_boostRandomizer > 7)
            {
                _boostSelected = 4;
            }
            else
            {
                _boostSelected = 3;
            }

            GameObject newPowerUp = Instantiate(powerBoost[_boostSelected], _position, Quaternion.identity);
            newPowerUp.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(_timer);
        }
    }
    IEnumerator ProtonSpawn()
    {
        while (_spawning == true)
        {
            float _isSpawning = Random.Range(0f, 10f);
            int _specialBoost = Random.Range(5, 7);
            if (_isSpawning < 7)
            {


                float _randomizerPosition = Random.Range(-1f, 1f);
                float randomHigh = Random.Range(-2.73f, 4.55f);

                if (_randomizerPosition < 0)
                {

                    _position = new Vector3(-11.38f, randomHigh, 0f);



                }

                else if (_randomizerPosition > 0)
                {

                    _position = new Vector3(11.38f, randomHigh, 0f);

                }

                GameObject newPowerUp = Instantiate(powerBoost[_specialBoost], _position, Quaternion.identity);
                newPowerUp.transform.parent = gameObject.transform;
                
            }
            yield return new WaitForSeconds(7f);
        }
    }
}
