using JetBrains.Annotations;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    private ListaDeTarefas dadosGlobais;
    public GameObject taskCard;
    public Transform spawnPosition;
    void Start()
    {
        TextAsset TextAsset = Resources.Load<TextAsset>("data");
        if (TextAsset != null)
        {
            string dadosJson = TextAsset.text;

            dadosGlobais = JsonUtility.FromJson<ListaDeTarefas>(dadosJson);

            foreach (Tarefa t in dadosGlobais.listaDeTarefas)
            { 
                Debug.Log($"ID: {t.id}, Título: {t.titulo}, Status: {t.status}, Conclusão: {t.textoDaConclusao}");

                GameObject bloco = Instantiate(taskCard, spawnPosition.position, Quaternion.identity);
                CardController card = bloco.GetComponent<CardController>();
                card.ConfigurarCard(t);
            }
        }
    }
}
