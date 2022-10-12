using Characters.Player;
using Characters.Player.Move;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class MovementSound : MonoBehaviour
{
    [SerializeField] private AudioSource movementAudio;
    [SerializeField] private MoveInputHandler moveInputHandler;
    [SerializeField] private MoveDelta moveDelta;
    [SerializeField] private float soundDelay;

    private Coroutine coroutine;

    void Start()
    {
        SoundManager.Instance.AddAudioSource(movementAudio.clip.name, movementAudio);

        moveInputHandler.forward.Execute.AddListener(MovementSound_Play);
        moveInputHandler.backward.Execute.AddListener(MovementSound_Play);
        moveInputHandler.left.Execute.AddListener(MovementSound_Play);
        moveInputHandler.right.Execute.AddListener(MovementSound_Play);
    }

    IEnumerator MovementSoundRepeat()
    {
        while (PlayerStatus.IsMoving && !Utils.Compare(moveDelta.GetLastDelta(), Vector3.zero) && !PlayerStatus.IsJumping) // moveDelta.GetDelta �ʿ�
        {
            if (PlayerStatus.IsCrouching)
            {
                SoundManager.Instance.SetVolume("PlayerWalkingVolume", -17f);
            }
            else
            {
                SoundManager.Instance.SetVolume("PlayerWalkingVolume", -12f);
            }
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
