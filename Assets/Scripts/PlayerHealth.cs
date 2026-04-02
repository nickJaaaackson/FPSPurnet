using PurrNet;
using PurrNet.Prediction;
using System;
using UnityEngine;

public class PlayerHealth : PredictedIdentity<PlayerHealth.HealthState>
{
    [SerializeField] private int maxHealth;
    public static event Action<PlayerID?> OnPlayerDeath;
    public static Action KillAllPlayers;

    protected override HealthState GetInitialState()
    {
        return new HealthState()
        {
            health = maxHealth
        };
    }
    private void OnEnable()
    {
        KillAllPlayers += OnKillAllPlayers;
    }
    private void OnDisable()
    {
        KillAllPlayers -= OnKillAllPlayers;
    }
    public void ChangeHealth(int change)
    {
        currentState.health += change;

        if (currentState.health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        //Debug.Log($"Player died with owner: {owner}");
        OnPlayerDeath?.Invoke(owner);
        predictionManager.hierarchy.Delete(gameObject);
    }
    private void OnKillAllPlayers()
    {
        predictionManager.hierarchy.Delete(gameObject);
    }

    [ContextMenu("Kill Player")]
    private void KillPlayer()
    {
        ChangeHealth(-9999);
    }

    public struct HealthState : IPredictedData<HealthState>
    {
        public int health;
        public void Dispose() { }

        public override string ToString()
        {
            return $"Health: {health}";
        }
    }
}
