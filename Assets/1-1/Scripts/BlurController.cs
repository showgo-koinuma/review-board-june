using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurController : MonoBehaviour
{
    GameObject m_player;
    SpriteRenderer m_blurSprite;
    SpriteRenderer m_playerSprite;
    float timer;
    float colorA;
    float outTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindWithTag("Player");
        transform.localScale = m_player.transform.localScale;
        m_playerSprite = m_player.GetComponent<SpriteRenderer>();
        m_blurSprite = GetComponent<SpriteRenderer>();
        m_blurSprite.sprite = m_playerSprite.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        colorA = Time.deltaTime / outTime;
        m_blurSprite.color -= new Color(0, 0, 0, colorA);
        if (m_blurSprite.color.a < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
