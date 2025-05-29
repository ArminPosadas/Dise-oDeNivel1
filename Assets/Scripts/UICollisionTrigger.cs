using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class UICollisionTrigger : MonoBehaviour
{
    public float revealDuration = 0.3f;
    private RandomSelectorButton scanner;
    private Dictionary<GameObject, Coroutine> activeReveals = new Dictionary<GameObject, Coroutine>();

    private void Awake() => scanner = FindFirstObjectByType<RandomSelectorButton>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!scanner.IsScanning) return;

        var targets = scanner.GetScannedTargets();
        if (targets.Contains(other.gameObject))
        {
            Transform child = other.transform.GetChild(0);
            if (child != null)
            {
                if (activeReveals.TryGetValue(child.gameObject, out Coroutine existing))
                {
                    StopCoroutine(existing);
                }
                
                Coroutine newReveal = StartCoroutine(FlashObject(child.gameObject));
                activeReveals[child.gameObject] = newReveal;
            }
        }
    }

    private IEnumerator FlashObject(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(revealDuration);
        obj.SetActive(false);
        activeReveals.Remove(obj);
    }

    public void CleanupReveals()
    {
        foreach (var reveal in activeReveals)
        {
            if (reveal.Key != null)
            {
                StopCoroutine(reveal.Value);
                reveal.Key.SetActive(false);
            }
        }
        activeReveals.Clear();
    }
}