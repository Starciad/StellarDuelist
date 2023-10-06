namespace StardustDefender.Core.GUI
{
    public abstract class SGUI
    {
        internal bool IsActive { get; private set; }

        internal void Initialize()
        {
            OnInitialize();
        }
        internal void Update()
        {
            IsActive = ConditionToBeDrawn();

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

        public void Enable()
        {
            this.IsActive = true;
        }
        public void Disable()
        {
            this.IsActive = false;
        }

        protected abstract bool ConditionToBeDrawn();
        protected virtual void OnInitialize() { return; }
        protected virtual void OnUpdate() { return; }
        protected virtual void OnDraw() { return; }
    }
}
