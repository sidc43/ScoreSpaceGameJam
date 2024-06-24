using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float hp;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 movement;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite right;
    [SerializeField] private Sprite straight;
    [SerializeField] private Attack attack;
    [SerializeField] private GameObject leftBooster;
    [SerializeField] private GameObject rightBooster;
    [SerializeField] private Slider healthbar;

    private float attackInterval;
    private float attackTime;
    private bool canShoot;

    void Start()
    {
        canShoot = true;
        healthbar.maxValue = hp;
        healthbar.value = hp;
    }

    void Update()
    {
        if (GameManager.LMB)
        {
            Shoot();
        }

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");


        HandleSprite();

        if (attackInterval <= 0)
        {
            attackInterval = attack.interval;
            canShoot = true;
        }
        else
        {
            attackInterval -= Time.fixedDeltaTime;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    private IEnumerator Blink(GameObject go)
    {
        SpriteRenderer goSprite = go.GetComponent<SpriteRenderer>();

        Color defaultColor = goSprite.color;

        goSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);

        goSprite.color = defaultColor;
    }

    IEnumerator Flash()
    {
        Color spriteColor = this.gameObject.GetComponent<SpriteRenderer>().color;
        for (int n = 0; n < 2; n++)
        {
            SetSpriteColor(new Color(1, 1, 1, 0), this.gameObject);
            yield return new WaitForSeconds(0.1f);
            SetSpriteColor(spriteColor, this.gameObject);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SetSpriteColor(Color col, GameObject go)
    {
        go.GetComponent<SpriteRenderer>().color = col;
    }

    private void HandleSprite()
    {
        if (movement.x < 0)
        {
            sprite.sprite = left;
        }
        else if (movement.x > 0)
        {
            sprite.sprite = right;
        }
        else
            sprite.sprite = straight;

        if (movement.y > 0)
        {
            StartCoroutine("Blink", leftBooster);
            StartCoroutine("Blink", rightBooster);
        }
        else
        {
            StartCoroutine("Blink", leftBooster);
            StartCoroutine("Blink", rightBooster);
            leftBooster.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            rightBooster.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        if (canShoot)
        {
            GameObject attackInstance = Instantiate(attack.protectile);
            attackInstance.transform.position = this.transform.position;
            float angle = Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg;
            attackInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            attackInstance.GetComponent<Rigidbody2D>().AddForce(transform.up * attack.speed * Time.deltaTime);

            attackInterval = attack.interval;
            canShoot = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (hp <= 0)
            {
                Destroy(this.gameObject);
                GameManager.GOTO("Game Over");
            }
            hp -= enemy.damage;
            healthbar.value = hp;

            StartCoroutine(Flash());
        }
    }

    
}
