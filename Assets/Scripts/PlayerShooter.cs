using PurrNet.Prediction;
using UnityEngine;

public class PlayerShooter : PredictedIdentity<PlayerShooter.ShootInput, PlayerShooter.ShootState>
{
    [SerializeField] private float fireRate = 3;
    [SerializeField] private float damage = 35;
    [SerializeField] private Vector3 centerOfCamera = new Vector3(0, 1.5f, 0);

    public float shootCooldown => 1 / fireRate;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InputReader inputReader;
    protected override void Simulate(ShootInput input, ref ShootState state, float delta)
    {
        if (state.cooldownTimer > 0)
        {
            state.cooldownTimer -= delta;
            return;
        }

        if (!input.shoot) { return; }

        state.cooldownTimer = shootCooldown;
        Shoot();
    }

    private void Shoot()
    {
        var forward = playerMovement.currentInput.cameraForward ?? currentState.lastKnownForward;
        currentState.lastKnownForward = forward;

        var position = transform.TransformPoint(centerOfCamera);

        if (!Physics.Raycast(position + forward * 0.5f, forward, out RaycastHit hit))
        {
            return;
        }

        if (hit.transform.TryGetComponent(out PlayerHealth health))
        {
            health.ChangeHealth((int)-damage);
        }
    }

    protected override void UpdateInput(ref ShootInput input)
    {
        input.shoot |= inputReader.AttackPressed;
    }

    protected override void ModifyExtrapolatedInput(ref ShootInput input)
    {
        input.shoot = false; // Don't extrapolate shooting, only use actual input
    }
    public struct ShootInput : IPredictedData
    {
        public bool shoot;
        public void Dispose() { }
    }

    public struct ShootState : IPredictedData<ShootState>
    {
        public float cooldownTimer;
        public Vector3 lastKnownForward;
        public void Dispose() { }
    }
}
