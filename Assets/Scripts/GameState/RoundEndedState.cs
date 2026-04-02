using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;
using UnityEngine;

public class RoundEndedState : PredictedStateNode<RoundEndedState.RoundEndState>
{
    [SerializeField] private float timeToRestart = 3f;
    public override void ViewEnter(bool isVerified)
    {
        if (isVerified)
        {
            Debug.Log($"Successfully enters the round ended state!");
        }
    }
    public override void Enter()
    {
        currentState.roundRestartTimer = timeToRestart;
    }
    protected override void StateSimulate(ref RoundEndState state, float delta)
    {
        if (state.roundRestartTimer > 0)
        {
            state.roundRestartTimer -= delta;
            if (state.roundRestartTimer <= 0)
            {
                PlayerHealth.KillAllPlayers?.Invoke();
                machine.Next();
            }
        }
    }
    public struct RoundEndState : IPredictedData<RoundEndState>
    {
        public float roundRestartTimer;
        public void Dispose() { }
    }


}
