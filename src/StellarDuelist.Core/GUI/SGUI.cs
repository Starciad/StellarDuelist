namespace StellarDuelist.Core.GUI
{
    public abstract class SGUI
    {
        internal bool IsActive { get; private set; }
        private bool previouslyActivated;

        internal void Initialize()
        {
            OnInitialize();
        }
        internal void Update()
        {
            this.IsActive = ConditionToBeDrawn();

            // It has just been activated.
            if (this.IsActive && !this.previouslyActivated)
            {
                Enable();
            }

            // It has just been deactivated.
            if (!this.IsActive && this.previouslyActivated)
            {
                Disable();
            }

            // Update previous state.
            this.previouslyActivated = this.IsActive;

            // Update if enabled.
            if (this.IsActive)
            {
                OnUpdate();
            }
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
            OnEnable();
        }
        public void Disable()
        {
            this.IsActive = false;
            OnDisable();
        }

        protected abstract bool ConditionToBeDrawn();
        protected virtual void OnInitialize() { return; }
        protected virtual void OnUpdate() { return; }
        protected virtual void OnDraw() { return; }

        protected virtual void OnEnable() { return; }
        protected virtual void OnDisable() { return; }
    }
}
