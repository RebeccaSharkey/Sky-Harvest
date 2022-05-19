using System.Collections;
using UnityEngine;

/// <summary>
/// Monobehaviour that listens for screen transition events and triggers them
/// </summary>
public class ScreenTransitionHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] fadeOutTransitions;
    [SerializeField] private GameObject[] fadeInTransitions;
    [SerializeField] private GameObject raycastBlocker;

    private Coroutine transition;

    private void OnEnable()
    {
        CustomEvents.UI.OnRandomFadeOut += RandomFadeOut;
        CustomEvents.UI.OnRandomFadeIn += RandomFadeIn;
    }

    private void OnDisable()
    {
        CustomEvents.UI.OnRandomFadeOut -= RandomFadeOut;
        CustomEvents.UI.OnRandomFadeIn -= RandomFadeIn;
    }

    private void RandomFadeOut()
    {
        transition = StartCoroutine(RandomFadeOutRoutine());
    }

    private void RandomFadeIn()
    {
        transition = StartCoroutine(RandomFadeInRoutine());
    }

    /// <summary>
    /// Picks and plays a random fade out effect
    /// </summary>
    /// <returns>Ienumerator</returns>
    private IEnumerator RandomFadeOutRoutine()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnSetAllowanceOfInventory?.Invoke(false);

        int index = Random.Range(0, fadeOutTransitions.Length);

        raycastBlocker.SetActive(true);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
        fadeOutTransitions[index].SetActive(true);
        fadeOutTransitions[index].GetComponent<Animation>().Play();
        yield return new WaitForSeconds(fadeOutTransitions[index].GetComponent<Animation>().clip.length);
        CustomEvents.UI.OnTransitionFinish?.Invoke(); //lets scripts that wait for transitions to finish know it's finished
        yield return null;
        fadeOutTransitions[index].SetActive(false);
    }

    /// <summary>
    /// Pick and plays a random fade in eggect
    /// </summary>
    /// <returns>Ienumerator</returns>
    private IEnumerator RandomFadeInRoutine()
    {

        int index = Random.Range(0, fadeOutTransitions.Length);

        fadeInTransitions[index].SetActive(true);
        fadeInTransitions[index].GetComponent<Animation>().Play();
        yield return new WaitForSeconds(fadeInTransitions[index].GetComponent<Animation>().clip.length);
        CustomEvents.UI.OnTransitionFinish?.Invoke();
        yield return null;
        fadeInTransitions[index].SetActive(false);
        raycastBlocker.SetActive(false);
        CustomEvents.InventorySystem.PlayerInventory.OnSetAllowanceOfInventory?.Invoke(true);
    }
}
