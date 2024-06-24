using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float attackInterval;
    private float attackTime;
    private bool canShoot;

    void Start()
    {
        canShoot = true;
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
    }

    private IEnumerator Blink(GameObject go)
    {
        SpriteRenderer goSprite = go.GetComponent<SpriteRenderer>();

        Color defaultColor = goSprite.color;

        goSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);

        goSprite.color = defaultColor;
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
            attackInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * attack.speed * Time.deltaTime);

            attackInterval = attack.interval;
            canShoot = false;
        }
    }
}
