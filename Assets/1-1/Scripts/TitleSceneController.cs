using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    //�e�L�X�g�\��
    [SerializeField] GameObject m_toStartText;
    float m_uiTimer;
    float m_uiInterval = 0.5f;
    int m_uiBool = 1;
    //�������
    [SerializeField] GameObject m_instructionsPanel;
    [SerializeField] GameObject m_instructionsCanvas;
    RectTransform m_panelRectTransform;
    int m_InstructionsActive = -1;
    float m_panelScaleY;
    private void Start()
    {
        m_instructionsCanvas.SetActive(false);
        m_panelRectTransform = m_instructionsPanel.GetComponent<RectTransform>();
        m_panelRectTransform.localScale = new Vector3(0.8f, 0, 1);
    }
    void Update()
    {
        //�E�N���b�N�ŃQ�[���V�[����ǂݍ���
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene("SampleScene");
        }
        //m_uiInterval�b���Ƃ�UI��_�ł�����
        m_uiTimer += Time.deltaTime;
        if (m_uiTimer > m_uiInterval)
        {
            m_uiTimer = 0;
            m_uiBool *= -1;
        }
        if (m_uiBool > 0)
        {
            m_toStartText.SetActive(true);
        }
        else
        {
            m_toStartText.SetActive(false);
        }
        //Enter�L�[�ő��������\���A��\������
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_InstructionsActive *= -1;
        }
        if (m_InstructionsActive > 0)
        {
            m_panelScaleY = m_panelRectTransform.localScale.y;
            m_panelScaleY += 0.8f * Time.deltaTime;
            if (m_panelScaleY > 0.8f)
            {
                m_panelScaleY = 0.8f;
            }
            m_panelRectTransform.localScale = new Vector3(0.8f, m_panelScaleY, 1);
        }
        else
        {
            m_instructionsCanvas.SetActive(false);
            m_panelScaleY = m_panelRectTransform.localScale.y;
            m_panelScaleY -= 0.8f * Time.deltaTime;
            if (m_panelScaleY < 0)
            {
                m_panelScaleY = 0;
            }
            m_panelRectTransform.localScale = new Vector3(0.8f, m_panelScaleY, 1);
        }
        if (m_panelScaleY >= 0.8f)
        {
            m_instructionsCanvas.SetActive(true);
        }
        else
        {
            m_instructionsCanvas.SetActive(false);
        }
    }
}
