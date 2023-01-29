using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimationHelper : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform playerInventorySlot;
    [SerializeField] private NavMeshAgent playerNavMeshAgent;

    private readonly int playerAnimatorParamID_IsRunning = Animator.StringToHash("Is Running");
    private readonly int playerAnimatorParamID_BusyHands = Animator.StringToHash("Busy Hands");

    private PlayerState playerState;
    private bool isCarrying;

    private void Update()
    {
        UpdatePlayerStateInfo();
        UpdatePlayerAnimatorState();
    }

    private void UpdatePlayerStateInfo()
    {
        float minVelocity = 0.1f;

        playerState = playerNavMeshAgent.velocity.magnitude > minVelocity ? PlayerState.Running : PlayerState.Idle;
        isCarrying = playerInventorySlot.childCount > 0 ? true : false;
    }

    private void UpdatePlayerAnimatorState()
    {
        if (playerAnimator.IsInTransition(0)) return; 

        PlayerState playerState_Animator = playerAnimator.GetBool(playerAnimatorParamID_IsRunning) ? PlayerState.Running : PlayerState.Idle;
        bool isCarrying_Animator = playerAnimator.GetBool(playerAnimatorParamID_BusyHands);

        if (playerState != playerState_Animator)
        {
            bool isRunning = playerState == PlayerState.Running;
            playerAnimator.SetBool(playerAnimatorParamID_IsRunning, isRunning);
        }

        if (isCarrying != isCarrying_Animator)
            playerAnimator.SetBool(playerAnimatorParamID_BusyHands, isCarrying);
    }

    private enum PlayerState { Idle, Running }
}