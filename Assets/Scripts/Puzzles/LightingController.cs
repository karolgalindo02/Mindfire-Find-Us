using UnityEngine;
using System.Collections;

public class LightingController : MonoBehaviour
{
    [SerializeField] private float targetDensity = 0.4f;
    [SerializeField] private float normalDensity = 0f;
    [SerializeField] private float transitionDuration = 0.1f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string stairStartTag = "StairStart";

    private Coroutine currentCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ChangeFogDensity(targetDensity));
        }
        else if (other.CompareTag(stairStartTag))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ChangeFogDensity(normalDensity));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ChangeFogDensity(normalDensity));
        }
    }

    private IEnumerator ChangeFogDensity(float targetDensity)
    {
        float startDensity = RenderSettings.fogDensity;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            RenderSettings.fogDensity = Mathf.Lerp(startDensity, targetDensity, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        RenderSettings.fogDensity = targetDensity;
    }
}