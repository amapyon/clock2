namespace clock
{
    interface ITimer
    {
        void Start();
        void Pause();
        void Clear();
        void Increment(string time);
        void ChangeFormat();
    }
}
