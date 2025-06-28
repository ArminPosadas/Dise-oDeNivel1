using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SergioHdz.Scripts
{
    public class QueueManager : MonoBehaviour
    {
        [Header("Positions")]
        [SerializeField] private Transform[] queuePositions;
        [SerializeField] private Transform frontPosition;
        [SerializeField] private Transform boothPosition;
        [SerializeField] private Transform acceptPosition;
        [SerializeField] private Transform rejectPosition;
        
        [Header("Checkpoints")]
        [SerializeField] private Transform acceptCheckpoint;
        [SerializeField] private Transform rejectCheckpoint;

        [Header("References")]
        [SerializeField] private GameObject npcPrefab;
        [SerializeField] private Button playButton;
        [SerializeField] private Image npcPortraitUI;

        private Queue<NpcController> npcQueue = new Queue<NpcController>();
        private NpcController currentNpc;
        private NpcController npcInBooth;

        private void Start()
        {
            SpawnNpcs(5);
            AdvanceQueue();
            UpdatePlayButton();
            ShowNpcImage(null);
        }

        private void SpawnNpcs(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject npcGo = Instantiate(npcPrefab, queuePositions[i].position, Quaternion.identity);
                NpcController npc = npcGo.GetComponent<NpcController>();
                npcQueue.Enqueue(npc);
            }
        }

        public void AdvanceQueue()
        {
            currentNpc = null;

            if (npcQueue.Count == 0)
            {
                Debug.Log("No hay más NPCs en la fila.");
                return;
            }

            currentNpc = npcQueue.Dequeue();
            currentNpc.MoveTo(frontPosition.position);

            int index = 0;
            foreach (var npc in npcQueue)
            {
                npc.MoveTo(queuePositions[index].position);
                index++;
            }

            UpdatePlayButton();
        }

        public void OnPlayClicked()
        {
            if (currentNpc != null && npcInBooth == null)
            {
                npcInBooth = currentNpc;
                
                npcInBooth.OnReachedDestination += HandleNpcReachedDestination;

                npcInBooth.MoveTo(boothPosition.position);
                currentNpc = null;
                AdvanceQueue();
                UpdatePlayButton();
            }
        }
        
        private void HandleNpcReachedDestination(NpcController npc)
        {
            if (npc == npcInBooth && Vector3.Distance(npc.Destination, boothPosition.position) < 0.01f)
            {
                ShowNpcImage(npc);
            }
        }



        public void AcceptNpc()
        {
            if (npcInBooth != null)
            {
                ShowNpcImage(null);
                npcInBooth.MoveTo(acceptCheckpoint.position);
                StartCoroutine(HandleCheckpointThenFinal(npcInBooth, acceptCheckpoint.position, acceptPosition.position));
            }
        }

        public void RejectNpc()
        {
            if (npcInBooth != null)
            {
                ShowNpcImage(null);
                npcInBooth.MoveTo(rejectCheckpoint.position);
                StartCoroutine(HandleCheckpointThenFinal(npcInBooth, rejectCheckpoint.position, rejectPosition.position));
            }
        }

        private IEnumerator DestroyAfterDelay(NpcController npc)
        {
            while (Vector3.Distance(npc.transform.position, acceptPosition.position) > 0.1f &&
                   Vector3.Distance(npc.transform.position, rejectPosition.position) > 0.1f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            // ✅ Mover esta parte antes del Destroy
            if (npc == npcInBooth)
            {
                npcInBooth = null;
                ShowNpcImage(null);
                UpdatePlayButton();
            }

            Destroy(npc.gameObject);
        }


        private void UpdatePlayButton()
        {
            if (playButton != null)
            {
                playButton.interactable = (npcInBooth == null && currentNpc != null);
            }
        }

        private void ShowNpcImage(NpcController npc)
        {
            if (npcPortraitUI != null)
            {
                if (npc != null && npc.GetPortraitSprite() != null)
                {
                    npcPortraitUI.sprite = npc.GetPortraitSprite();
                    npcPortraitUI.gameObject.SetActive(true);
                    npcPortraitUI.transform.SetAsFirstSibling();
                }
                else
                {
                    npcPortraitUI.gameObject.SetActive(false);
                }
            }
        }
        
        private IEnumerator HandleCheckpointThenFinal(NpcController npc, Vector3 checkpoint, Vector3 finalDestination)
        {
            // Esperar a que llegue al checkpoint
            while (Vector3.Distance(npc.transform.position, checkpoint) > 0.1f)
            {
                yield return null;
            }

            // ✅ Ya podemos activar el botón nuevamente
            npcInBooth = null;
            UpdatePlayButton();

            // Luego continuar hacia la posición final
            npc.MoveTo(finalDestination);
            StartCoroutine(DestroyAfterDelay(npc));
        }

    }
}





