using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int currWave = 1;

    public int numOfZombieInScene { get; set; }

    int _playerHP = 100;
    public int plyerHP { 
        get { return _playerHP; } 
        set { if (value < 0 && _playerHP > 0) { _playerHP += value; }
            else if(value > 0 && _playerHP < 100)
            {
                if (_playerHP + value > 100) _playerHP = 100;
                else _playerHP += value;
            }
            text_PlayerHP.text = _playerHP.ToString(); } 
    }

    public int zombieHp = 10;

    [SerializeField] Text text_PlayerHP;

    [SerializeField] int spawnDelay;
    [SerializeField] Transform[] spawnPoint;
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject superZombiePrefab;

    [SerializeField] GameObject pressIToStart;
    [SerializeField] GameObject wave;

    [SerializeField] GameObject planePrefab;

    bool isStart;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isStart)
        {
            WaveStart();
        }
        
        if(numOfZombieInScene == 0 && isStart)
        {
            isStart = false;
            pressIToStart.SetActive(true);
            DropBox();
        }
    }

    void WaveStart()
    {
        isStart = true;

        pressIToStart.SetActive(false);
        wave.SetActive(true);
        wave.GetComponent<Text>().text = "Wave " + currWave.ToString();
        
        StartCoroutine(DeactiveWave());
        StartCoroutine(Spawn());

        numOfZombieInScene = currWave * 5;

        if (currWave % 5 == 0)
        {
            numOfZombieInScene += currWave / 5;
            StartCoroutine(SpawnSuperZombie());
        }

        DropBox();
    }

    IEnumerator Spawn()
    {
        for (int i = 1; i <= 5 * currWave; i++)
        {
            int random1 = Random.Range(0, spawnPoint.Length);
            Instantiate(zombiePrefab, spawnPoint[random1].position, Quaternion.identity);

            int random2 = Random.Range(0, 20);
            if(random2 == 0)
            {
                Instantiate(superZombiePrefab, spawnPoint[random1].position, Quaternion.identity);
            }

            yield return new WaitForSeconds(1f);
        }

        EndSpawn();
    }

    IEnumerator SpawnSuperZombie()
    {
        int num = currWave / 5;

        for(int i = 1; i <= num;  i++)
        {
            int random = Random.Range(0, spawnPoint.Length);
            Instantiate(superZombiePrefab, spawnPoint[random].position, Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator DeactiveWave()
    {
        yield return new WaitForSeconds(3f);
        wave.SetActive(false);
    }

    void DropBox()
    {
        Instantiate(planePrefab);
    }

    void EndSpawn()
    {
        currWave++;
        zombieHp += 3;
    }
}
