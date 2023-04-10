using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMaster : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool = null;
    public GameObject bulletPrefab;
    public GameObject motherShipPrefab;
    private Vector3 motherShipSpawnPos = new Vector3(3.72f, 3.45f, 0);
    private float motherShipTimer = 2f;

    private Vector3 hMoveDistance = new Vector3(0.05f, 0, 0);
    private Vector3 vMoveDistance = new Vector3(0, 0.15f, 0);

    private const float MAX_LEFT = -3f;
    private const float MAX_RÝGHT = 3f;
    private const float MAX_MOVE_SPEED = 0.02f;
    private const float MOTHERSHÝP_MÝN = 15f;
    private const float MOTHERSHÝP_MAX = 60f;
    private const float START_Y = 1.7f;

    public static List<GameObject> allAlies = new List<GameObject>();

    private bool entering = true;
    private bool movingRight;
    private float moveTimer = 0.01f;
    private float moveTime = 0.005f;
    private float shootTimer = 3f;
    private const float ShootTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Alien"))
        {
            allAlies.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (entering)
        {
            transform.Translate(Vector2.down * Time.deltaTime *10);
            if (transform.position.y <= START_Y)
            {
                entering = false;
            }
        }
        else
        {
            if (moveTimer <= 0)
            {
                MoveEnemies();
            }
            if (shootTimer <= 0)
            {
                Shoot();
            }
            if (motherShipTimer < 0)
            {
                SpawnMotherShip();
            }
            moveTimer -= Time.deltaTime;
            shootTimer -= Time.deltaTime;
            motherShipTimer -= Time.deltaTime;
        }
    }
    private void MoveEnemies()
    {
        int hitMax = 0;
        if (allAlies.Count > 0)
        {
            for (int i = 0; i < allAlies.Count; i++)
            {
                if (movingRight)
                {
                    allAlies[i].transform.position += hMoveDistance;
                }
                else
                {
                    allAlies[i].transform.position -= hMoveDistance;
                }
                if (allAlies[i].transform.position.x > MAX_RÝGHT || allAlies[i].transform.position.x < MAX_LEFT)
                {
                    hitMax++;
                }
            }
            if (hitMax > 0)
            {
                for (int i = 0; i < allAlies.Count; i++)
                {
                    allAlies[i].transform.position -= vMoveDistance;
                }
                movingRight = !movingRight;
            }
            moveTimer = GetMoveSpeed();
        }
    }
    private void SpawnMotherShip()
    {
        Instantiate(motherShipPrefab, motherShipSpawnPos, Quaternion.identity);
        motherShipTimer = Random.Range(MOTHERSHÝP_MÝN, MOTHERSHÝP_MAX);
    }
    private void Shoot()
    {
        Vector2 pos = allAlies[Random.Range(0, allAlies.Count)].transform.position;

        //Instantiate(bulletPrefab, pos, Quaternion.identity);
        GameObject obj = objectPool.GetPooledObject();
        obj.transform.position = pos;

        shootTimer = ShootTime;
    }
    private float GetMoveSpeed()
    {
        float f = allAlies.Count * moveTime;

        if (f < MAX_MOVE_SPEED)
        {
            return MAX_MOVE_SPEED;
        }
        else
        {
            return f;
        }
    }
}
