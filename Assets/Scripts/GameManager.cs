using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject hardEnemyPrefab;
    public Transform[] spawnLocations;

    private bool gameOver = false;
    public TextMeshProUGUI resultText;
    private HealthAndDamage enemyHealth;

    private void Start()
    {
        if (enemyPrefab == null || hardEnemyPrefab == null || resultText == null ||
            spawnLocations == null || spawnLocations.Length == 0)
        {
            Debug.LogError(gameObject.name + ": GameManager has a missing reference or no spawn locations");
            enabled = false;
            return;
        }

        resultText.gameObject.SetActive(false);

        Transform spawnPoint = spawnLocations[Random.Range(0, spawnLocations.Length)];
        GameObject prefab = enemyPrefab;
        if (Random.Range(0, 2) == 1)
        {
            prefab = hardEnemyPrefab;
        }

        if (spawnPoint == null)
        {
            Debug.LogError(gameObject.name + ": one of the spawn locations is empty");
            enabled = false;
            return;
        }

        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        enemyHealth = enemy.GetComponent<HealthAndDamage>();

        if (enemyHealth == null)
        {
            Debug.LogError(gameObject.name + ": the enemy prefab has no HealthAndDamage");
            enabled = false;
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }
        if (enemyHealth.IsDead())
        {
            PlayerWon();
        }
    }

    public void PlayerWon()
    {
        ShowResult("You Won");
    }

    public void PlayerLost()
    {
        ShowResult("You Lose");
    }

    private void ShowResult(string message)
    {
        if (gameOver || !enabled)
        {
            return;
        }
        gameOver = true;
        resultText.text = message;
        resultText.gameObject.SetActive(true);
    }
}
