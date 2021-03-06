﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

public class Participant : MonoBehaviour
{
    // Participant's movement and response input variables.
    #pragma warning disable 649
    [SerializeField] private SteamVR_Action_Vector2 _joystickMovement;
    [SerializeField] private float _speed = 1f;
    #pragma warning restore 649
    
    private UnityEngine.CharacterController _charController;
    public static bool leftSpawned;
    

    private void Start()
    {
        // Get CharacterController for movement.
        _charController = GetComponent<UnityEngine.CharacterController>();

        // Different spawns for different experiments/scenes.
        switch (UIOptions.experimentID)
        {
            // Randomize left or right spawn of participant.
            case "Individual_GoNoGo" when Random.Range(0, 2) == 0:
                transform.position = new Vector3(-1.5f, 0, -5);
                leftSpawned = true;
                break;
            case "Individual_GoNoGo":
                transform.position = new Vector3(1.5f, 0, -5);
                break;
            case "Individual_TwoChoice":
                transform.position = new Vector3(0,0,-5);
                break;
            case "EntranceHall":
                transform.position = new Vector3(0,0,-12);
                break;
            case "Joint_GoNoGo":
                transform.position = UIOptions.isHost ? new Vector3(-1.5f, 0, -5) : new Vector3(1.5f, 0, -5);
                break;
        }
    }

    private void Update()
    {
        // Do not allow movement in experiment rooms.
        if (SceneManager.GetActiveScene().name == "EntranceHall")
        {
            Move();
        }
    }

    // Origin: Valem, https://www.youtube.com/watch?v=5C6zr4Q5AlA&t=735s&ab_channel=Valem.
    private void Move()
    {
        // Checks whether there is actual input to get teleporting working with joystick movement.
        if (!(_joystickMovement.axis.magnitude > 0.1f)) return;
        
        // Move according to direction player is looking at.
        Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(_joystickMovement.axis.x, 0, _joystickMovement.axis.y));
        _charController.Move(_speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0,9.81f,0 * Time.deltaTime));
    }
}
