using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    [SerializeField] private Tower tower;
    private List<GameObject> targets = new List<GameObject>();

    void Start()
    {
        UpdateRange();
    }

    void Update()
    {
        if (targets.Count > 0)
        {
            if (tower.first)
            {
                float minDistance = Mathf.Infinity;
                int maxIndex = 0;
                GameObject firstTarget = null;

                foreach (GameObject target in targets)
                {
                    int index = target.GetComponent<Enemy>().index;
                    float distance = target.GetComponent<Enemy>().distance;

                    if (index > maxIndex || (index == maxIndex && distance < minDistance))
                    {
                        maxIndex = index;
                        minDistance = distance;
                        firstTarget = target;
                    }
                }
                tower.target = firstTarget;
            }
            else if (tower.last)
            {
                float maxDistance = -Mathf.Infinity;
                int minIndex = int.MaxValue;
                GameObject lastTarget = null;

                foreach (GameObject target in targets)
                {
                    int index = target.GetComponent<Enemy>().index;
                    float distance = target.GetComponent<Enemy>().distance;

                    if (index < minIndex || (index == minIndex && distance > maxDistance))
                    {
                        minIndex = index;
                        maxDistance = distance;
                        lastTarget = target;
                    }
                }
                tower.target = lastTarget;
            }
            else if (tower.strong)
            {
                GameObject strongestTarget = null;
                float maxHP = 0;

                foreach (GameObject target in targets)
                {
                    float hp = target.GetComponent<Enemy>().hp;

                    if (hp > maxHP)
                    {
                        maxHP = hp;
                        strongestTarget = target;
                    }
                }
                tower.target = strongestTarget;
            }
            else if (tower.weak)
            {
                GameObject weakestTarget = null;
                float minHP = 100;

                foreach (GameObject target in targets)
                {
                    float hp = target.GetComponent<Enemy>().hp;

                    if (hp < minHP)
                    {
                        minHP = hp;
                        weakestTarget = target;
                    }
                }
                tower.target = weakestTarget;
            }
            else
            {
                tower.target = targets[0];
            }
        }
        else
        {
            tower.target = null;
        }
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            targets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            targets.Remove(collision.gameObject);
        }
    }

    public void UpdateRange()
    {
        transform.localScale = new Vector3(tower.range, tower.range, tower.range);
    }
}
