using UnityEngine;

public class Model
{
    public float ForwardSpeed { get; private set; }
    public float HorizontalSpeed { get; private set; }

    public float turnSpeed { get; private set; } = 2f;
    public float fireSpeed { get; private set; }

    public float BulletDamage { get; private set; } = 10f;
    public float BulletRange { get; private set; } = 70f;
    public float MinFireDelay { get; private set; } = 0.3f;
    public float MaxFireDelay { get; private set; } = 0.5f;

   // public int health { get; private set; } = 100;

    public int hits { get; private set; } = 0;

    public int solarOrbCollected { get; private set; } = 50;

    public int currentScore { get; private set; } = 0;

    public int highScore { get; private set; } = 0;
   // public int energyBar { get; private set; } = 50;

    public int thruster { get; private set; } = 50;

    public int shield { get; private set; } = 0;


    public int hitMultiplier { get; private set; } = 1;
    private float hitResetTimer = 0f;
    private const float hitResetDuration = 3f; // Reset after 3 seconds without hit

    //   public int thrusterEnergy { get; private set; } = 0;

    public Model(float forwardSpeed, float horizontalSpeed , float turn)
    {
        ForwardSpeed = forwardSpeed;
        HorizontalSpeed = horizontalSpeed;
        turnSpeed = turn;
    }

    public void CollectSolarOrb(int amount)
    {
        solarOrbCollected = Mathf.Max(0, solarOrbCollected + amount);

        if (amount > 0)
            currentScore += amount;
    }

    public void CollectThruster(int amount)
    {
        thruster = Mathf.Max(0, thruster + amount);
    }

    public void CollectShield(int amount)
    {
        shield += amount;
    }

    public void CollectHits(int hits)
    {
        this.hits += hits;
       
    }
    public void AddScore(int amount)
    {
        currentScore += amount;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }
  /*  public void RegisterHit()
    {
        this.hits += hits;
        hitMultiplier++;
        hitResetTimer = hitResetDuration;
    }
    public void UpdateHitTimer(float deltaTime)
    {
        if (hitMultiplier > 1)
        {
            hitResetTimer -= deltaTime;
            if (hitResetTimer <= 0f)
            {
                ResetHits();
                hitMultiplier = 0; // Reset multiplier
            }
        }
    }*/
    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
    public void ResetHits()
    {
        hits = 0;
    }
}
