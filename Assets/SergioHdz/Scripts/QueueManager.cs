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

        [Header("References")]
        [SerializeField] private GameObject npcPrefab;
        [SerializeField] private Button playButton;

        private Queue<NPCController> npcQueue = new Queue<NPCController>();
        private NPCController currentNpc;  
        private NPCController npcInBooth;  

        private void Start()
        {
            SpawnNpcs(5);
            AdvanceQueue();
            UpdatePlayButton();
        }

        private void SpawnNpcs(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject npcGo = Instantiate(npcPrefab, queuePositions[i].position, Quaternion.identity);
                NPCController npc = npcGo.GetComponent<NPCController>();
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
                npcInBooth.MoveTo(boothPosition.position);
                currentNpc = null;
                AdvanceQueue(); // Solo avanza la fila visual
                UpdatePlayButton(); // Desactiva el botón
            }
        }

        public void AcceptNpc()
        {
            if (npcInBooth != null)
            {
                npcInBooth.MoveTo(acceptPosition.position);
                StartCoroutine(DestroyAfterDelay(npcInBooth));
            }
        }

        public void RejectNpc()
        {
            if (npcInBooth != null)
            {
                npcInBooth.MoveTo(rejectPosition.position);
                StartCoroutine(DestroyAfterDelay(npcInBooth));
            }
        }

        private IEnumerator DestroyAfterDelay(NPCController npc)
        {
            while (Vector3.Distance(npc.transform.position, acceptPosition.position) > 0.1f &&
                   Vector3.Distance(npc.transform.position, rejectPosition.position) > 0.1f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            Destroy(npc.gameObject);

            if (npc == npcInBooth)
            {
                npcInBooth = null;
                UpdatePlayButton(); // Habilita el botón para el siguiente
            }
        }

        private void UpdatePlayButton()
        {
            if (playButton != null)
            {
                playButton.interactable = (npcInBooth == null && currentNpc != null);
            }
        }
    }
}




