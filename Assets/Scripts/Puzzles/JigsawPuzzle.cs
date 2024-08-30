using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawPuzzle : MonoBehaviour
{
    public static JigsawPuzzle Instance { get; private set; }

    public Transform gameTransform;
    [SerializeField] private Transform piecePrefab;
    public Material jigsawMaterial;

    public List<Transform> pieces;
    private int emptyLocation;
    private int size;



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

    private void CreateGamePieces(float gapThickness)
    {
        float width = 1 / (float)size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                piece.tag = "PuzzlePiece"; // Asign tag "PuzzlePiece"
                piece.GetComponent<Renderer>().material = jigsawMaterial;
                pieces.Add(piece);
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                                  +1 - (2 * width * row) - width,
                                                  0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";
                if ((row == size - 1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
                    mesh.uv = uv;
                }
            }
        }
    }

    private void ShufflePieces()
    {
        for (int i = 0; i < pieces.Count - 1; i++)
        {
            int rnd = Random.Range(i, pieces.Count);
            if (rnd != emptyLocation && i != emptyLocation)
            {
                (pieces[i], pieces[rnd]) = (pieces[rnd], pieces[i]);
                (pieces[i].localPosition, pieces[rnd].localPosition) = (pieces[rnd].localPosition, pieces[i].localPosition);
            }
        }
    }

    void Start()
    {
        pieces = new List<Transform>();
        size = 3;

        // Verify if there are pieces in the gameTransform
        if (gameTransform.childCount > 0)
        {
            for (int i = 0; i < gameTransform.childCount; i++)
            {
                Transform piece = gameTransform.GetChild(i);
                piece.tag = "PuzzlePiece"; // Verify tag "PuzzlePiece"
                piece.GetComponent<Renderer>().material = jigsawMaterial;
                pieces.Add(piece);
                if (!piece.gameObject.activeSelf)
                {
                    emptyLocation = i;
                }
            }
        }
        else
        {
            CreateGamePieces(0.02f);
        }

        // Mix the pieces
        ShufflePieces();
    }

    public void TryMovePiece(GameObject piece)
    {
        int index = pieces.IndexOf(piece.transform);
        if (index != -1)
        {
            if (SwapIfValid(index, -size, size)) { CheckPuzzleSolved(); return; }
            if (SwapIfValid(index, +size, size)) { CheckPuzzleSolved(); return; }
            if (SwapIfValid(index, -1, 0)) { CheckPuzzleSolved(); return; }
            if (SwapIfValid(index, +1, size - 1)) { CheckPuzzleSolved(); return; }
        }
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
            emptyLocation = i;
            return true;
        }
        return false;
    }

    private void CheckPuzzleSolved()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != i.ToString())
            {
                return;
            }
        }
        foreach (Transform piece in pieces)
        {
            Destroy(piece.GetComponent<PieceInteractable>());
        }

        Debug.Log("You have solved the puzzle!");
    }

    public bool IsPuzzleSolved()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != i.ToString())
            {
                return false;
            }
        }
        return true;
    }
}