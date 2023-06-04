using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour
{
    Animator m_animator;
    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_animator.SetBool("CheckPoint", true);
            collision.GetComponent<PlayerController>().m_respawnPosition = transform.position;
        }
    }
}
