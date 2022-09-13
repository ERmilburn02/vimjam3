using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletProjectile : MonoBehaviour
{
    private Rigidbody m_RB;

    [SerializeField] private float m_Speed = 10.0f;

    private void Awake()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_RB.velocity = transform.forward * m_Speed;

        Destroy(gameObject, 10.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
