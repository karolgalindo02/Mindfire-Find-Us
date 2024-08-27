using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public GameObject uiInfoPaintContainer;
    public TextMeshProUGUI uiInfoPaintMessage;
    public TextMeshProUGUI yesButtonText;
    public TextMeshProUGUI noButtonText;
    public GameObject yesButton;
    public GameObject noButton;

    private string currentPaintName;
    private string currentArtistName;
    private int collectedPieces = 0;
    public AudioSource switchSound;

    private string[] correctOrder = new string[]
    {
        "The Last Supper",
        "The Creation of Adam",
        "Fallen Angel",
        "Starry Night",
        "Guernica",
        "The Persistence of Memory"
    };

    private int currentStep = 0; // Índice para rastrear el paso actual en el orden
    private HashSet<string> checkedPaintings = new HashSet<string>(); // Conjunto para rastrear las pinturas revisadas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        HandleInput();
    }

    public void ShowPaintInfo(string paintName, string artistName)
    {
        if (!checkedPaintings.Contains(paintName))
        {
            checkedPaintings.Add(paintName);
            StartCoroutine(DisplayPaintInfo(paintName, artistName));
        }
    }

    private IEnumerator DisplayPaintInfo(string paintName, string artistName)
    {
        // Pausar el juego
        Time.timeScale = 0f;

        uiInfoPaintMessage.text = $"The painting is called {paintName} by {artistName}.";
        uiInfoPaintContainer.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        uiInfoPaintMessage.text = "There's a switch below. Will you push it?";
        yesButton.SetActive(true);
        noButton.SetActive(true);
        yesButtonText.text = "Yes<";
        noButtonText.text = "No";
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            yesButtonText.text = "Yes<";
            noButtonText.text = "No";
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            yesButtonText.text = "Yes";
            noButtonText.text = "No<";
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (yesButtonText.text == "Yes<")
            {
                Debug.Log("Select Yes");
                switchSound.Play(); // Reproduce el sonido del switch
                CheckPuzzleOrder();
            }
            else if (noButtonText.text == "No<")
            {
                Debug.Log("Select No");
                ResetPuzzle();
            }
            uiInfoPaintContainer.SetActive(false);
            yesButton.SetActive(false);
            noButton.SetActive(false);
            yesButtonText.text = "Yes";
            noButtonText.text = "No";

            // Reanudar el juego
            Time.timeScale = 1f;
        }
    }

    private void CheckPuzzleOrder()
    {
        string currentPainting = PaintManager.Instance.GetCurrentPaintName();
        if (currentPainting == correctOrder[currentStep])
        {
            currentStep++;
            if (currentStep >= correctOrder.Length)
            {
                Debug.Log("Puzzle solved!");
                // Aquí podrías implementar lo que sucede cuando se resuelve el puzzle
            }
        }
        else
        {
            Debug.Log("Incorrect order. Resetting puzzle.");
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        currentStep = 0;
        checkedPaintings.Clear(); // Limpiar el conjunto de pinturas revisadas
        // Aquí podrías implementar lo que sucede al reiniciar el puzzle
    }

    public void CollectPiece()
    {
        collectedPieces++;
        Debug.Log("Piece collected. Total pieces: " + collectedPieces);
    }
    public int GetCollectedPieces()
    {
        return collectedPieces;
    }
}