using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private Spawner spawner;

    private void OnEnable()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    void Update ()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 5);
        if (transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Asteroid>() != null)
        {
            Game.AsteroidDestroyed();
            Destroy(gameObject);
            spawner.asteroids.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
