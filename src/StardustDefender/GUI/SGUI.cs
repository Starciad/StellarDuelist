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
            if (!this.IsActive)
            {
                return;
            }

            OnUpdate();
        }
        internal void Draw()
        {
            if (!this.IsActive)
            {
                return;
            }

            OnDraw();
        }

        internal void Enable()
        {
            this.IsActive = true;
        }
        internal void Disable()
        {
            this.IsActive = false;
        }

        protected virtual void OnInitialize() { return; }
        protected virtual void OnUpdate() { return; }
        protected virtual void OnDraw() { return; }
    }
}
