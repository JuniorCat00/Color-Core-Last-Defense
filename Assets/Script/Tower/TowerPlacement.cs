using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rangeSprite;
    [SerializeField] private CircleCollider2D rangeCollider;
    [SerializeField] private Color gray;
    [SerializeField] private Color red;

    [NonSerialized] public bool isPlacing = true;
    private bool isRestricted = false;

    private Tower tower;

    private float lastTapTime = 0f;
    private float doubleTapThreshold = 0.3f;

    void Awake()
    {
        tower = GetComponent<Tower>();
        rangeCollider.enabled = false;
    }

    void Update()
    {
        if (isPlacing)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(InputReader.PointerPosition);
            transform.position = pos;

            if (InputReader.TapThisFrame)
            {
                float currentTime = Time.time;
                if (currentTime - lastTapTime < doubleTapThreshold && !isRestricted && tower.cost <= Player.main.ink)
                {
                    ConfirmPlacement();
                }
                lastTapTime = currentTime;
            }
        }

        rangeSprite.color = isRestricted ? red : gray;

    }

    public void ConfirmPlacement()
    {
        if (!isRestricted && tower.cost <= Player.main.ink)
        {
            rangeCollider.enabled = true;
            isPlacing = false;
            rangeSprite.enabled = false;
            Player.main.ink -= tower.cost;
            GetComponent<TowerPlacement>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Restricted" || collision.gameObject.tag == "Tower" && isPlacing)
        {
            isRestricted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Restricted" || collision.gameObject.tag == "Tower" && isPlacing)
        {
            isRestricted = false;
        }
    }

}
