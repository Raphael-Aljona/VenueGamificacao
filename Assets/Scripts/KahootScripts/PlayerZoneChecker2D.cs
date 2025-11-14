using UnityEngine;
using Photon.Pun;

public class PlayerZoneChecker2D : MonoBehaviour
{
    public int currentZone = -1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Resposta"))
        {
            currentZone = other.GetComponent<AnswerZone2D>().zoneIndex;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        currentZone = -1;
    }

}
