using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public GameObject m_player;
    private void Update()
    {
        transform.position = new Vector3(m_player.transform.position.x + 10, 3, -10);
    }
}
