using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    private Player _player;

    [SerializeField]
    private int points=15;

    private Animator _animator;

    [SerializeField]
    private GameObject _hurBox;

    private AudioSource _audio;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioClip _shieldDown;

    [SerializeField]
    private GameObject _shield;
    private bool _isShieldActive = false;

    [SerializeField]
    private int enemyId;
    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();        
        _audio = GetComponent<AudioSource>();
        
    }
    void Start()
    {
        transform.position = new Vector3(Random.Range(-7.42f, 7.75f), 6.49f, 0f);
        StartCoroutine(scoring());
        _audio.clip = _explosionSound;

        float gotshield = Random.Range(0f, 10f);

        if (gotshield > 7f && enemyId == 0)
        {
            _shield.SetActive(true);
            _isShieldActive = true;
        }
        else
        {
            Destroy(_shield);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);

        //para moverlo a un punto aleatorio en la parte de arriba
        if(transform.position.y < -6.63f)
        {
            if (enemyId == 1)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = new Vector3(Random.Range(-7.42f, 7.75f), 6.49f, 0f);
            }
        }
    }

    public void BodyCollision(Collider2D other)
    {
        
        if (other.gameObject.tag == "Laser")
        {      
            if (_isShieldActive == false)
            {
                _audio.clip = _explosionSound;
                _audio.Play();
                speed = 0;
                _animator.SetTrigger("Explotion");
                Destroy(gameObject, 2.38f);
                Destroy(other.gameObject);
                _hurBox.SetActive(false);

                if (_player != null)
                {
                    _player.AddScore(points);
                }
            }
            else
            {
                _audio.clip = _shieldDown;
                _audio.Play();
                _shield.SetActive(false);
                _isShieldActive = false;
                Destroy(other.gameObject);
            }
        }
        
        if (other.gameObject.tag == "ProtonLaser")
        {
            _audio.clip = _explosionSound;
            _audio.Play();
            speed = 0;
            _animator.SetTrigger("Explotion");
            Destroy(gameObject, 2.38f);
            _hurBox.SetActive(false);
            _shield.SetActive(false);

            if (_player != null)
            {
                _player.AddScore(points);
            }
        }

        if (other.gameObject.tag == "Player")
        {
            _audio.clip = _explosionSound;
            _audio.Play();

            if (enemyId != 1)
            {
                _animator.SetTrigger("Explotion");
                speed = 0;
                Destroy(gameObject, 2.38f);
                _hurBox.SetActive(false);
                _shield.SetActive(false);
                if (_player != null)
                {
                    _player.Damage();
                }
            }

            else
            {

                _hurBox.SetActive(false);
                if (_player != null)
                {
                    _player.Damage();
                }
            }
        }
        
        if (other.gameObject.tag == "Clear")
        {
            gameObject.SetActive(false);
        }

    }
    IEnumerator scoring()
    {
        points--;

        yield return new WaitForSeconds(1f);

        StartCoroutine(scoring());
    }

    public IEnumerator tarjet(Collider2D _tarjet)
    {
        if (_tarjet.gameObject.tag == "Player")
        {            
            switch (enemyId)
            {
                case 0:
                    break;
                case 1:
                    float _speedBackUp = speed;
                    speed = 0f;
                    yield return new WaitForSeconds(1f);
                    speed = _speedBackUp + 7f;                    
                    break;
            }
        }
    }

    
}
