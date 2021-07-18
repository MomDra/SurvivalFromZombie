using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int currWave = 1;
    int numOfZombieBeSpawned = 10;
    public int numOfZombieInScene { get; set; }

    [SerializeField] int spawnDelay;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject zombiePrefab;

    [SerializeField] GameObject pressIToStart;
    [SerializeField] GameObject wave;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isStart)
        {
            WaveStart();
        }
        
        if(numOfZombieInScene == 0)
        {
            isStart = false;
            pressIToStart.SetActive(true);
        }
    }

    void WaveStart()
    {
        isStart = true;
        numOfZombieInScene = numOfZombieBeSpawned;

        pressIToStart.SetActive(false);
        wave.SetActive(true);
        wave.GetComponent<Text>().text = "Wave" + currWave.ToString();
        currWave++;
        StartCoroutine(DeactiveWave());

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for(int i = 0; i< numOfZombieBeSpawned; i++)
        {
            Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator DeactiveWave()
    {
        yield return new WaitForSeconds(3);
        wave.SetActive(false);
    }
}
