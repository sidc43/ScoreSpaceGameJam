using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public float waveProgress;
    public int[] waveCapacity;
    public GameObject enemyHolder;
    public int currentEnemies;
    public Slider waveSlider;

    public bool attemptLogin;

    public bool spawnEnemiesDev;

    [SerializeField] private PlayerController player;
    [SerializeField] Enemy[] enemies;
    [SerializeField] private TextMeshProUGUI wavetext;

    public int wave
    {
        get { return _wave; }
        set
        {
            _wave = value;
            wavetext.text = $"Wave: {_wave - 1}";
        }
    }
    private int _wave;

    void Start()
    {
        wave = 1;
        waveProgress = 0;
        waveSlider.maxValue = waveCapacity[wave - 1];
        waveSlider.value = waveCapacity[wave - 1];
    }

    void Update()
    {
        if (spawnEnemiesDev)
        {


            if (currentEnemies <= 0)
            {
                if (wave != 1)
                {
                    StartCoroutine(Sleep(2));
                    waveSlider.maxValue = waveCapacity[wave - 1];
                    waveSlider.value = waveCapacity[wave - 1];
                }
                GenerateWave(waveCapacity[wave - 1]);
                wave++;
            }
        }
    }

    private IEnumerator Sleep(int sec)
    {
        yield return new WaitForSeconds(sec);
    }

    private void GenerateWave(int cap)
    {
        for (int i = 0; i < cap; i++)
        {
            foreach (Enemy enemy in enemies) 
            {
                GameObject enemyInstance = Instantiate(enemy.gameObject);
                enemyInstance.transform.SetParent(enemyHolder.transform);

                float spawnY = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
                float spawnX = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

                enemyInstance.transform.position = new Vector2(spawnX, spawnY);
                currentEnemies++;
            }
        }
    }
}