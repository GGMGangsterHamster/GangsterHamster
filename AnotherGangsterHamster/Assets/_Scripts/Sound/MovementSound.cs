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

    private Coroutine coroutine;

    void Start()
    {
        moveInputHandler.forward.Execute.AddListener(MovementSound_Play);
        moveInputHandler.backward.Execute.AddListener(MovementSound_Play);
        moveInputHandler.left.Execute.AddListener(MovementSound_Play);
        moveInputHandler.right.Execute.AddListener(MovementSound_Play);
    }

    IEnumerator MovementSoundRepeat()
    {
        while (PlayerStatus.IsMoving) // moveDelta.GetDelta ÇÊ¿ä
        {
            movementAudio.Play();
            yield return new WaitForSeconds(soundDelay);
        }
    }

    public void MovementSound_Play(object obj)
    {
        if (!movementAudio.isPlaying)
        {
            coroutine = StartCoroutine(MovementSoundRepeat());
        }
    }

    public void MovementSound_Stop()
    {
        movementAudio.Stop();
    }
}
