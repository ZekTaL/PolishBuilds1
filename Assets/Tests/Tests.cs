using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Tests
{
    private Game game;

    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        game = gameGameObject.GetComponent<Game>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(game.gameObject);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator AsteroidMoveDown()
    {
        Spawner spawner = game.GetSpawner();
        GameObject asteroid = spawner.SpawnAsteroid();

        float initialPos = asteroid.transform.position.y;

        yield return new WaitForSeconds(0.5f);

        Assert.Less(asteroid.transform.position.y, initialPos);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator GameOverOccursOnAsteroidCollision()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        GameObject ship = game.GetShip();

        asteroid.transform.position = ship.transform.position;

        yield return new WaitForSeconds(0.1f);

        Assert.True(game.isGameOver);
    }

    [UnityTest]
    public IEnumerator NewGameRestartsGame()
    {
        game.isGameOver = true;
        game.StartNewGame();

        Assert.False(game.isGameOver);
        yield return null;
    }

    [UnityTest]
    public IEnumerator LaserMovesUp()
    {
        GameObject shipGameObject = game.GetShip();
        Ship ship = shipGameObject.GetComponent<Ship>();
        GameObject laser = ship.SpawnLaser();

        float initialYPos = laser.transform.position.y;
        yield return new WaitForSeconds(0.2f);

        Assert.Greater(laser.transform.position.y, initialYPos);
    }

    [UnityTest]
    public IEnumerator LaserDestroysAsteroids()
    {
        GameObject shipGO = game.GetShip();
        Ship ship = shipGO.GetComponent<Ship>();
        GameObject laser = ship.SpawnLaser();

        Spawner spawner = game.GetSpawner();
        GameObject asteroid = spawner.SpawnAsteroid();

        laser.transform.position = asteroid.transform.position;

        yield return new WaitForSeconds(0.2f);

        // Assert.IsNull works differently from the next line
        UnityEngine.Assertions.Assert.IsNull(asteroid);
    }


    [UnityTest]
    public IEnumerator ShipCanMoveHorizontally()
    {
        GameObject shipGameObject = game.GetShip();
        Ship ship = shipGameObject.GetComponent<Ship>();

        // check if moves right
        float initialXPos = ship.transform.position.x;
        ship.MoveRight();

        yield return new WaitForSeconds(0.2f);

        Assert.Greater(ship.transform.position.x, initialXPos);

        // check if moves left
        initialXPos = ship.transform.position.x;
        ship.MoveLeft();

        yield return new WaitForSeconds(0.2f);

        Assert.Less(ship.transform.position.x, initialXPos);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ScoreIsZeroAtStart()
    {
        Assert.Zero(game.score);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ScoreIncreasesOnAsteroidDestroyed()
    {
        int initialScore = game.score;

        // I destroy the asteroid first
        GameObject shipGO = game.GetShip();
        Ship ship = shipGO.GetComponent<Ship>();
        GameObject laser = ship.SpawnLaser();

        Spawner spawner = game.GetSpawner();
        GameObject asteroid = spawner.SpawnAsteroid();

        laser.transform.position = asteroid.transform.position;

        yield return new WaitForSeconds(0.2f);

        // check the new score
        int newScore = game.score;

        Assert.Less(initialScore, newScore);

        yield return null;
    }
}

