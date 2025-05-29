using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager main;

    public Transform spawnpoint;
    public Transform[] checkpoints;

    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject fast;
    [SerializeField] private GameObject tank;

    [SerializeField] private int wave = 1;
    [SerializeField] private int enemyCount = 6;
    [SerializeField] private float enemyCountRate = 0.2f;
    [SerializeField] private float spawnDelayMax = 1; 
    [SerializeField] private float spawnDelayMin = 0.75f;

    [SerializeField] private float normalRate = 0.5f;
    [SerializeField] private float fastRate = 0.4f;
    [SerializeField] private float tankRate = 0.1f;

    [SerializeField] private int maxWave = 20;

    [SerializeField] private GameObject wavePanel;
    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private PlayerInput inputActions;

    private bool waveOver = false;
    private bool waveClear = false;
    private List<GameObject> waveset = new List<GameObject>();
    private int enemyLeft;

    private int normalCount;
    private int fastCount;
    private int tankCount;

    void Awake()
    {
        main = this;
        inputActions = new PlayerInput();
    }
    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }

    void Start()
    {
        SetWave();
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (!waveOver && waveClear && enemies.Length == 0)
        {
            Player.main.ink += 50 + (10 * wave);
            waveOver = true;
            wavePanel.SetActive(true);
        }
    }

    void SetWave()
    {
        if (waveText != null)
{
    waveText.text = "Wave : " + wave + " / " + maxWave;
}
        Debug.Log("Wave : " + wave);
        normalCount = Mathf.RoundToInt(enemyCount * normalRate + fastRate + tankRate);
        fastCount = 0;
        tankCount = 0;

        if (wave % 3 == 0)
        {
            normalCount = Mathf.RoundToInt(enemyCount * normalRate);
            fastCount = Mathf.RoundToInt(enemyCount * fastRate);
        }
        if (wave % 6 == 0)
        {
            normalCount = Mathf.RoundToInt(enemyCount * normalRate);
            fastCount = Mathf.RoundToInt(enemyCount * fastRate);
            tankCount = Mathf.RoundToInt(enemyCount * tankRate);
        }


        enemyLeft = normalCount + fastCount + tankCount;
        //enemyCount = enemyLeft;

        waveset = new List<GameObject>();
        for (int i = 0; i < normalCount; i++)
        {
            waveset.Add(normal);
        }
        for (int i = 0; i < fastCount; i++)
        {
            waveset.Add(fast);
        }
        for (int i = 0; i < tankCount; i++)
        {
            waveset.Add(tank);
        }
        waveset = Shuffle(waveset);
        
        StartCoroutine(Spawn());
    }

    [SerializeField] private GameObject winGUI;
    public void NextWave()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        wavePanel.SetActive(false);

        if (wave >= maxWave)
        {
            winGUI.SetActive(true);
            return;
        }

        if (waveClear && enemies.Length == 0)
        {
            wave++;
            waveClear = false;
            waveOver = false;
            enemyCount += Mathf.RoundToInt(enemyCount * enemyCountRate);
            SetWave();
        }
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < waveset.Count; i++)
        {
            Instantiate(waveset[i], spawnpoint.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(spawnDelayMin,spawnDelayMax));
        }
        waveClear = true;
    }

    public List<GameObject> Shuffle(List<GameObject> waveset)
    {
        List<GameObject> temp = new List<GameObject>();
        List<GameObject> result = new List<GameObject>();
        temp.AddRange(waveset);

        for (int i = 0; i < waveset.Count; i++)
        {
            int index = Random.Range(0, temp.Count -1);
            result.Add(temp[index]);
            temp.RemoveAt(index);
        }
        return result;
    }
}

