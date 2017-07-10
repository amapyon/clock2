namespace clock
{
    class NullTimer : ITimer
    {
        public void Start() { }
        public void Pause() { }
        public void Clear() { }
        public void Increment(string time) { }
        public void ChangeFormat() { }
        public void AddWindow(TimerWindow mainWindow) { }
        public void SetThrough(bool through) { }
    }
}
