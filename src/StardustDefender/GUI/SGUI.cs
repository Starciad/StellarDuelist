namespace StardustDefender.GUI
{
    internal abstract class SGUI
    {
        internal bool IsActive { get; private set; }

        internal void Initialize()
        {
            OnInitialize();
        }
        internal void Update()
        {
            if (!IsActive)
                return;

            OnUpdate();
        }
        internal void Draw()
        {
            if (!IsActive)
                return;

            OnDraw();
        }

        internal void Enable()
        {
            IsActive = true;
        }
        internal void Disable()
        {
            IsActive = false;
        }

        protected virtual void OnInitialize() { return; }
        protected virtual void OnUpdate() { return; }
        protected virtual void OnDraw() { return; }
    }
}
