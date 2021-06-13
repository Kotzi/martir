using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAreaDetector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<PlayerController>();
        if (player) {
            player.setIsConnected(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        var player = collider.GetComponent<PlayerController>();
        if (player) {
            player.setIsConnected(false);
        }
    }
}
