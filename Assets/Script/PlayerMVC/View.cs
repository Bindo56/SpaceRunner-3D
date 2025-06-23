using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static PoolEnum;

public class View : MonoBehaviour
{

    public Transform[] gunPoints;
    [SerializeField] ParticleSystem[] exhust;
    [SerializeField] TextMeshProUGUI solarOrb;
    [SerializeField] TextMeshProUGUI thursterOrb;
    [SerializeField] TextMeshProUGUI sheildOrb;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI hitText;

    public GameObject bulletPrefab;
    public PoolTag bulletTag = PoolTag.Bullet;
    public Model model;
    [SerializeField] Controller controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

  
    Vector3 sheildTextScale;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        // model = new Model(forwardSpeed: 30f, horizontalSpeed: 25f, turn: 2f);
    }
    void Start()
    {
        solarOrb.text = "Solar Orb: " + model.solarOrbCollected;
        thursterOrb.text = "Thruster Orb: " + model.thruster;
        sheildOrb.text = "Shield Orb: " + model.shield;
        scoreText.text = "Score: " + model.currentScore;
        highScoreText.text = "High Score: " + model.highScore;
        //soalrTextScale = solarOrb.rectTransform.localScale;
      //  thurusterTextScale = thursterOrb.rectTransform.localScale;
        sheildTextScale = sheildOrb.rectTransform.localScale;
    }
    public void SetModel(Model m)
    {
        model = m;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private float currentZRotation = 0f;
    private float zVelocity = 0f;
    public void Move(Vector3 movement, float horizontalInput)
    {
        float speedMultiplier = controller.isThrusterActive ? 3f : 1f;
        transform.Translate(movement * Time.deltaTime * speedMultiplier, Space.World);
        if (controller.isThrusterActive)
        {
            thursterOrb.text = "Thruster: " + model.thruster;
            // transform.Translate(movement * Time.deltaTime, Space.World);  

        }
            float targetZ = horizontalInput * 20f;
        currentZRotation = Mathf.SmoothDampAngle(currentZRotation, targetZ, ref zVelocity, 0.1f);
        transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
        // transform.rotation = Quaternion.Euler(0, 0, horizontalInput * 30); // Rotate based on horizontal input
    }

    public void FireProjectiles(float damage, float range)
    {
        foreach (Transform gun in gunPoints)
        {
            GameObject bullet = PoolManager.Instance.SpawnFromPool(bulletTag, gun.position, gun.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetStats(damage, range);
        }


        /* foreach (Transform gun in gunPoints)
         {
             GameObject bullet = GameObject.Instantiate(bulletPrefab, gun.position,Quaternion.identity);
             Bullet bulletScript = bullet.GetComponent<Bullet>();
             bulletScript.SetStats(damage, range);
         }*/
    }

    private void OnTriggerEnter(Collider other)
    {
       
        var solarOrbComp = other.GetComponent<SolarOrb>();
        if (solarOrbComp != null)
        {
            if (model.solarOrbCollected < 100)
            {
                model.CollectSolarOrb(10);
                solarOrb.text = "Solar Orb: " + model.solarOrbCollected;
                StartCoroutine(PopText(solarOrb));
            }
            other.gameObject.SetActive(false);
            return;
        }

        
        var thrusterComp = other.GetComponent<ThrusterOrb>();
        if (thrusterComp != null)
        {
            model.CollectThruster(10);
            thursterOrb.text = "Thruster Orb: " + model.thruster;
            StartCoroutine(PopText(thursterOrb));
            other.gameObject.SetActive(false);
            return;
        }

       
        var shieldOrbComp = other.GetComponent<SheildOrb>();
        if (shieldOrbComp != null)
        {
            model.CollectShield(10);
            sheildOrb.text = "Shield Orb: " + model.shield;
            StartCoroutine(PopText(sheildOrb));
            other.gameObject.SetActive(false);
            return;
        }

        
        if (other.GetComponent<DestrutableMetriod>() != null || other.GetComponent<Non_destrutableMetriod>() != null)
        {
            if (model.shield >= 10)
            {
                model.CollectShield(-10);
                sheildOrb.text = "Shield Orb: " + model.shield;
                controller.ResetHit();
                StartCoroutine(PopText(sheildOrb));
                other.gameObject.SetActive(false);
            }
            else
            {
                GameOver();
            }
            return;
        }
    }
   
    public void changeExhustColor(Color color)
    {
        foreach (ParticleSystem exhust in exhust)
        {
            var renderer = exhust.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.trailMaterial.color = color;
            }
        }
    }

    public void UpdateHitsText(int hit)
    {
        hitText.text = "Hit x" + hit;
        StartCoroutine(PopText(hitText));
    }

    public void GameOver()
    {
        controller.GameOver();
    }
    public void UpdateScoreText(int currentScore, int highScore)
    {
        scoreText.text = "Score: " + currentScore;
        highScoreText.text = "High Score: " + highScore;
    }

    public void UpdateSolarOrbText(int solarOrbCollected)
    {
        solarOrb.text = "Solar Orb: " + model.solarOrbCollected;
    }
   
    private IEnumerator PopText(TextMeshProUGUI text)
    {
       // Vector3 originalScale = text.rectTransform.localScale;
        Vector3 popScale = sheildTextScale * 1.5f;

        float duration = 0.15f;
        float t = 0f;

       
        while (t < duration)
        {
            text.rectTransform.localScale = Vector3.Lerp(sheildTextScale, popScale, t / duration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

      
        t = 0f;
        while (t < duration)
        {
            text.rectTransform.localScale = Vector3.Lerp(popScale, sheildTextScale, t / duration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        text.rectTransform.localScale = sheildTextScale;
    }
}
