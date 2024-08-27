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
    public bool IsPuzzleSolved()
    {
        return currentStep >= correctOrder.Length;
    }
    private string[] correctOrder = new string[]
    {
        "The Last Supper",
        "The Creation of Adam",
        "Fallen Angel",
        "Starry Night",
        "Guernica",
        "The Persistence of Memory"
    };

    private int currentStep = 0; // Order of the puzzle
    private HashSet<string> checkedPaintings = new HashSet<string>(); // Group of checked paintings

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
        Time.timeScale = 0f;

        uiInfoPaintMessage.text = $"{paintName} by {artistName}.";
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            yesButtonText.text = "Yes<";
            noButtonText.text = "No";
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            yesButtonText.text = "Yes";
            noButtonText.text = "No<";
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (yesButtonText.text == "Yes<")
            {
                Debug.Log("Select Yes");
                switchSound.Play(); // Play the sound effect

                // Add the painting to the checkedPaints set only if the player selects Yes
                PaintManager.Instance.AddCheckedPainting(PaintManager.Instance.GetCurrentPaintName());

                CheckPuzzleOrder();
            }
            else if (noButtonText.text == "No<")
            {
                Debug.Log("Select No");
                // The pinture is not added to the checkedPaints set
            }

            uiInfoPaintContainer.SetActive(false);
            yesButton.SetActive(false);
            noButton.SetActive(false);
            yesButtonText.text = "Yes";
            noButtonText.text = "No";

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
                // Logic for puzzle solved
            }
        }
        else
        {
            Debug.Log("Incorrect order. Resetting puzzle.");
            ResetPuzzle();
        }
    }
    public void AddToCheckedPaintings(string paintName)
    {
        checkedPaintings.Add(paintName);
    }

    private void ResetPuzzle()
    {
        currentStep = 0;
        checkedPaintings.Clear(); // Clean the set
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