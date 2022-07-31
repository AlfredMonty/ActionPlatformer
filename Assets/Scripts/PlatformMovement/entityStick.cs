using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entityStick : MonoBehaviour
{
    public GameObject player;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == player.name)
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == player.name)
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}