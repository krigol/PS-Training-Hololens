using UnityEngine;

public class Selfdestruct : MonoBehaviour
{

    public float despawnTime = 5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
