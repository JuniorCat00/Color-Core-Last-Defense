using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    [System.Serializable]
    class Level
    {
        public float range = 8f;
        public int damage = 25;
        public float fireRate = 1f;
        public int cost = 100;
    }

    [SerializeField] private Level[] level = new Level[3];
    [NonSerialized] public int currentlevel = 0;
    [NonSerialized] public string currentCost;

    private Tower tower;
    [SerializeField] private TowerRange towerRange;

    void Awake()
    {
        tower = GetComponent<Tower>();
        currentCost = level[0].cost.ToString();
    }

    public void Upgrade()
    {
        // ป้องกันออกนอกช่วง
        if (currentlevel >= level.Length)
            return;

        int upgradeCost = level[currentlevel].cost;

        // ★ ใช้ InkManager แทน Player
        if (InkManager.main.ink >= upgradeCost)
        {
            Debug.Log("Upgrading tower to level: " + (currentlevel + 1));

            tower.range = level[currentlevel].range;
            tower.damage = level[currentlevel].damage;
            tower.fireRate = level[currentlevel].fireRate;
            towerRange.UpdateRange();

            // ★ ใช้ InkManager แทน Player
            InkManager.main.ink -= upgradeCost;

            currentlevel++;

            if (currentlevel >= level.Length)
            {
                currentCost = "Max";
            }
            else
            {
                currentCost = level[currentlevel].cost.ToString();
            }
        }
    }
}
