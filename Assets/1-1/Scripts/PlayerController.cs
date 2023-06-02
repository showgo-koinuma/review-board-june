using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_moveSpeed = 5f;
    [SerializeField] float m_maxSpeed = 20f;
    [SerializeField] float m_jumpPower = 10f;
    [SerializeField] float m_grabSpeed = 3f;
    [SerializeField] float m_dashSpeed = 100f;
    [SerializeField] float m_dashTime = 0.1f;

    Rigidbody2D m_playerRb;
    Animator m_animator;
    //移動、反転
    Vector2 m_scale;
    float m_lscaleX;
    float m_horizontal;
    //ジャンプ
    bool m_isGround;
    //壁掴み
    float m_vertical;
    float gravity;
    float m_grabTimer;
    float m_grabAnim;
    //ダッシュ
    bool m_isCanDash;
    Vector2 m_direction;
    bool m_dashTimerIsStart;
    float dashtimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_playerRb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        gravity = m_playerRb.gravityScale;
        m_grabAnim = 1;
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");
        //maxSpeed以上出ないように
        if (Math.Pow(m_playerRb.velocity.x, 2) > Math.Pow(m_maxSpeed, 2))
        {
            float x = m_playerRb.velocity.x;
            float y = m_playerRb.velocity.y;
            m_playerRb.velocity = new Vector2(m_maxSpeed * Math.Sign(x), y);
        }
        if (m_playerRb.velocity.y > m_jumpPower)
        {
            float x = m_playerRb.velocity.x;
            m_playerRb.velocity = new Vector2(x, m_jumpPower);
        }

        //左右反転
        if (transform.localScale.x * m_horizontal < 0)
        {
            m_lscaleX = transform.localScale.x;
            m_lscaleX *= -1;
            m_scale = new Vector2(m_lscaleX, transform.localScale.y);
            transform.localScale = m_scale;
        }

        //ジャンプ
        if (m_isGround && Input.GetButtonDown("Jump"))
        {
            m_playerRb.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);
            m_animator.Play("Jump");
        }

        //壁掴み
        //判定をlinecastする
        Vector2 start = this.transform.position;
        Vector2 lineForGrab = new Vector2(transform.localScale.x * 0.9f, -0.7f);
        Debug.DrawLine(start, start + lineForGrab, Color.red);
        RaycastHit2D hit = Physics2D.Linecast(start, start + lineForGrab);
        if (Input.GetButton("Fire1") && hit.collider)
        {
            if (hit.collider.gameObject.tag == "CanGrabWall")
            {
                if (!(m_vertical == 0))
                {
                    //上下しているときアニメーションを動かす
                    m_grabTimer += Time.deltaTime;
                    if (m_grabTimer > 0.2)
                    {
                        m_grabAnim *= -1;
                        m_animator.SetFloat("GrabAnim", m_grabAnim);
                        m_grabTimer = 0;
                    }
                }
                m_playerRb.velocity = new Vector2(0, m_vertical * m_grabSpeed);
                m_playerRb.gravityScale = 0;
                if (Input.GetButtonDown("Jump"))
                {
                    //壁掴み中にJumpした場合、反転させ、ジャンプ
                    m_lscaleX = transform.localScale.x;
                    m_lscaleX *= -1;
                    m_scale = new Vector2(m_lscaleX, transform.localScale.y);
                    transform.localScale = m_scale;
                    float x;
                    if (transform.localScale.x > 0) { x = 1; }
                    else { x = -1; }
                    m_playerRb.AddForce(new Vector2(x, 1).normalized * m_jumpPower, ForceMode2D.Impulse);
                    m_animator.Play("Jump");
                }
                m_animator.SetBool("Grab", true);
            }
            else
            {
                m_playerRb.gravityScale = gravity;
                m_animator.SetBool("Grab", false);
            }
        }
        else
        {
            m_playerRb.gravityScale = gravity;
            m_animator.SetBool("Grab", false);
        }

        //ダッシュ
        if (Input.GetButtonDown("Dash") && m_isCanDash && m_vertical >= 0)
        {
            m_dashTimerIsStart = true;
            m_direction = new Vector2(Math.Sign(transform.localScale.x), m_vertical).normalized;
            if (m_horizontal == 0 && m_vertical > 0)
            {
                m_direction = Vector2.up;
            }
        }
        if (m_dashTimerIsStart)
        {
            m_isCanDash = false;
            m_playerRb.velocity = m_direction * m_dashSpeed;
            dashtimer += Time.deltaTime;
            if (dashtimer > m_dashTime)
            {
                dashtimer = 0f;
                m_dashTimerIsStart = false;
            }
        }

        //アニメーター
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

    private void FixedUpdate()
    {
        //左右移動
        m_playerRb.AddForce(Vector2.right * m_horizontal * m_moveSpeed, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_isGround = true;
        }
        //m_isGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_isGround = false;
        }
        //m_isGround = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        m_isCanDash = true;
    }
}
