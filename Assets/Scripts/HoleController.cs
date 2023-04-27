using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    [SerializeField] private Transform HolePosition;
    [SerializeField] private LayerMask bodyLayer;
    [SerializeField] private ParticleSystem particle;

    public static HoleController Instance;
    private void Awake() => Instance = this;
    private void Update()
    {
        CheckCollision();
    }
    void CheckCollision()
    {
        Collider[] bodies = Physics.OverlapSphere(HolePosition.position, 2, bodyLayer);
        foreach(Collider body in bodies) {
            var enemy = body.GetComponentInParent<EnemyController>();
            if (enemy != null) {
                enemy.GetPoints();
                particle.Play();
                break;
            }
        }
    }
}


