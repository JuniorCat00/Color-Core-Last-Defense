using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    [Header("Tower Prefabs")]
    [SerializeField] private GameObject BrushTower;
    [SerializeField] private GameObject PencilTower;
    [SerializeField] private GameObject PallateTower;

    [Header("Layer")]
    [SerializeField] private LayerMask towerLayer;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject sellPanel;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI towerLevel;
    [SerializeField] private TextMeshProUGUI towerCost;
    [SerializeField] private TextMeshProUGUI towerTarget;
    [SerializeField] private TextMeshProUGUI towerDamage;
    [SerializeField] private TextMeshProUGUI towerRange;
    [SerializeField] private TextMeshProUGUI towerFireRate;
    [SerializeField] private Image towerIconImage;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private GameObject selectedTower = null;
    private GameObject placingTower = null;

    private void Awake()
    {
        if (Touchscreen.current != null)
            InputSystem.EnableDevice(Touchscreen.current);
    }

    void Update()
    {
        if (InputReader.CancelThisFrame)
        {
            ClearPlacementOnly();
        }

        if (placingTower != null)
        {
            var tp = placingTower.GetComponent<TowerPlacement>();

            if (!tp.isPlacing)
            {
                placingTower = null;
            }
        }

        if (InputReader.TapThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(InputReader.PointerPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 100f, towerLayer);

            if (hit.collider != null)
            {
                SelectTower(hit.collider.gameObject);
            }
            else
            {
                DeselectTower();
            }
        }
    }
    private void SelectTower(GameObject towerObj)
    {
        if (selectedTower != null)
        {
            selectedTower.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
        }

        selectedTower = towerObj;

        selectedTower.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;

        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);

        panel.SetActive(true);
        sellPanel.SetActive(true);

        TowerUpgrade upgrade = selectedTower.GetComponent<TowerUpgrade>();

        towerName.text = selectedTower.name.Replace("(Clone)", "").Trim();
        towerLevel.text = "Tower LVL : " + upgrade.currentlevel;
        towerCost.text = upgrade.currentCost;

        UpdateTowerStatsUI();
    }

    private void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
        }

        selectedTower = null;

        panel.SetActive(false);
        sellPanel.SetActive(false);
    }

    public void ClearPlacementOnly()
    {
        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);

        if (placingTower != null)
        {
            var tp = placingTower.GetComponent<TowerPlacement>();

            if (tp != null && tp.isPlacing)
            {
                Destroy(placingTower);
            }
            placingTower = null;
        }
    }

    public void SetTower(GameObject tower)
    {
        ClearPlacementOnly();

        placingTower = Instantiate(tower);
    }

    public void UpgradeSelected()
    {
        if (selectedTower == null) return;

        var upgrade = selectedTower.GetComponent<TowerUpgrade>();
        if (upgrade == null) return;

        upgrade.Upgrade();
        UpdateTowerStatsUI();

        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);
    }


    private void UpdateTowerStatsUI()
    {
        if (selectedTower == null) return;

        Tower tower = selectedTower.GetComponent<Tower>();
        TowerUpgrade upgrade = selectedTower.GetComponent<TowerUpgrade>();

        towerLevel.text = "Tower LVL : " + upgrade.currentlevel;
        towerCost.text = upgrade.currentCost;

        towerDamage.text = "Damage : " + tower.damage;
        towerRange.text = "Range : " + tower.range;
        towerFireRate.text = "Fire Rate : " + tower.fireRate;

        if (towerIconImage != null && tower.towerIcon != null)
        {
            towerIconImage.sprite = tower.towerIcon;
            towerIconImage.enabled = true;
        }
        else if (towerIconImage)
        {
            towerIconImage.enabled = false;
        }

        if (tower.first) towerTarget.text = "First";
        else if (tower.last) towerTarget.text = "Last";
        else if (tower.strong) towerTarget.text = "Strongest";
        else if (tower.weak) towerTarget.text = "Weakest";
    }

    public void SellSelectedTower()
    {
        if (selectedTower == null) return;

        InkManager.main.ink += selectedTower.GetComponent<Tower>().cost / 2;

        Destroy(selectedTower);
        selectedTower = null;

        panel.SetActive(false);
        sellPanel.SetActive(false);

        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);
    }

    public void ChangeTarget()
    {
        if (selectedTower == null) return;

        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);

        Tower tower = selectedTower.GetComponent<Tower>();

        if (tower.first)
        {
            tower.first = false;
            tower.last = true;
            tower.strong = false;
            tower.weak = false;
            towerTarget.text = "Last";
        }
        else if (tower.last)
        {
            tower.first = false;
            tower.last = false;
            tower.strong = true;
            tower.weak = false;
            towerTarget.text = "Strongest";
        }
        else if (tower.strong)
        {
            tower.first = false;
            tower.last = false;
            tower.strong = false;
            tower.weak = true;
            towerTarget.text = "Weakest";
        }
        else if (tower.weak)
        {
            tower.first = true;
            tower.last = false;
            tower.strong = false;
            tower.weak = false;
            towerTarget.text = "First";
        }
    }
}
