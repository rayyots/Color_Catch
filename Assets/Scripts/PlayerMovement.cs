using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int score = 0;
    public string targetColorString = "Red";  // Public string to input the color name manually
    private Color targetColor;
    public Text scoreText;
    public Text targetColorText;
    public Text timerText;
    public Button restartButton;
    private Rigidbody rb;
    public float moveSpeed = 4f;

    private Transform cameraTransform;
    private Vector3 movementDirection;
    private bool gameOver = false;
    private float timer = 60f;
    private bool timerActive = true;

    public AudioClip correctCollectSound;
    public AudioClip incorrectCollectSound;
    private AudioSource audioSource;
    public AudioSource backgroundMusic;
    public AudioClip gameOverSound;  // Game over sound effect

    // Reference to CollectibleSpawner to control spawning
    public CollectibleSpawner collectibleSpawner;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        restartButton.gameObject.SetActive(false);

        // Set the target color from the input string
        SetTargetColor(targetColorString);

        UpdateScoreText();
        timerText.text = "Time Left: " + Mathf.FloorToInt(timer).ToString();

        // Start the collectible spawning
        collectibleSpawner.StartSpawning();
    }

    void Update()
    {
        if (gameOver) return;

        if (timerActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                TimerEnd();
            }
            timerText.text = "Time Left: " + Mathf.FloorToInt(timer).ToString();
        }

        // Player movement logic
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        movementDirection = (forward * moveZ + right * moveX).normalized;

        if (!gameOver)
        {
            rb.velocity = movementDirection * moveSpeed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameOver) return;

        if (other.CompareTag("Collectible"))
        {
            Color collectibleColor = other.GetComponent<Renderer>().material.color;

            if (ColorsMatch(collectibleColor, targetColor))
            {
                score += 10;
                audioSource.PlayOneShot(correctCollectSound);
            }
            else
            {
                score -= 5;
                audioSource.PlayOneShot(incorrectCollectSound);
            }

            UpdateScoreText();
            Destroy(other.gameObject);
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    void TimerEnd()
    {
        gameOver = true;

        // Stop the background music if it's playing
        backgroundMusic.Stop();

        // Play the game over sound
        audioSource.PlayOneShot(gameOverSound);

        // Stop spawning collectibles
        collectibleSpawner.StopSpawning();

        // Show the restart button
        restartButton.gameObject.SetActive(true);

        // Display the final score in the targetColorText
        targetColorText.text = "Game Over! Final Score: " + score;

        // Optionally, change the text color to indicate the game is over
        targetColorText.color = Color.red;  // Change to red or another color to indicate game over
    }



    public void RestartGame()
    {
        score = 0;
        timer = 60f;
        gameOver = false;
        timerActive = true;
        restartButton.gameObject.SetActive(false);

        transform.position = new Vector3(0, 1, 0);  // Reset player position
        rb.velocity = Vector3.zero;  // Stop player movement

        UpdateScoreText();
        timerText.text = "Time Left: " + Mathf.FloorToInt(timer).ToString();

        SetTargetColor(targetColorString);

        // Restart spawning collectibles
        collectibleSpawner.StartSpawning();

        // Play the background music again
        backgroundMusic.Play();
    }

    void SetTargetColor(string colorName)
    {
        targetColor = GetColorByName(colorName);
        targetColorText.text = $"Target {colorName}";
        targetColorText.color = targetColor;
    }

    Color GetColorByName(string colorName)
    {
        switch (colorName.ToLower())
        {
            case "red":
                return Color.red;
            case "blue":
                return Color.blue;
            case "yellow":
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    bool ColorsMatch(Color color1, Color color2)
    {
        return Mathf.Approximately(color1.r, color2.r) &&
               Mathf.Approximately(color1.g, color2.g) &&
               Mathf.Approximately(color1.b, color2.b);
    }
}
