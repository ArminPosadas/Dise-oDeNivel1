using SergioHdz.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class NPCDecisionPanel : MonoBehaviour
{
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;
    [SerializeField] private Button playButton;
    [SerializeField] private QueueManager queueManager;

    private void Start()
    {
        acceptButton.onClick.AddListener(OnAccept);
        rejectButton.onClick.AddListener(OnReject);
        playButton.onClick.AddListener(queueManager.OnPlayClicked);
    }

    private void OnAccept()
    {
        queueManager.AcceptNpc();
    }

    private void OnReject()
    {
        queueManager.RejectNpc();
    }
}
