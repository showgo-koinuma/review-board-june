using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour
{
    [SerializeField] AudioClip m_sound;
    Animator m_animator;
    AudioSource m_aoudioSource;
    bool m_isRed = true;
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_aoudioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && m_isRed)
        {
            m_animator.SetBool("CheckPoint", true);
            collision.GetComponent<PlayerController>().m_respawnPosition = transform.position;
            m_aoudioSource.PlayOneShot(m_sound);
            m_isRed = false;
        }
    }
}
