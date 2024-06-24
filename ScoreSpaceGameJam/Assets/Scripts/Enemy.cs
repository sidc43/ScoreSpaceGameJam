using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string _name;
    public float speed;
    public float damage;
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
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), speed * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void Damage(float dmgTaken)
    {
        hp -= dmgTaken;
        if (hp <= 0)
        {
            Destroy(this.gameObject);
            gameManager.currentEnemies--;
        }
    }
}