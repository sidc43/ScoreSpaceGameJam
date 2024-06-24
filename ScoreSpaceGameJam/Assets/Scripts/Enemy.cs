using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string _name;
    public float speed;
    public float damage;
    public float waveSpeedMultiplier;
    public float waveDamageMultiplier;
    [SerializeField] private float hp;
    public int waveToSpawn;
    public GameObject protectile;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameManager gameManager;
    private PlayerController player;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Damage(float dmgTaken)
    {
        hp -= dmgTaken;
        if (hp <= 0)
        {
            Destroy(this.gameObject);
            gameManager.currentEnemies--;
            gameManager.waveSlider.value = gameManager.currentEnemies;
        }
    }
}