using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public GameObject uiInfoPaintContainer;
    public TextMeshProUGUI uiInfoPaintMessage;
    private string currentPaintName;
    private string currentArtistName;
    private int collectedPieces = 0;
    public AudioSource switchSound;
    private float typingTime = 0.04f;
    public WeaponSwitch weaponSwitch;
    public Material jigsawMaterial;
    public int pieceMaterialCount = 0;

    public GameObject cinematicPuzzlePaints;
    public GameObject cameraPaint2;
    public GameObject cameraPaint;
    public GameObject cameraPaint1;
    public PlayableDirector timelineDirector;
    public Canvas[] canvases; // Canvases to deactivate during the cinematic
    public AudioSource ambientMusicSource; // Music to control during the cinematic
    private Coroutine musicCoroutine;

    [SerializeField] private GameObject firePrefab1; // Reference to the first fire prefab
    [SerializeField] private GameObject firePrefab2; // Reference to the second fire prefab

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

    public void ChangePieceMaterial(GameObject piece)
    {
        // Check if the player has the piece in hand
        bool hasPieceInHand = weaponSwitch.GetCurrentItemName() == "Piece";

        if (hasPieceInHand)
        {
            // Check if the piece already has the jigsaw material
            if (piece.GetComponent<Renderer>().material == jigsawMaterial)
            {
                uiInfoPaintMessage.text = "This piece already has the jigsaw material";
                return;
            }

            // Check if any piece in the gameTransform already has the jigsaw material
            foreach (Transform child in JigsawPuzzle.Instance.gameTransform)
            {
                if (child.GetComponent<Renderer>().material == jigsawMaterial)
                {
                    uiInfoPaintMessage.text = "A piece already has the jigsaw material";
                    return;
                }
            }

            piece.GetComponent<Renderer>().material = jigsawMaterial;
            piece.tag = "Untagged"; // Remove the tag "PuzzlePiece"
            weaponSwitch.RemoveItemFromInventory("Piece", true);
            weaponSwitch.SwitchToNextItem();
            pieceMaterialCount++;

            // Check if the first 8 pieces have the jigsaw material
            if (pieceMaterialCount >= 8)
            {
                AddPieceInteractableScripts();
            }
        }
        else
        {
            uiInfoPaintMessage.text = "You need to have a puzzle piece in hand to change it";
        }
    }

    private void AddPieceInteractableScripts()
    {
        Transform parentTransform = JigsawPuzzle.Instance.gameTransform;

        foreach (Transform piece in parentTransform)
        {
            piece.tag = "PuzzlePiece";
            if (piece.gameObject.GetComponent<PieceInteractable>() == null)
            {
                piece.gameObject.AddComponent<PieceInteractable>();
            }
        }
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
        uiInfoPaintContainer.SetActive(true);

        string firstMessage = $"'{paintName}' by {artistName}";
        string secondMessage = "There's a switch below. Will you push it? Y/N";
        uiInfoPaintMessage.text = string.Empty;

        // Show the first message letter by letter
        foreach (char letter in firstMessage)
        {
            uiInfoPaintMessage.text += letter;
            yield return new WaitForSecondsRealtime(typingTime);
        }

        // Wait for 2 seconds before showing the second message
        yield return new WaitForSecondsRealtime(2);
        uiInfoPaintMessage.text = string.Empty;

        // Show the second message letter by letter
        foreach (char letter in secondMessage)
        {
            uiInfoPaintMessage.text += letter;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Select Yes");
            switchSound.Play(); // Play the sound effect

            // Add the painting to the checkedPaints set only if the player selects Yes
            PaintManager.Instance.AddCheckedPainting(PaintManager.Instance.GetCurrentPaintName());

            CheckPuzzleOrder();

            uiInfoPaintContainer.SetActive(false);
            Time.timeScale = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Select No");
            // The painting is not added to the checkedPaints set

            uiInfoPaintContainer.SetActive(false);
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
                StartCinematic();
            }
        }
        else
        {
            Debug.Log("Incorrect order. Resetting puzzle.");
            ResetPuzzle();
        }
    }

    private void StartCinematic()
    {
        // Deactivate the Canvases during the cinematic
        SetCanvasesActive(false);

        // Stop the ambient music
        if (ambientMusicSource != null)
        {
            ambientMusicSource.Stop();
        }

        // Activate the necessary objects for the cinematic
        cinematicPuzzlePaints.SetActive(true);
        cameraPaint2.SetActive(true);
        cameraPaint.SetActive(true);
        cameraPaint1.SetActive(true);

        // Start the PlayableDirector
        timelineDirector.Play();
        timelineDirector.stopped += OnTimelineStopped;
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        if (director == timelineDirector)
        {
            EndCinematic();
        }
    }

    private void EndCinematic()
    {
        // Reactivate the Canvases after the cinematic
        SetCanvasesActive(true);

        // Play the ambient music
        if (ambientMusicSource != null)
        {
            ambientMusicSource.Play();
        }

        // Deactivate the cinematic objects
        cinematicPuzzlePaints.SetActive(false);
        cameraPaint2.SetActive(false);
        cameraPaint.SetActive(false);
        cameraPaint1.SetActive(false);

        // Deactivate the PlayableDirector
        timelineDirector.gameObject.SetActive(false);

        // Activate the fire prefabs
        ActivateFirePrefabs();
    }

    private void ActivateFirePrefabs()
    {
        if (firePrefab1 != null)
        {
            firePrefab1.SetActive(true);
        }

        if (firePrefab2 != null)
        {
            firePrefab2.SetActive(true);
        }
    }

    private void SetCanvasesActive(bool state)
    {
        foreach (Canvas canvas in canvases)
        {
            canvas.gameObject.SetActive(state);
        }
    }

    private IEnumerator StartAmbientMusicAfterDelay()
    {
        yield return new WaitForSeconds(13f);
        if (timelineDirector.state == PlayState.Playing)
        {
            ambientMusicSource.Play();
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