using UnityEngine;

public class starController : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    
     private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("basket"))
        {
            gameManager.instance.IncrementScore();
            Destroy(gameObject);
        }
    }
}
