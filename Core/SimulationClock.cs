public class SimulationClock
{
    public float TimeScale { get; set; } = 1f; 
    public float UpdateIntervalSeconds { get; set; } = 1.0f; 

    private float _accumulator = 0f;

    public bool ShouldUpdate(float deltaTime)
    {
        _accumulator += deltaTime * TimeScale;
        if (_accumulator >= UpdateIntervalSeconds)
        {
            _accumulator -= UpdateIntervalSeconds;
            return true;
        }
        return false;
    }
}