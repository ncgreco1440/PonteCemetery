using PonteCemetery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If the player goes outside a certain boundary without completing 
/// the sections challenges, a trigger collider around the player will 
/// activate. And all graves that touch this collider will change their 
/// Names to "Lost?"
/// </summary>
public class Lost : MonoBehaviour
{
    public List<GameEvent> m_GameEvents;
    private int m_Iter;

    private void OnTriggerEnter(Collider collider)
    {
        for(m_Iter = 0; m_Iter < m_GameEvents.Count;  m_Iter++)
        {
            if (!m_GameEvents[m_Iter].Completed())
            {
                Player.IsLost();
                break;
            }
        }
    }
}