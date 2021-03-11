using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public int score = 0;
    public bool isGameOver = false;

    [SerializeField]
    private GameObject shipModel;
    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private GameObject backToMainMenuButton;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Spawner spawner;

    private static Game instance;

    private void Start()
    {
        instance = this;

        StartNewGame();
    }

    public static void GameOver()
    {
        instance.isGameOver = true;
        instance.spawner.StopSpawning();
        instance.shipModel.GetComponent<Ship>().Explode();
        instance.gameOverText.SetActive(true);
        instance.backToMainMenuButton.SetActive(true);
    }

    public void StartNewGame()
    {
        isGameOver = false;
        gameOverText.SetActive(false);
        backToMainMenuButton.SetActive(false);
        score = 0;
        scoreText.text = "Score: " + score;
        scoreText.enabled = true;

        shipModel.transform.position = new Vector3(0, -3.2f, 0);
        shipModel.transform.eulerAngles = new Vector3(90, 180, 0);

        spawner.BeginSpawning();
        shipModel.GetComponent<Ship>().RepairShip();
        spawner.ClearAsteroids();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void AsteroidDestroyed()
    {
        instance.score++;
        instance.scoreText.text = "Score: " + instance.score;
    }

    public GameObject GetShip()
    {
        return shipModel.gameObject;
    }

    public Spawner GetSpawner()
    {
        return spawner.GetComponent<Spawner>();
    }
}
