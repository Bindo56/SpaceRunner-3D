using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [SerializeField] private View view;
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject touchPanel;
    [SerializeField] GameObject infoPanel;
    public Button thrusterBtn;
  //  [SerializeField] Partic
    public bool isThrusterActive = false;
    private float nextFireTime;
    private float orbTimer = 0f;
    private float scoreTimer = 0f;

    public int soalrOrb;

    private Model model;
    public static Controller Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Time.timeScale = 0f;
        thrusterBtn.interactable = false;
        // view.part
        model = new Model(forwardSpeed: 30f, horizontalSpeed: 25f , turn : 2f);
        model.LoadHighScore();
        model.ResetHits();
        view.SetModel(model);
        //  lastPosition = view.transform.position;
        ScheduleNextShot();
        view.changeExhustColor(new Color(1f, 0.85f, 0.3f));
    }


    // Update is called once per frame
    void Update()
    {
        scoreTimer += Time.deltaTime;
        soalrOrb = model.solarOrbCollected;
        float rawInput = joystick.Horizontal;
        orbTimer += Time.deltaTime;
        float horizontalInput = 0f;
        if (rawInput > 0.3f)
            horizontalInput = 1f;
        else if (rawInput < -0.3f)
            horizontalInput = -1f;

      //  transform.Rotate(0, horizontalInput * model.turnSpeed, 0);

       // Vector3 movement = new Vector3(0, 0, model.ForwardSpeed);

         Vector3 movement = new Vector3(rawInput * model.HorizontalSpeed, 0, model.ForwardSpeed);
         view.Move(movement , horizontalInput);

        if (orbTimer >= 1f)
        {
            orbTimer -= 1f;

            if (model.solarOrbCollected > 0)
            {
                model.CollectSolarOrb(-1);
                view.UpdateSolarOrbText(model.solarOrbCollected);
            }
            else
            {
               
                 GameOver();
            }
        }
      //  model.UpdateHitTimer(Time.deltaTime);

        if (scoreTimer >= 0.5f)
        {
            int pointsToAdd = Mathf.FloorToInt(scoreTimer / 0.5f);
            scoreTimer -= pointsToAdd * 0.5f;

            model.AddScore(pointsToAdd);
            view.UpdateScoreText(model.currentScore, model.highScore);
        }
        /*  //Old 
          float horizontalInput = joystick.Horizontal; // -1 to 1
          Vector3 movement = new Vector3(horizontalInput * model.HorizontalSpeed, 0, model.ForwardSpeed);
          view.Move(movement);*/

        if (Time.time >= nextFireTime)
        {
            view.FireProjectiles(model.BulletDamage, model.BulletRange);
            ScheduleNextShot();
        }
        if (model.thruster >= 50)
        {
            thrusterBtn.interactable = true;
        }
        else
        {
            thrusterBtn.interactable = false; 
        }

    }
    public void UseThruster()
    {
        if (model.thruster >= 50)
        {
            model.CollectThruster(-50);
            view.changeExhustColor(new Color(0.35f, 0.65f, 0.85f));
            isThrusterActive = true;
           

            StartCoroutine(StopThruster(2f)); 
        }
        else
        {
            thrusterBtn.interactable = false; 
            Debug.Log("Not enough thruster energy!");
        }
    }
    private IEnumerator StopThruster(float duration)
    {
        yield return new WaitForSeconds(duration);
        view.changeExhustColor(new Color(1f, 0.85f, 0.3f));
        isThrusterActive = false;
    }

    private void ScheduleNextShot()
    {
        nextFireTime = Time.time + Random.Range(model.MinFireDelay, model.MaxFireDelay);
    }

    public void mainMenuOver()
    {
        mainMenuPanel.SetActive(false);
        infoPanel.SetActive(true);
        touchPanel.SetActive(true);
        Time.timeScale = 1f; // Resume the game
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
    public void registerHit(int hits)
    {
       // model.RegisterHit();
        model.CollectHits(hits);
        model.AddScore(10);
        view.UpdateHitsText(model.hits);
    }

    public void ResetHit()
    {
        model.ResetHits();
        view.UpdateHitsText(model.hits);
    }

    public void RestartGame()
    {
        gameOverPanel.SetActive(false); //change 
        Time.timeScale = 1f;
       
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
