using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLight : MonoBehaviour {

    public float MoveSpeed = 3.0f;

    private float size;
    private Vector3 targetPosition;

    void Start()
    {
        size = transform.localScale.magnitude;
        targetPosition = GameManager.car.transform.position;
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * MoveSpeed);
        CheckForCollision();
    }

    /// <summary>
    /// Check if light has collided with Player and destroy self
    /// </summary>
    private void CheckForCollision() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, size);
        foreach(Collider collider in hitColliders) {
            if (collider.CompareTag("Player")) {
                GameManager.HitByLight();
            }
            else if (!collider.CompareTag("Spawner") && collider.gameObject != this.gameObject && !collider.CompareTag("Floor")) {
                Destroy(gameObject);
            }
        }
    }
}
