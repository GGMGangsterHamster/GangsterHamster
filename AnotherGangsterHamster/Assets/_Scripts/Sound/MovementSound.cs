using Characters.Player;
using Characters.Player.Move;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSound : MonoBehaviour
{
    [SerializeField] private AudioSource movementAudio;
    [SerializeField] private MoveInputHandler moveInputHandler;
    [SerializeField] private float soundDelay;

    void Start()
    {
        moveInputHandler.forward.Execute.AddListener(MovementSound_Play);
        moveInputHandler.backward.Execute.AddListener(MovementSound_Play);
        moveInputHandler.left.Execute.AddListener(MovementSound_Play);
        moveInputHandler.right.Execute.AddListener(MovementSound_Play);
    }

    void Update()
    {
        if (!PlayerStatus.IsMoving || PlayerStatus.IsJumping)
        {
            MovementSound_Stop();
        }
    }

    IEnumerator MovementSoundRepeat()
    {
        while (PlayerStatus.IsMoving)
        {
            movementAudio.Play();
            yield return new WaitForSeconds(soundDelay);
            yield return null;  
        }
    }

    public void MovementSound_Play(object obj)
    {
        if (!movementAudio.isPlaying)
        {
            movementAudio.Play();
        }
    }

    public void MovementSound_Stop()
    {
        movementAudio.Stop();
    }
}
