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

    private float originalSpeed;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        originalSpeed = moveSpeed;
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
            // ★ แจ้ง Event ให้ InkManager จัดการเงิน
            EventManager.EnemyKilled(value);

            if (audioClip != null)
            {
                GameObject soundGO = new GameObject("DeathSound");
                AudioSource src = soundGO.AddComponent<AudioSource>();
                src.clip = audioClip;
                src.Play();
                Destroy(soundGO, audioClip.length);
            }

            Destroy(gameObject);
        }
    }
    // ✦ เพิ่มสำหรับระบบสกิล (Command Pattern)
    public void ApplySlow(float multiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowCoroutine(multiplier, duration));
    }

    private IEnumerator SlowCoroutine(float multiplier, float duration)
    {
        moveSpeed = originalSpeed / multiplier; // ลดความเร็ว 1.5f
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;             // คืนค่าเดิมเมื่อหมดเวลา
    }
}
