using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    private ListaDeTarefas dadosGlobais;
    public GameObject taskCard;
    public List<Transform> spawnPosition;

    private int index = 0;
    private GameObject block;
    private List<int> indexAvailable;

    public TextMeshProUGUI tituloModal;
    public TextMeshProUGUI descModal;

    private GameManager gameManager;
    public int numCards = 10;
    void Start()
    {
        TextAsset TextAsset = Resources.Load<TextAsset>("data");
        if (TextAsset != null)
        {
            string dadosJson = TextAsset.text;

            dadosGlobais = JsonUtility.FromJson<ListaDeTarefas>(dadosJson);
            indexAvailable = new List<int>();
            for (int i = 0; i < dadosGlobais.listaDeTarefas.Count; i++) { 
                indexAvailable.Add(i);
            }
            SpawnNextCard(); 
        }
    }

    public void SpawnNextCard() {
        if (index < numCards)
        {

            int randomIndex = Random.Range(0, indexAvailable.Count);
            int selectedIndex = indexAvailable[randomIndex];

            indexAvailable.Remove(selectedIndex);

            Tarefa t = dadosGlobais.listaDeTarefas[selectedIndex];
            Debug.Log($"ID: {t.id}, Título: {t.titulo}, Status: {t.status}, Conclusão: {t.textoDaConclusao}");

            int randomSpawn = Random.Range(0, spawnPosition.Count);
            Transform selectedSpawn = spawnPosition[randomSpawn];
            block = Instantiate(taskCard, selectedSpawn.position, Quaternion.identity);
            CardController card = block.GetComponent<CardController>();
            card.ConfigurarCard(t);

            card.tituloModal = tituloModal;
            card.descModal = descModal;

            index++;
        }
    }
}
