using System.Collections;
using UnityEngine;
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

    [Header("paneles de inicio")]
    public GameObject startPanel; // Panel de inicio
    public Button startButton; //boton de inicio
    public Text panelInicioMensaje; // para UI normal

    public int score { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        Time.timeScale = 0f; // Pausa el juego al arrancar
        startPanel.SetActive(true); // Muestra el panel de inicio
        finalText.gameObject.SetActive(false);

        if (startButton != null)
            startButton.onClick.AddListener(StartGame);
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

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();

        finalText.gameObject.SetActive(false);
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);

        foreach (Fruit fruit in fruits) {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsByType<Bomb>(FindObjectsSortMode.None);

        foreach (Bomb bomb in bombs) {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();

        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }
    }

    public void Explode()
    {
        blade.enabled = false;
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

}