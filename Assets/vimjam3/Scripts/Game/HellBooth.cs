using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBooth : Interactable
{
    private bool m_HasInteracted = false;

    [SerializeField] private GameObject m_TrapDoor = null;
    [SerializeField] private GameObject m_InvisWalls = null;

    [SerializeField] private float m_DurationVoiceToWalls = 0.1f;
    [SerializeField] private float m_DurationWallsToTrapDoor = 0.1f;

    public override void Interact()
    {
        if (!m_HasInteracted)
        {
            m_HasInteracted = true;
            StartCoroutine(HB());
        }
    }

    IEnumerator HB()
    {
        while (true)
        {
            ShooterController.SetForceHideInteract(true);

            // TODO: Play voice line
            yield return new WaitForSeconds(m_DurationVoiceToWalls);

            // TODO: Spawn Fire FX
            m_InvisWalls.SetActive(true);

            // TODO: Set to length of voice clip
            yield return new WaitForSeconds(m_DurationWallsToTrapDoor);

            m_TrapDoor.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            ShooterController.SetForceHideInteract(false);

            yield break;
        }
    }
}
