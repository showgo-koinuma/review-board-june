using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObjectController : MonoBehaviour
{
    [SerializeField] GameObject m_objectToRespawn;
    [SerializeField] float m_respawnTime = 3f;
    GameObject respawnedObject;
    bool m_destroyedObject;
    float m_timer;
    void Start()
    {
        respawnedObject = Instantiate(m_objectToRespawn, transform.position, transform.rotation);
    }
    void Update()
    {
        if (!respawnedObject)
        {
            m_destroyedObject = true;
        }
        if (m_destroyedObject)
        {
            m_timer += Time.deltaTime;
            if (m_timer > m_respawnTime)
            {
                respawnedObject = Instantiate(m_objectToRespawn, transform.position, transform.rotation);
                m_destroyedObject = false;
                m_timer = 0f;
            }
            
        }
    }
}
