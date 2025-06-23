using System.Collections;
using UnityEngine;


public class DestrutableMetriod : MonoBehaviour
{
    private bool isHit = false;
   [SerializeField] ParticleSystem blast;
    private Controller controller;

    // [SerializeField] private GameObject blastPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetController(Controller ctrl)
    {
        controller = ctrl;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHit) return;


        if (other.GetComponent<Bullet>() != null)
        {
            isHit = true;
            StartCoroutine(Blast());
            
            //gameObject.SetActive(false);
        }
/*
        View view = other.GetComponent<View>();

        if (view != null)
        {
            view.GameOver();
        }*/

    }

    IEnumerator Blast()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col != null)
        {
            col.enabled = false;
        }
        controller.registerHit(1);
        blast.Play();
        //transform.GetComponent<BoxCollider>
        yield return new WaitForSeconds(0.35f);
        transform.gameObject.SetActive(false);
     
       // Destroy(blast); 
        isHit = false;

    }


}
