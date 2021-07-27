using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int currWave = 1;
    int numOfZombieBeSpawned = 5;
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
    [SerializeField] Text text_PlayerHP;

    [SerializeField] int spawnDelay;
    [SerializeField] Transform[] spawnPoint;
    [SerializeField] GameObject zombiePrefab;

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
        wave.GetComponent<Text>().text = "Wave" + currWave.ToString();
        currWave++;
        StartCoroutine(DeactiveWave());

        StartCoroutine(Spawn());

        DropBox();
    }

    IEnumerator Spawn()
    {
        int num = spawnPoint.Length < currWave ? spawnPoint.Length : currWave - 1;
        numOfZombieInScene = numOfZombieBeSpawned * num;

        for (int j = 0;  j < numOfZombieBeSpawned; j++)
        {
            for (int i = 0; i < num; i++)
            {
                Instantiate(zombiePrefab, spawnPoint[i].position, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator DeactiveWave()
    {
        yield return new WaitForSeconds(3);
        wave.SetActive(false);
    }

    void DropBox()
    {
        Instantiate(planePrefab);
    }
}
