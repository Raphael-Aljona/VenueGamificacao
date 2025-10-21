using Unity.VisualScripting;
using UnityEngine;

public class CloseModal : MonoBehaviour
{
    public GameObject modalCompeticao;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void closeModal()
    {
        modalCompeticao.SetActive(false);
    }
}
