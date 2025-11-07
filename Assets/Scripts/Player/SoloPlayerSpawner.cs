using UnityEngine;

public class SoloPlayerSpawner : MonoBehaviour
{
    public GameObject PlayerSpawner;
    void Start()
    {
        Instantiate(PlayerSpawner, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
