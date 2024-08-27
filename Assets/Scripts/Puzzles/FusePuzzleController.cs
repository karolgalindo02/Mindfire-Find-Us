using UnityEngine;

public class FusePuzzleController : MonoBehaviour
{
    public static FusePuzzleController Instance { get; private set; }

    [SerializeField] private GameObject fuseGreen;
    [SerializeField] private GameObject fuseRed;
    [SerializeField] private GameObject fuseBlue;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private Material correctMaterial;
    [SerializeField] private Material incorrectMaterial;

    [SerializeField] private Transform fuseGreenPosition;
    [SerializeField] private Transform fuseRedPosition;
    [SerializeField] private Transform fuseBluePosition;

    [SerializeField] private LightingController lightingController;
    [SerializeField] private GameObject lightCollider;
    [SerializeField] private AudioSource fuseSound;
    [SerializeField] private AudioSource lightOnSound;
    [SerializeField] private GameObject electricEffect;
    [SerializeField] private FusePuzzleController fusePuzzleController;

    private bool allFusesLocked = false;
private int fusesPlaced = 0;

private GameObject currentFuse;

public void PlaceFuse()
{
    fusesPlaced++;
}

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
    public void CheckFusePosition(GameObject fuse)
    {
        if (allFusesLocked) return; // Avoid checking if all fuses are already correct

        Fuse fuseScript = fuse.GetComponent<Fuse>();
        if (fuseScript == null || fuseScript.IsLocked) return;

        Transform correctPosition = GetCorrectPosition(fuse);

        if (correctPosition != null && Vector3.Distance(fuse.transform.position, correctPosition.position) < 0.1f)
        {
            // Snap fuse to correct position
            fuse.transform.position = correctPosition.position;
            fuseScript.IsCorrect = true;

            // Lock fuse
            fuseScript.LockFuse();

            // Update light material to correct
            int lightIndex = GetLightIndexByTag(fuse.tag);
            if (lightIndex >= 0 && lightIndex < lights.Length)
            {
                lights[lightIndex].GetComponent<Renderer>().material = correctMaterial;
            }

            // Play sound effect
            fuseSound?.Play();

            // Keep emission active
            EnableEmission(fuse);

            // Check if all fuses are in correct positions
            if (AreAllFusesCorrect())
            {
                allFusesLocked = true; // Block further checks
                OnAllFusesCorrect();
            }
        }
        else
        {
            fuseScript.IsCorrect = false;

            // Update light material to incorrect
            int lightIndex = GetLightIndexByTag(fuse.tag);
            if (lightIndex >= 0 && lightIndex < lights.Length)
            {
                lights[lightIndex].GetComponent<Renderer>().material = incorrectMaterial;
            }

            // Disable emission if not correct
            DisableEmission(fuse);
        }
    }

    private Transform GetCorrectPosition(GameObject fuse)
    {
        if (fuse.CompareTag("FuseGreen"))
        {
            return fuseGreenPosition;
        }
        else if (fuse.CompareTag("FuseRed"))
        {
            return fuseRedPosition;
        }
        else if (fuse.CompareTag("FuseBlue"))
        {
            return fuseBluePosition;
        }
        return null;
    }

    private bool AreAllFusesCorrect()
    {
        return fuseGreen.GetComponent<Fuse>().IsCorrect &&
               fuseRed.GetComponent<Fuse>().IsCorrect &&
               fuseBlue.GetComponent<Fuse>().IsCorrect;
    }

    private void OnAllFusesCorrect()
    {
        // Disable fog effect
        RenderSettings.fog = false;

        // Disable LightCollider
        lightCollider?.SetActive(false);

        // Play all fuses correct sound
        lightOnSound?.Play();

        // Activate electric effect
        if (electricEffect != null)
        {
            electricEffect.SetActive(true);
            Invoke(nameof(DeactivateAnimationPrefab), 3f); // Desactivate after 3 seconds
        }
    }

    private int GetLightIndexByTag(string tag)
    {
        switch (tag)
        {
            case "FuseGreen":
                return 0;
            case "FuseRed":
                return 1;
            case "FuseBlue":
                return 2;
            default:
                return -1;
        }
    }

    private void DeactivateAnimationPrefab()
    {
        electricEffect?.SetActive(false);
    }

    private void EnableEmission(GameObject fuse)
    {
        Renderer renderer = fuse.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    private void DisableEmission(GameObject fuse)
    {
        Renderer renderer = fuse.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.DisableKeyword("_EMISSION");
        }
    }
}