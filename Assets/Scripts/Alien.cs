using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public int scoreValue;
    public GameObject explosion;
    public GameObject coinPrefab;
    public GameObject healthPrefab;
    public GameObject lifePrefab;

    private const int LIFE_CHANCE = 1;
    private const int HEALTG_CHANCE = 10;
    private const int COÝN_CHANCE = 50;

    public void Kill()
    {
        UIManager.UpdateScore(scoreValue);
        AlienMaster.allAlies.Remove(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);

        int ran = Random.Range(0, 1000);
        if (ran <= LIFE_CHANCE)
        {
            Instantiate(lifePrefab, transform.position, Quaternion.identity);
        }
        else if (ran <= HEALTG_CHANCE)
        {
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
        }
        else if (ran <= COÝN_CHANCE)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        if (AlienMaster.allAlies.Count == 0)
        {
            GameManager.SpawnNewWave();
        }
        gameObject.SetActive(false);
    }
}
