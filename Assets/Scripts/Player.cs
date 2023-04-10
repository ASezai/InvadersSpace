using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;

    private const float maxX = 3.6f;
    private const float minX = -3.6f;

    //private float speed = 3f;
    private bool isShooting;
    //private float cooldown = 0.5f;
    [SerializeField] private ObjectPool objectPool = null;
    public ShipStats shipStats;
    private Vector2 offScreenPos = new Vector2(0, -20f);
    private Vector2 startPos = new Vector2(0, -5f);

    public void Start()
    {
        shipStats.currentHealth = shipStats.maxHealth;
        shipStats.curretLifes = shipStats.maxLifes;
        transform.position = startPos;
        UIManager.UpdateHealthBar(shipStats.currentHealth);
        UIManager.UpdateLives(shipStats.curretLifes);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A) && transform.position.x > minX)
        {
            transform.Translate(Vector2.left * Time.deltaTime * shipStats.shipSpeed);
        }

        if (Input.GetKey(KeyCode.D) && transform.position.x < maxX)
        {
            transform.Translate(Vector2.right * Time.deltaTime * shipStats.shipSpeed);
        }
        if (Input.GetKey(KeyCode.Space) && !isShooting)
        {
            StartCoroutine(Shoot());
        }
#endif
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        //Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        GameObject obj = objectPool.GetPooledObject();
        obj.transform.position = gameObject.transform.position;
        yield return new WaitForSeconds(shipStats.fireRate);
        isShooting = false;
    }
    public void AddHealth()
    {
        if (shipStats.currentHealth == shipStats.maxHealth)
        {
            UIManager.UpdateScore(250);
        }
        else
        {
            shipStats.currentHealth++;
            UIManager.UpdateHealthBar(shipStats.currentHealth);
        }
    }
    public void AddLife()
    {
        if (shipStats.curretLifes == shipStats.maxLifes)
        {
            UIManager.UpdateScore(1000);
        }
        else
        {
            shipStats.curretLifes++;
            UIManager.UpdateHealthBar(shipStats.curretLifes);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("Hit");
            collision.gameObject.SetActive(false);
            TakeDamage();
        }
    }
    private IEnumerator Respawn()
    {
        transform.position = offScreenPos;
        yield return new WaitForSeconds(2);

        shipStats.currentHealth = shipStats.maxHealth;
        transform.position = startPos;
        UIManager.UpdateHealthBar(shipStats.currentHealth);
    }

    public void TakeDamage()
    {
        shipStats.currentHealth--;
        UIManager.UpdateHealthBar(shipStats.currentHealth);
        if (shipStats.currentHealth <= 0 )
        {
            shipStats.curretLifes--;
            UIManager.UpdateLives(shipStats.curretLifes);
            if (shipStats.curretLifes <= 0)
            {
                Debug.Log("gameover");
            }
            else
            {
                Debug.Log("Respawn");
                StartCoroutine(Respawn());
            }
        }
    }
}
