using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SkeletonObjectManager : MonoBehaviour
{
    [Header("References")]
    public List<GameObject> skeletonSlots;
    public TextMeshProUGUI objectsTextDisplay;
    public RandomSelectorButton scanner;
    
    private List<string> currentObjects = new List<string>();

    public void SetupWithObjects(List<string> prohibitedObjects)
    {
        foreach (var slot in skeletonSlots)
        {
            slot.SetActive(false);
        }
        
        currentObjects = prohibitedObjects;
        objectsTextDisplay.text = "Hidden Objects: " + string.Join(", ", currentObjects);
        
        scanner.potentialTargets.Clear();
        for (int i = 0; i < Mathf.Min(currentObjects.Count, skeletonSlots.Count); i++)
        {
            skeletonSlots[i].SetActive(true);
            scanner.potentialTargets.Add(skeletonSlots[i]);
        }
    }
}