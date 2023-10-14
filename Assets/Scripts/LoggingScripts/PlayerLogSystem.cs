using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogSystem : LoggerGeneric
{
    private FirstPersonControllerTank controller;

    // Start is called before the first frame update
    void Start()
    {
        // Initializer
        controller = GetComponent<FirstPersonControllerTank>();
    }

    public override LogInfo GetCurrentObjectInfo()
    {
        string name = controller.name;
        Vector3 pos = controller.gameObject.transform.position;
        Vector3 rot = controller.gameObject.transform.rotation.eulerAngles;
        Vector3 scale = controller.gameObject.transform.localScale;


        return new PlayerInfo(name, pos, rot, scale, controller.m_timeLeft, controller.m_woundNumber, controller.m_disableControlTimer, 
            controller.m_walkingSpeed, controller.m_rotateSpeed);
    }

    class PlayerInfo : LogInfo
    {
        public string name;

        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public float TimeLeft;
        public int WoundCounter;
        public float DisableControlTimer;
        public float WalkingSpeed;
        public float RotatingSpeed;

        public PlayerInfo(string name, Vector3 position, Vector3 rotation, Vector3 scale, float TimeLeft, int WoundCounter, float DisableControlTimer, float WalkingSpeed, float RotatingSpeed)
        {
            this.name = name;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.TimeLeft = TimeLeft;
            this.WoundCounter = WoundCounter;
            this.DisableControlTimer = DisableControlTimer;
            this.WalkingSpeed = WalkingSpeed;
            this.RotatingSpeed = RotatingSpeed;
        }
    }
    
}
