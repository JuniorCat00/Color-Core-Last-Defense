using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 50;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int value = 10;

    private Rigidbody2D rb2d;
    private Transform checkpoint;

    [NonSerialized] public int index = 0;
    [NonSerialized] public float distance = 0;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Start()
    {
        checkpoint = EnemyManager.main.checkpoints[index];
    }

    void Update()
    {
        checkpoint = EnemyManager.main.checkpoints[index];
        distance = Vector2.Distance(transform.position, EnemyManager.main.checkpoints[index].position);


        if (Vector2.Distance(checkpoint.transform.position, transform.position) <= 0.1f)
        {
            index++;
            //Debug.Log(gameObject.name + "Reaching to Checkpoint : " + index);
            if (index >= EnemyManager.main.checkpoints.Length)
            {
                Player.main.TakeDamage(hp);
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    { 
        Vector2 direction = (checkpoint.position - transform.position).normalized;
        transform.right = checkpoint.position - transform.position;
        rb2d.velocity = direction * moveSpeed;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Player.main.ink += value;

            if (audioClip != null)
            {
                GameObject soundGO = new GameObject("DeathSound");
                AudioSource src = soundGO.AddComponent<AudioSource>();
                src.clip = audioClip;
                src.Play();
                Destroy(soundGO, audioClip.length); // ลบทิ้งหลังเสียงจบ
            }

            Destroy(gameObject);
        }
    }
}
