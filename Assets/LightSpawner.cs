using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpawner : MonoBehaviour
{
    public GameObject SmallLightPrefab;
    public float SpawnRate = 0.3f;

    private float timer = 0f;
    
    void Update()
    {
        CheckForSpawn();
    }

    /// <summary>
    /// Updates timer and checks whether a light needs to be instantiated
    /// </summary>
    private void CheckForSpawn() {
        timer += Time.deltaTime;
        if (timer > 1 / SpawnRate) {
            Instantiate(SmallLightPrefab, transform.position + Vector3.up, Quaternion.identity);
            timer = 0f;
        }
    }
}
