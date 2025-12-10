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
    [SerializeField] private GameObject gameOverGUI;

    public int CurrentHP => hp;

    void Awake()
    {
        main = this;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        EventManager.PlayerHpChanged(hp);

        if (hp <= 0)
        {
            EventManager.PlayerDead();
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
