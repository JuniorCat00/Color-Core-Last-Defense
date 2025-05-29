using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        if (currentlevel < level.Length)
        {
            int upgradeCost = level[currentlevel].cost;

            if (Player.main.ink >= upgradeCost)
            {
                Debug.Log("Upgrading tower to level: " + (currentlevel + 1));
                tower.range = level[currentlevel].range;
                tower.damage = level[currentlevel].damage;
                tower.fireRate = level[currentlevel].fireRate;
                towerRange.UpdateRange();

                Player.main.ink -= level[currentlevel].cost;

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
}
