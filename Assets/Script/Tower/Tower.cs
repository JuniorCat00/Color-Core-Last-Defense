using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    public float range = 8f;
    public int damage = 25;
    public float fireRate = 1f;
    public int cost = 50;

    [Header("Target Mode")]
    public bool first = true;
    public bool last = false;
    public bool strong = false;
    public bool weak = false;

    [Header("Effect")]
    [SerializeField] GameObject fireEffect;
    public Sprite towerIcon;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    [NonSerialized]
    public GameObject target;
    private float cooldown = 0f;

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Start()
    {
        fireEffect.SetActive(false);
    }

    void Update()
    {
        if (target)
        {
            if (cooldown >= fireRate)
            {
                transform.right = target.transform.position - transform.position;
                target.GetComponent<Enemy>().TakeDamage(damage);
                cooldown = 0f;
                if (audioSource != null && audioClip != null)
                {
                    audioSource.PlayOneShot(audioClip);
                }

                StartCoroutine(FireEffect());
            }
            else
            {
                cooldown += 1 * Time.deltaTime;
            }
        }
    }

    IEnumerator FireEffect()
    {
        fireEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fireEffect.SetActive(false);
    }
}
