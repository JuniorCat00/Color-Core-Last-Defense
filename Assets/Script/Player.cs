using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player main;

    [SerializeField] private int hp = 100;
    public int ink = 500;

    [SerializeField] private TextMeshProUGUI HpGUI;
    [SerializeField] private TextMeshProUGUI InkGUI;

    [SerializeField] private GameObject gameOverGUI;

    void Awake()
    {
        main = this;
    }
     
    void Update()
    {
        HpGUI.text = "Hp : " + hp.ToString();
        InkGUI.text = "Ink : " + ink.ToString();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            gameOverGUI.SetActive(true);
        }
    }

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
