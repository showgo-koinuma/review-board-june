using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    bool m_isGrab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CanGrabWall")
        {
            m_isGrab = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        m_isGrab = false;
    }
    public bool isGrab()
    {
        return m_isGrab;
    }
}
