using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    #region Explosion
    [Header("Explosion")]
    [Space(5)]
    public float delay = 3f;
    float countdown;
    bool boomTime = false;
    public float explosionForce = 700f;
    public float radius;
    public GameObject fractured;
    public GameObject pyroBall;

    public AudioSource explosionSound;
    public AudioSource pyroSiz;
    #endregion

    #region Placement
    [Header("Placement")]
    [Space(5)]
    public Camera Camera;
    public GameObject grenadePrefab;
    public GameObject displaySphere;
    public LayerMask ground;
    public float placemntDistance;
    public GameObject explosionEffect;
    public ParticleSystem explosionEffectPrefab;
    public RawImage pyro;
    public RawImage psiBlast;
    #endregion

    #region Input
    [Header("Input")]
    [Space(5)]
    public InputActionReference activatePyro;
    public InputActionReference fire;
    #endregion

    private PlayerInput playerInput;
    private bool isActive = false;

    AudioManager audioManager;


    private void OnTogglePyrokinesis(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        boomTime = isActive;

        if (pyro != null)
            pyro.gameObject.SetActive(isActive);

        if (isActive)
        {
            pyroSiz.Play();
            if (audioManager != null)
                audioManager.PlaySFX(audioManager.cartsHitting);
        }
        else
        {
            pyroSiz.Stop();

            if (psiBlast != null)
                psiBlast.gameObject.SetActive(!isActive);
        }
    }

}