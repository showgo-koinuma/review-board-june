using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject m_timerText;
    [SerializeField] GameObject m_pauseText;
    [SerializeField] GameObject m_gameOverText;
    [SerializeField] GameObject m_clearText;
    [SerializeField] GameObject m_clearTimeText;
    [SerializeField] GameObject m_deathCountText;
    [SerializeField] GameObject m_continueButton;
    [SerializeField] GameObject m_respawnButton;
    [SerializeField] GameObject m_fromTheBeginningButton;
    [SerializeField] GameObject m_blackCurtainUp;
    [SerializeField] GameObject m_blackCurtainDown;
    GameObject m_player;
    CinemachineVirtualCamera m_virtualCamera;
    AudioSource[] m_audioSource;
    float m_defaultVolume;
    TextMeshProUGUI m_timerTextText;

    string m_timerMin;
    string m_timerSec;
    bool m_gameClear;
    float m_gameTimer;
    float m_clearTime;
    float m_clearTimer;
    int m_deathCount = 0;
    private void Start()
    {
        m_player = GameObject.Find("Player");
        m_virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        m_audioSource = GetComponents<AudioSource>();
        m_defaultVolume = m_audioSource[0].volume;
        m_timerTextText = m_timerText.GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        m_gameTimer += Time.deltaTime;
        m_timerMin = ((int)m_gameTimer / 60).ToString();
        if (m_timerMin.Length < 2) { m_timerMin = "0" + m_timerMin; }
        m_timerSec = ((int)m_gameTimer % 60).ToString();
        if (m_timerSec.Length < 2) { m_timerSec = "0" + m_timerSec; }
        m_timerTextText.text = $"{m_timerMin}:{m_timerSec}";
        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            m_pauseText.SetActive(true);
            m_continueButton.SetActive(true);
            m_respawnButton.SetActive(true);
            m_fromTheBeginningButton.SetActive(true);
        }
        if (m_gameClear)
        {
            m_clearTimer = m_gameTimer - m_clearTime; //�N���A���Ă���̕b��
            //1�b�ŃY�[���A�����@0.5�b�ŃJ������~����player�ޏ�@1�b�ナ�U���g�i�\��j
            if (m_clearTimer < 0.8)
            {
                m_virtualCamera.m_Lens.OrthographicSize -= 3 * Time.deltaTime / 0.8f;
                m_blackCurtainUp.SetActive(true);
                m_blackCurtainDown.SetActive(true);
                m_blackCurtainUp.transform.localScale += new Vector3(0, 0.25f * Time.deltaTime / 0.8f, 0);
                m_blackCurtainDown.transform.localScale += new Vector3(0, 0.25f * Time.deltaTime / 0.8f, 0);
            }
            if (m_clearTimer > 0.8)
            {
                m_virtualCamera.Follow = null;
            }
            if (m_clearTimer > 1.8)
            {
                m_clearText.SetActive(true);
                m_clearTimeText.SetActive(true);
                int min = (int)m_clearTime / 60;
                int sec = (int)m_clearTime % 60;
                m_clearTimeText.GetComponent<Text>().text = $"�N���A�^�C���@{min}��{sec}�b";
                m_deathCountText.SetActive(true);
                m_deathCountText.GetComponent<Text>().text = $"���񂾉񐔁@{m_deathCount}��";
                m_fromTheBeginningButton.SetActive(true);
                m_audioSource[1].Play();
            }
            if (m_audioSource[0].volume > 0)
            {
                m_audioSource[0].volume -= m_defaultVolume * Time.deltaTime / 1.8f;//1.8�b�Ńt�F�[�h�A�E�g
            }
        }
    }
    public IEnumerator GameOver()
    {
        Time.timeScale = 0;
        m_deathCount++;
        m_player.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);
        yield return new WaitForSecondsRealtime(0.5f);
        m_gameOverText.SetActive(true);
        m_respawnButton.SetActive(true);
        m_fromTheBeginningButton.SetActive(true);
    }
    public void Continue()
    {
        Time.timeScale = 1;
        m_pauseText.SetActive(false);
        m_continueButton.SetActive(false);
        m_respawnButton.SetActive(false);
        m_fromTheBeginningButton.SetActive(false);
    }
    public void Respawn()
    {
        m_pauseText.SetActive(false);
        m_gameOverText.SetActive(false);
        m_continueButton.SetActive(false);
        m_respawnButton.SetActive(false);
        m_fromTheBeginningButton.SetActive(false);
        Time.timeScale = 1;
        m_player.GetComponent<PlayerController>().IsAlive();
        m_player.transform.position = m_player.GetComponent<PlayerController>().m_respawnPosition;
        m_player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        m_player.GetComponent<PlayerController>().DashReset();
        m_player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    public void FromTheBeginning()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title Scene");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_gameClear = true;
        m_clearTime = m_gameTimer;
        GameObject.Find("Player").GetComponent<PlayerController>().IsClear();
    }
}
