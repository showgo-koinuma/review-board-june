using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public float m_moveCameraSpeed = 5f;
    public GameObject m_player;
    float m_dis;
    float m_xDis;
    private void Start()
    {
        transform.position = new Vector3(5, 3, -10);
    }
    private void Update()
    {
        m_dis = Vector2.Distance(m_player.transform.position, transform.position);
        m_xDis = m_player.transform.position.x - (transform.position.x - 10);
        if (m_dis  > 1)
        {
            Vector3 dir = Vector2.right * Mathf.Sign(m_xDis) * m_moveCameraSpeed;
            transform.Translate(dir * Time.deltaTime);
        }
        if (transform.position.x < 5)
        {
            transform.position = new Vector3(5, 3, -10);
        }
    }
}
