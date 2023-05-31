using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_moveSpeed = 50f;

    Rigidbody2D m_playerRb;

    Vector2 m_scale;
    float m_lscaleX;
    float m_horizontal;

    // Start is called before the first frame update
    void Start()
    {
        m_playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //ç∂âEà⁄ìÆ
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_playerRb.AddForce(Vector2.right * m_horizontal * m_moveSpeed, ForceMode2D.Force);

        //ç∂âEîΩì]
        if (transform.localScale.x * m_horizontal < 0)
        {
            m_lscaleX = transform.localScale.x;
            m_lscaleX *= -1;
            m_scale = new Vector2(m_lscaleX, transform.localScale.y);
            transform.localScale = m_scale;
        }
    }

    private void FixedUpdate()
    {
        
    }
}
