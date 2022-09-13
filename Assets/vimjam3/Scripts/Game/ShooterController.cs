using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using TMPro;

[RequireComponent(typeof(StarterAssetsInputs), typeof(ThirdPersonController))]
public class ShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_AimCamera = null;
    [SerializeField] private float m_AimSensitivity = 0.5f;
    [SerializeField] private float m_NormalSensitivity = 1.0f;
    [SerializeField] private LayerMask m_AimColliderMask = new LayerMask();
    [SerializeField] private float m_RotationLerp = 20.0f;
    [SerializeField] private TMP_Text m_InteractText = null;
    /*[SerializeField] private GameObject m_BulletPrefab = null;
    [SerializeField] private Transform m_SpawnBulletTransform = null;*/

    private static ShooterController m_Instance;

    private StarterAssetsInputs m_Inputs = null;
    private ThirdPersonController m_Controller = null;
    private bool m_ForceHideInteract = false;

    private void Awake()
    {
        m_Inputs = GetComponent<StarterAssetsInputs>();
        m_Controller = GetComponent<ThirdPersonController>();

        m_Instance = this;
    }

    private void Update()
    {
        Vector3 mouseWorldPos = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, m_AimColliderMask))
        {
            mouseWorldPos = hit.point;
        }
        else
        {
            mouseWorldPos = ray.GetPoint(10.0f);
        }

        if (m_Inputs.aim)
        {
            m_AimCamera.gameObject.SetActive(true);
            m_Controller.Sensitivity = m_AimSensitivity;
            m_Controller.RotateOnMove = false;

            Vector3 worldAimTarget = mouseWorldPos;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDir = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * m_RotationLerp);

        }
        else
        {
            m_AimCamera.gameObject.SetActive(false);
            m_Controller.Sensitivity = m_NormalSensitivity;
            m_Controller.RotateOnMove = true;
        }

        if (m_Inputs.shoot)
        {
            /*Vector3 aimDir = (mouseWorldPos - m_SpawnBulletTransform.position).normalized;
            Instantiate(m_BulletPrefab, m_SpawnBulletTransform.position, Quaternion.LookRotation(aimDir, Vector3.up));
            m_Inputs.shoot = false;*/
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (!m_ForceHideInteract)
                m_InteractText.text = $"Press <sprite=\"prompts\" name=\"{m_Inputs.GetInteractKey()}\"> to interact";
            else
            m_InteractText.text = string.Empty;

            if (m_Inputs.interact)
            {
                other.GetComponent<Interactable>().Interact();
                m_Inputs.interact = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (!m_ForceHideInteract)
                m_InteractText.text = $"Press <sprite=\"prompts\" name=\"{m_Inputs.GetInteractKey()}\"> to interact";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            m_InteractText.text = string.Empty;
        }
    }

    public static void SetForceHideInteract(bool newForceHideInteractState)
    {
        m_Instance.m_ForceHideInteract = newForceHideInteractState;
    }
}
