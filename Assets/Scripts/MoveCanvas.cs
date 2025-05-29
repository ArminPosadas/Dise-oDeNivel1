using UnityEngine;
using UnityEngine.UI;

public class MoveCanvas : MonoBehaviour
{
    public RectTransform parent;
    public float distance = 200f;
    public float duration = 0.5f;
    public Button toggleButton;

    private bool isMovedLeft = false;
    private bool isTransitioning = false;

    public void ToggleMove()
    {
        if (isTransitioning) return;

        isTransitioning = true;
        toggleButton.interactable = false;

        float targetX = isMovedLeft ? parent.anchoredPosition.x + distance : parent.anchoredPosition.x - distance;
        LeanTween.moveX(parent, targetX, duration)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                isMovedLeft = !isMovedLeft;
                isTransitioning = false;
                toggleButton.interactable = true;
            });
    }
}