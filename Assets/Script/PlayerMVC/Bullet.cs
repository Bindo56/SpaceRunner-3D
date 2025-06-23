using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 100f;
    private float damage;
    private float range;
    private Vector3 startPos;

    private void Start()
    {
      //  transform.rotation = Quaternion.Euler(90f, 0,0); // Add slight random rotation
    }

    public void SetStats(float damage, float range)
    {
        this.damage = damage;
        this.range = range;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Vector3.Distance(startPos, transform.position) >= range)
        {
            this.gameObject.SetActive(false);
            // Destroy(gameObject);
        }

    }
}
