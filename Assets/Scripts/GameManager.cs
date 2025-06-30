using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Blade blade;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Text scoreText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Text finalText;

    [SerializeField] private Text livesText;
    [SerializeField] private Text timerText;

    [SerializeField] private int time;
    private float timer = 0f;

    private int lives = 3;



    [Header("paneles de inicio")]
    public GameObject startPanel; // Panel de inicio
    public Button startButton; //boton de inicio

    public Text panelInicioMensaje; // para UI normal

    public int score { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Time.timeScale = 0f; // Pausa el juego al arrancar
        startPanel.SetActive(true); // Muestra el panel de inicio
        finalText.gameObject.SetActive(false);

        if (blade != null) // Añadir una comprobación de null por seguridad
        {
            blade.enabled = false;
        }

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        //UpdateLives(); 


    }

    public void StartGame()
    {
        startPanel.SetActive(false); //Oculta el panel de inicio
        NewGame(); //Reinicia el juego (activa objetos, etc)
        Time.timeScale = 1f; // Reanuda el tiempo
    }

    private void NewGame()
    {
        ClearScene();

        // Habilita el Blade solo cuando el juego realmente comienza
        if (blade != null) // Añadir una comprobación de null por seguridad
        {
            blade.enabled = true;
        }

        spawner.enabled = true; // El spawner también debe controlarse

        score = 0;
        scoreText.text = score.ToString();

        finalText.gameObject.SetActive(false);

        timer = 0f;
        UpdateTimer();

        lives = 3;




    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);

        foreach (Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsByType<Bomb>(FindObjectsSortMode.None);

        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();


    } 
    
    public void UpdateLives()
    {

        lives--;
        livesText.text = lives.ToString();
        

        if (lives <= 0)
        {
            Explode();
            NewGame();
        }
    }

    public void Explode()
    {
        // Deshabilita el Blade cuando explota una bomba o termina el juego
        if (blade != null)
        {
            blade.enabled = false;
        }
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence());

    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = 0f;
        finalText.text = "GAME OVER";
        finalText.gameObject.SetActive(true);

        //esperar 2 segundos antes de empezar
        float waitTime = 2f;
        float timer = 0f;
        while (timer < waitTime)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        finalText.gameObject.SetActive(false);
        fadeImage.color = Color.clear;
        startPanel.SetActive(true);  // Permite reiniciar

    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        timer += Time.deltaTime;
        //UpdateTimer();

        if (timer >= time)
        {
            Explode();
            NewGame();
        }

        UpdateTimer();


    }

    private void UpdateTimer()
    {
        int seconds = Mathf.FloorToInt(timer);

        timerText.text = seconds.ToString();

    }

    








}