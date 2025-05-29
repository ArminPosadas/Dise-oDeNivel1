using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CheckButtonController : MonoBehaviour
{
    [Header("References")]
    public RandomSelectorButton scanner;
    public List<GameObject> interactiveParents;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public float highlightTime = 0.5f;

    private HashSet<GameObject> scannedTargets;
    private HashSet<GameObject> selectedTargets = new HashSet<GameObject>();
    private bool isChecking = false;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartVerification);
        SetupParentButtons();
    }

    private void SetupParentButtons()
    {
        foreach (var parent in interactiveParents)
        {
            var btn = parent.GetComponent<Button>() ?? parent.AddComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnParentClicked(parent));
            btn.interactable = false;
        }
    }

    private void StartVerification()
    {
        if (!scanner.IsScanComplete()) return;
        
        scannedTargets = scanner.GetScannedTargets();
        selectedTargets.Clear();
        isChecking = true;
        
        foreach (var parent in interactiveParents)
        {
            parent.GetComponent<Button>().interactable = true;
            if (parent.transform.childCount > 0)
            {
                parent.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void OnParentClicked(GameObject parent)
    {
        if (!isChecking) return;

        bool isCorrect = scannedTargets.Contains(parent);
        selectedTargets.Add(parent);
        
        if (parent.transform.childCount > 0)
        {
            StartCoroutine(HighlightChild(parent.transform.GetChild(0).gameObject, 
                              isCorrect ? correctColor : wrongColor));
        }

        if (selectedTargets.Count == scannedTargets.Count)
        {
            bool allCorrect = scannedTargets.SetEquals(selectedTargets);
            Debug.Log(allCorrect ? "Perfect!" : "Missed some targets");
            EndVerification();
        }
    }

    private IEnumerator HighlightChild(GameObject child, Color color)
    {
        var img = child.GetComponent<Image>();
        if (img == null) yield break;
        
        var originalColor = img.color;
        img.color = color;
        yield return new WaitForSeconds(highlightTime);
        img.color = originalColor;
    }

    private void EndVerification()
    {
        isChecking = false;
        foreach (var parent in interactiveParents)
        {
            parent.GetComponent<Button>().interactable = false;
            if (parent.transform.childCount > 0)
            {
                parent.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        GetComponent<Button>().interactable = false;
    }
}