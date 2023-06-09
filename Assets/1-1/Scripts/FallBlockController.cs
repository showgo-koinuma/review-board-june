using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FallBlockController : MonoBehaviour
{
    [SerializeField] float m_fallSpeed = 15f;
    bool m_isFall;
    private void Update()
    {
        if (m_isFall)
        {
            transform.Translate(Vector2.down * m_fallSpeed * Time.deltaTime);
        }
        if (transform.position.y < -8)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_isFall = true;
        }
    }
}
