using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class RandomSelectorButton : MonoBehaviour
{
    [Header("References")]
    public RectTransform scannerBar;
    public Collider2D scannerCollider;
    public Button checkButton;
    public List<GameObject> potentialTargets = new List<GameObject>();
    public TextMeshProUGUI scanResultsText;

    [Header("Settings")]
    public float scanDistance = 100f;
    public float scanSpeed = 0.5f;
    public float returnDelay = 1f;

    private Vector2 originalPosition;
    private HashSet<GameObject> scannedTargets = new HashSet<GameObject>();
    private bool isScanning = false;
    public bool IsScanning { get; private set; } = false;

    private void Start()
    {
        originalPosition = scannerBar.anchoredPosition;
        GetComponent<Button>().onClick.AddListener(StartScan);
    }

    private void StartScan()
    {
        if (IsScanning) return;
        
        scannedTargets.Clear();
        checkButton.interactable = false;
        scanResultsText.text = "Scanning...";
        
        int targetCount = Mathf.Min(Random.Range(1, 4), potentialTargets.Count);
        while (scannedTargets.Count < targetCount)
        {
            GameObject target = potentialTargets[Random.Range(0, potentialTargets.Count)];
            if (!scannedTargets.Contains(target))
            {
                scannedTargets.Add(target);
            }
        }

        StartCoroutine(ScanRoutine());
    }

    private IEnumerator ScanRoutine()
    {
        IsScanning = true;
        scannerCollider.enabled = true;
        
        yield return MoveScanner(originalPosition, originalPosition + Vector2.down * scanDistance);
        
        yield return new WaitForSeconds(returnDelay);
        
        yield return MoveScanner(scannerBar.anchoredPosition, originalPosition);
        
        scannerCollider.enabled = false;
        
        scanResultsText.text = scannedTargets.Count > 0 ? 
            $"Found {scannedTargets.Count} objects" : "No objects found";
        
        checkButton.interactable = true;
        IsScanning = false;
    }

    private IEnumerator MoveScanner(Vector2 startPos, Vector2 endPos)
    {
        float elapsed = 0f;
        while (elapsed < scanSpeed)
        {
            scannerBar.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed/scanSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }
        scannerBar.anchoredPosition = endPos;
    }

    public HashSet<GameObject> GetScannedTargets() => scannedTargets;
    public bool IsScanComplete() => !isScanning && scannedTargets.Count > 0;
}