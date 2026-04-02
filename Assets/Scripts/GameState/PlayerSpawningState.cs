using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawningState : PredictedStateNode<PlayerSpawningState.SpawnState>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private RoundRunningState roundRunningState;
    public override void Enter()
    {

        for (int i = 0; i < predictionManager.players.currentState.players.Count; i++)
        {
            PredictedObjectID? newPlayer;
            var player = predictionManager.players.currentState.players[i];

            if (spawnPoints.Count > 0)
            {
                var spawnPoint = spawnPoints[currentState.spawnPointIndex];
                currentState.spawnPointIndex = (currentState.spawnPointIndex + 1) % spawnPoints.Count;
                newPlayer = hierarchy.Create(playerPrefab, spawnPoint.position, spawnPoint.rotation, player);
            }
            else
            {
                newPlayer = hierarchy.Create(playerPrefab, owner: player);
            }

            if (!newPlayer.HasValue)
                return;

            predictionManager.SetOwnership(newPlayer, player);
            roundRunningState.OnPlayerSpawned(player, newPlayer.Value);
        }

        machine.Next();
    }

    public struct SpawnState : IPredictedData<SpawnState>
    {
        public int spawnPointIndex;
        public void Dispose() { }
    }
}
