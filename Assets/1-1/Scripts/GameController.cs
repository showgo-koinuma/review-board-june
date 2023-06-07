using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject m_pauseText;
    [SerializeField] GameObject m_gameOverText;
    [SerializeField] GameObject m_continueButton;
    [SerializeField] GameObject m_respawnButton;
    [SerializeField] GameObject m_fromTheBeginningButton;
    GameObject m_player;
    private void Start()
    {
        m_player = GameObject.Find("Player");
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            m_pauseText.SetActive(true);
            m_continueButton.SetActive(true);
            m_respawnButton.SetActive(true);
            m_fromTheBeginningButton.SetActive(true);
        }
        //if (Input.GetButtonDown("Cancel") && m_isPausing)
        //{
        //    Continue();
        //}
    }
    public IEnumerator GameOver()
    {
        Time.timeScale = 0;
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
        m_player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    public void FromTheBeginning()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
