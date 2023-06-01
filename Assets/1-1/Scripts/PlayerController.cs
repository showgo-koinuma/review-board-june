using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_moveSpeed = 5f;
    [SerializeField] float m_maxSpeed = 20f;
    [SerializeField] float m_jumpPower = 10f;
    [SerializeField] float m_grabSpeed = 3f;

    Rigidbody2D m_playerRb;
    Animator m_animator;
    //�ړ��A���]
    Vector2 m_scale;
    float m_lscaleX;
    float m_horizontal;
    //�W�����v
    bool m_isGround;
    //�ǒ͂�
    GameObject m_grabObject;
    bool m_isPushGrab;
    bool m_wallJump;
    float m_vertical;
    float gravity;
    float m_grabTimer;
    float m_grabAnim;

    // Start is called before the first frame update
    void Start()
    {
        m_playerRb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_grabObject = transform.Find("Grab").gameObject;
        gravity = m_playerRb.gravityScale;
        m_grabAnim = 1;
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");
        //���E�ړ�
        m_playerRb.AddForce(Vector2.right * m_horizontal * m_moveSpeed, ForceMode2D.Force);
        //maxSpeed�ȏ�o�Ȃ��悤��
        if (m_playerRb.velocity.x > m_maxSpeed)
        {
            float y = m_playerRb.velocity.y;
            m_playerRb.velocity = new Vector2(m_maxSpeed, y);
        }
        if (m_playerRb.velocity.x < m_maxSpeed * -1)
        {
            float y = m_playerRb.velocity.y;
            m_playerRb.velocity = new Vector2(m_maxSpeed * -1, y);
        }

        //���E���]
        if (transform.localScale.x * m_horizontal < 0)
        {
            m_lscaleX = transform.localScale.x;
            m_lscaleX *= -1;
            m_scale = new Vector2(m_lscaleX, transform.localScale.y);
            transform.localScale = m_scale;
        }

        //�W�����v
        if (m_isGround && Input.GetButtonDown("Jump"))
        {
            m_playerRb.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);
            m_animator.Play("Jump");
        }

        //�ǒ͂�
        if (Input.GetButtonDown("Fire1")) { m_isPushGrab = true; }
        if (Input.GetButtonUp("Fire1")) { m_isPushGrab = false; }
        if (m_isPushGrab)
        {
            var grab = m_grabObject.GetComponent<GrabController>();
            if (grab.isGrab())
            {
                if (!(m_vertical == 0))
                {
                    m_grabTimer += Time.deltaTime;
                    if (m_grabTimer > 0.2)
                    {
                        m_grabAnim *= -1;
                        m_animator.SetFloat("GrabAnim", m_grabAnim);
                        m_grabTimer = 0;
                    }
                }
                //float x = m_playerRb.velocity.x;
                m_playerRb.velocity = new Vector2(0, m_vertical * m_grabSpeed);
                m_playerRb.gravityScale = 0;
                if (Input.GetButtonDown("Jump"))
                {
                    m_lscaleX = transform.localScale.x;
                    m_lscaleX *= -1;
                    m_scale = new Vector2(m_lscaleX, transform.localScale.y);
                    transform.localScale = m_scale;
                    m_wallJump = true;
                }
                m_animator.SetBool("Grab", true);
            }
            else
            {
                if (m_wallJump)
                {
                    float x;
                    if (transform.localScale.x > 0) { x = 1; }
                    else { x = -1; }
                    m_playerRb.AddForce(new Vector2(x, 1).normalized * m_jumpPower, ForceMode2D.Impulse);
                    m_animator.Play("Jump");
                    m_wallJump = false;
                }
                m_playerRb.gravityScale = gravity;
                m_animator.SetBool("Grab", false);
            }
        }
        else
        {
            m_playerRb.gravityScale = gravity;
            m_animator.SetBool("Grab", false);
        }

        //�_�b�V��
        if (Input.GetButtonDown("Dash"))
        {
            Dash();
        }

        //�A�j���[�^�[
        if (m_isGround)
        {
            m_animator.SetBool("Idle", true);
            if (!(m_playerRb.velocity.x == 0))
            {
                m_animator.SetBool("Run", true);
            }
            else
            {
                m_animator.SetBool("Run", false);
            }
        }
        else
        {
            m_animator.SetBool("Run", false);
            m_animator.SetBool("Idle", false);
        }
    }

        private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_isGround = false;
        }
    }

    void Dash()
    {
        Debug.Log("dash");
    }
}
