using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private bool m_Alive;
    private Renderer m_Renderer;
    private bool m_IsOnEdge;
    private bool m_WillBeAlive;
    [SerializeField]
    private Material m_AliveColor;
    [SerializeField]
    private Material m_DeathColor;
    [SerializeField]
    private Material m_OnEdgeColor;

    public void Initialize(bool isOnEdge)
    {
        m_Alive = false;
        m_Renderer = GetComponent<Renderer>();
        m_IsOnEdge = isOnEdge;
    }

    public Block CheckIfOnEndge()
    {
        if (m_IsOnEdge)
        {
            return null;
        }
        else
        {
            return this;
        }
    }

    public void ComeAlive()
    {
        m_Alive = true;
        m_Renderer.material = m_AliveColor;
    }

    public void Die()
    {
        m_Alive = false;
        m_Renderer.material = m_DeathColor;
    }

    public bool GetStatus()
    {
        return m_Alive;
    }

    public void UpdateBlock()
    {
        if (m_WillBeAlive)
        {
            ComeAlive();
        }
        else
        {
            Die();
        }
    }

    public void WillBeAlive(bool willBeAlive)
    {
        m_WillBeAlive = willBeAlive;
    }

    public void IsOnEdge()
    {
        m_Renderer.material = m_OnEdgeColor;
    }
}
