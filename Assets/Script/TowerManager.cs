using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    [Header("Tower")]
    [SerializeField] private GameObject BrushTower;
    [SerializeField] private GameObject PencilTower;
    [SerializeField] private GameObject PallateTower;

    [SerializeField] private LayerMask towerLayer;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject sellPanel;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI towerLevel;
    [SerializeField] private TextMeshProUGUI towerCost;
    [SerializeField] private TextMeshProUGUI towerTarget;

    [SerializeField] private TextMeshProUGUI towerDamage;
    [SerializeField] private TextMeshProUGUI towerRange;
    [SerializeField] private TextMeshProUGUI towerFireRate;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    [SerializeField] private Image towerIconImage;

    private GameObject selectedTower;
    private GameObject placingTower;

    private void Awake()
    {
        InputSystem.EnableDevice(UnityEngine.InputSystem.Touchscreen.current);
    }

    void Update()
    {
        if (InputReader.CancelThisFrame)
        {
            ClearSelection();
        }

        if (placingTower)
        {
            if (!placingTower.GetComponent<TowerPlacement>().isPlacing)
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
                if (selectedTower)
                {
                    GameObject range1 = selectedTower.transform.GetChild(2).gameObject;
                    range1.GetComponent<SpriteRenderer>().enabled = false;
                }

                selectedTower = hit.collider.gameObject;
                GameObject range2 = selectedTower.transform.GetChild(2).gameObject;
                range2.GetComponent<SpriteRenderer>().enabled = true;

                panel.SetActive(true);
                sellPanel.SetActive(true);
                towerName.text = selectedTower.name.Replace("(Clone)", "").Trim();
                towerLevel.text = "Tower LVL : " + selectedTower.GetComponent<TowerUpgrade>().currentlevel.ToString();
                towerCost.text = selectedTower.GetComponent<TowerUpgrade>().currentCost;

                Tower tower = selectedTower.GetComponent<Tower>();

                if (tower.first)
                    towerTarget.text = "First";
                else if (tower.last)
                    towerTarget.text = "Last";
                else if (tower.strong)
                    towerTarget.text = "Strongest";
                else if (tower.weak)
                    towerTarget.text = "Weakest";

                if (towerIconImage != null && tower.towerIcon != null)
                {
                    towerIconImage.sprite = tower.towerIcon;
                    towerIconImage.enabled = true;
                }
                else
                {
                    towerIconImage.enabled = false;
                }

                if (hit.collider != null)
                {
                    if (audioSource != null && audioClip != null)
                    {
                        audioSource.PlayOneShot(audioClip);
                    }
                    selectedTower = hit.collider.gameObject;

                    selectedTower.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
                    panel.SetActive(true);

                    UpdateTowerStatsUI();
                }
            }
           /* else if (selectedTower)
            {
                if (audioSource != null && audioClip != null)
                {
                    audioSource.PlayOneShot(audioClip);
                }
                sellPanel.SetActive(false);
                panel.SetActive(false);
                GameObject range1 = selectedTower.transform.GetChild(2).gameObject;
                range1.GetComponent<SpriteRenderer>().enabled = false;
                selectedTower = null;
            }*/
        }
    }

    public void ClosePanel()
    {
        if (selectedTower)
        {
            selectedTower.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            selectedTower = null;
        }

        panel.SetActive(false);
        sellPanel.SetActive(false);

        if (audioSource && audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void SellSelectedTower()
    {
        if (selectedTower)
        {
            Player.main.ink += selectedTower.GetComponent<Tower>().cost / 2;
            Destroy(selectedTower);
            sellPanel.SetActive(false);
            panel.SetActive(false);
            selectedTower = null;
            if (audioSource != null && audioClip != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }

    public void ClearSelection()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        if (placingTower)
        {
            Destroy(placingTower);
            placingTower = null;
        }
    }

    public void SetTower(GameObject tower)
    {
        ClearSelection();
        placingTower = Instantiate(tower);
    }

    public void UpgradeSelected()
    {
        if (selectedTower)
        {
            selectedTower.GetComponent<TowerUpgrade>().Upgrade();
            UpdateTowerStatsUI();
            if (audioSource != null && audioClip != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }

    private void UpdateTowerStatsUI()
    {
        if (selectedTower == null) return;

        Tower tower = selectedTower.GetComponent<Tower>();

        towerLevel.text = "Tower LVL : " + selectedTower.GetComponent<TowerUpgrade>().currentlevel.ToString();
        towerCost.text = selectedTower.GetComponent<TowerUpgrade>().currentCost;

        towerDamage.text = "Damage : " + tower.damage.ToString("");
        towerRange.text = "Range : " + tower.range.ToString("");
        towerFireRate.text = "Fire Rate : " + tower.fireRate.ToString("");

        if (towerIconImage != null && tower.towerIcon != null)
        {
            towerIconImage.sprite = tower.towerIcon;
            towerIconImage.enabled = true;
        }
        else if (towerIconImage != null)
        {
            towerIconImage.enabled = false;
        }

        if (tower.first) towerTarget.text = "First";
        else if (tower.last) towerTarget.text = "Last";
        else if (tower.strong) towerTarget.text = "Strongest";
        else if (tower.weak) towerTarget.text = "Weakest";
    }

    public void ChangeTarget()
    {
        if (selectedTower)
        {
            if (audioSource != null && audioClip != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
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
                towerTarget.text = "Strong";
            }
            else if (tower.strong)
            {
                tower.first = false;
                tower.last = false;
                tower.strong = false;
                tower.weak = true;
                towerTarget.text = "Weak";
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

}
