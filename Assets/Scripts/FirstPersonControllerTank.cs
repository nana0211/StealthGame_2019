using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;


[RequireComponent(typeof(CharacterController))]
public class FirstPersonControllerTank : MonoBehaviour
{
    public float m_walkingSpeed;
    public float m_runSpeed;
    public float m_rotateSpeed;
    public Rigidbody rigidbody;
    [Range(0f, 1f)] public float m_RunstepLenghten;

    private float rotation_y;
    private CharacterController m_characterController;
    private Camera m_Camera;
    private Transform m_transformObject;

    public Slider m_walkSpeedSlider;
    public Slider m_rotationSpeedSlider;
    public Text m_woundCounterText;
    public Text m_FreezeText;
    public Text m_CountDownTimer;
    public bool m_disableTimer = false;
    [HideInInspector] public int m_woundNumber = 0;
    [HideInInspector] public float m_disableControlTimer = 0;
    [HideInInspector] public float m_timeLeft = 30f;
    [HideInInspector] private bool startInvisible = false;
    [HideInInspector] public bool isGetAttacked = false;
    [HideInInspector] public bool isLocked = false;
    [HideInInspector] public Vector3 forwardDirection;

    public bool IsInvisible
    {
        get
        {
            return startInvisible;
        }
        set
        {
            if (value)
            {
                transform.gameObject.layer = 2;
            }
            else
            {
                transform.gameObject.layer = 9;
                immunityTimeElapsed = 0f;
                isGetAttacked = false;
            }
        }
    }
	
    public float immunityTimeElapsed = 0f;
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_transformObject = GetComponent<Transform>();
        rotation_y = gameObject.transform.rotation.eulerAngles.y;
    }
   
    void Update()
    {
        // Walk Speed Slider
        m_walkingSpeed = m_walkSpeedSlider.value;
        m_rotateSpeed = m_rotationSpeedSlider.value;
        m_FreezeText.enabled = false;
        
        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
        // check if the player is on a moving floor. 
        if(!isLocked)
        ReadInput();
        UpdateWoundCounter();
        UpdateCountdownTimer();
        if (isGetAttacked)
        {
            startInvisible = IsImmunityOver(StateController.immunityTime);
            if (!startInvisible)
            {
                StateController.setImmTime = false;
            }
            
        }
        IsInvisible = startInvisible;
    }

   
    
   void ReadInput()
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        if (m_disableControlTimer > 0f)
        {
            horizontal = 0f;
            vertical = 0f;
            m_disableControlTimer -= Time.deltaTime;
            m_FreezeText.enabled = true;
        }
        else
        {
            m_FreezeText.enabled = false;
        }

        forwardDirection = new Vector3(0f, 0f, vertical);
        forwardDirection = m_Camera.transform.TransformDirection(forwardDirection);
        forwardDirection.y = 0;
        m_characterController.SimpleMove(forwardDirection * m_walkingSpeed);
        rotation_y += horizontal * m_rotateSpeed * Time.deltaTime;
        m_transformObject.rotation = Quaternion.Euler(0f, rotation_y, 0f);
    
    }
    public void IncWoundCounter(int woundInc)
    {
        m_woundNumber += woundInc;
    }

    public void ResetWoundCounter()
    {
        m_woundNumber = 0;
    }

    public void DisableControls(float seconds)
    {
        m_disableControlTimer = seconds;
    }

    void UpdateWoundCounter()
    {
        m_woundCounterText.text = "Wounds: " + m_woundNumber;
    }

    private void UpdateCountdownTimer()
    {
        if (!m_disableTimer)
        {
            m_timeLeft -= Time.deltaTime;
            m_CountDownTimer.text = "Time Left: " + Mathf.Round(m_timeLeft);
        }

        if(m_timeLeft < 10f)
        {
            m_CountDownTimer.color = Color.red;
        }
        else
        {
            m_CountDownTimer.color = Color.white;
        }

        if(m_timeLeft <= 0)
        {
            // The End
            SceneManager.LoadScene(1); 
        }
    }
	public bool IsImmunityOver(float duration)
	{
		immunityTimeElapsed += Time.deltaTime;
		//Debug.Log("immunity time elapsed " + immunityTimeElapsed);
		return (immunityTimeElapsed <= duration);
	}

}
