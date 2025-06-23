using UnityEngine;

public class SpawnView : MonoBehaviour
{
    //not needed 
    private void Update()
    {
       
        transform.Translate(Vector3.back * 20f * Time.deltaTime, Space.World);
    }
}
