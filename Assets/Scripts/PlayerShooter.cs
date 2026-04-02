using PurrNet.Prediction;
using UnityEngine;

public class PlayerShooter : PredictedIdentity<PlayerShooter.ShootInput, PlayerShooter.ShootState>
{
    [SerializeField] private float fireRate = 3;
    [SerializeField] private int damage = 35;
    [SerializeField] private Vector3 centerOfCamera = new Vector3(0, 1.5f, 0);
    [SerializeField] private float knockbackStrength = 5;
    [SerializeField] private ParticleSystem muzzleFlash;
    public float shootCooldown => 1 / fireRate;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InputReader inputReader;

    private PredictedEvent onShoot;

    protected override void LateAwake()
    {
        base.LateAwake();
        onShoot = new PredictedEvent(predictionManager, this);
        onShoot.AddListener(OnShootEvent);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onShoot.RemoveListener(OnShootEvent);
    }

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
        onShoot?.Invoke();
        var forward = playerMovement.currentInput.cameraForward ?? currentState.lastKnownForward;
        currentState.lastKnownForward = forward;

        var position = transform.TransformPoint(centerOfCamera);

        if (!Physics.Raycast(position + forward * 0.5f, forward, out RaycastHit hit))
        {
            return;
        }

        if (hit.transform.TryGetComponent(out PlayerHealth health))
        {
            health.ChangeHealth(-damage);
        }

        if (hit.transform.TryGetComponent(out PlayerMovement otherMovement))
        {
            Vector3 knockback = forward.normalized * knockbackStrength * 5f;
            knockback.y = knockbackStrength;
            otherMovement.Knockback(knockback);
        }
    }
    private void OnShootEvent()
    {
        muzzleFlash.Play();
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
