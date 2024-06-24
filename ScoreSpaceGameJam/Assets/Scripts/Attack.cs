using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject protectile;
    public float damage;
    public float speed;
    public float interval;
    public float time;
    private float tempCounter;

    void Start()
    {
        tempCounter = 0;
    }

    private void Update()
    {
        tempCounter += Time.fixedDeltaTime;
        if (tempCounter >= time)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().Damage(damage);
            Destroy(this.gameObject);
        }
    }
}
