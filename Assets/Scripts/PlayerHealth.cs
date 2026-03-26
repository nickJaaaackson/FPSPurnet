using PurrNet.Prediction;
using UnityEngine;

public class PlayerHealth : PredictedIdentity<PlayerHealth.HealthState>
{
    [SerializeField] private int maxHealth;

    protected override HealthState GetInitialState()
    {
        return new HealthState()
        {
            health = maxHealth
        };
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
        Debug.Log("Player Died!");
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
