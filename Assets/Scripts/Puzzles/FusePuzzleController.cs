using UnityEngine;
using System.Linq;

public class FusePuzzleController : MonoBehaviour
{
    public static FusePuzzleController Instance { get; private set; }

    [SerializeField] public GameObject[] fuses;
    [SerializeField] private Transform[] initialPositions;
    [SerializeField] private Transform[] finalPositions;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private Material lightMaterial;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color incorrectColor = Color.red;

    [SerializeField] private Transform fuseGreenPosition;
    [SerializeField] private Transform fuseRedPosition;
    [SerializeField] private Transform fuseBluePosition;

    private bool[] isCorrectPosition;

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

    private void Start()
    {
        isCorrectPosition = new bool[fuses.Length];
        ShuffleFuses();
    }

    private void ShuffleFuses()
    {
        System.Random rnd = new System.Random();
        Transform[] shuffledPositions = initialPositions.OrderBy(x => rnd.Next()).ToArray();
        for (int i = 0; i < fuses.Length; i++)
        {
            fuses[i].transform.position = shuffledPositions[i].position;
        }
    }

    public void MoveFuseToPosition(GameObject fuse)
    {
        for (int i = 0; i < finalPositions.Length; i++)
        {
            bool positionOccupied = false;
            for (int j = 0; j < fuses.Length; j++)
            {
                if (fuses[j].transform.position == finalPositions[i].position)
                {
                    positionOccupied = true;
                    break;
                }
            }

            if (!positionOccupied && Vector3.Distance(fuse.transform.position, finalPositions[i].position) < 0.1f)
            {
                fuse.transform.position = finalPositions[i].position;
                CheckFuses();
                return;
            }
        }
    }

    private void CheckFuses()
    {
        for (int i = 0; i < fuses.Length; i++)
        {
            Transform correctPosition = null;
            if (fuses[i].name.Contains("Green"))
            {
                correctPosition = fuseGreenPosition;
            }
            else if (fuses[i].name.Contains("Red"))
            {
                correctPosition = fuseRedPosition;
            }
            else if (fuses[i].name.Contains("Blue"))
            {
                correctPosition = fuseBluePosition;
            }

            if (correctPosition != null && Vector3.Distance(fuses[i].transform.position, correctPosition.position) < 0.1f)
            {
                isCorrectPosition[i] = true;
                lights[i].GetComponent<Renderer>().material.color = correctColor;
            }
            else
            {
                isCorrectPosition[i] = false;
                lights[i].GetComponent<Renderer>().material.color = incorrectColor;
            }
        }
    }

    private void ResetFuses()
    {
        ShuffleFuses();
        foreach (var light in lights)
        {
            light.GetComponent<Renderer>().material.color = Color.black;
        }
    }
}