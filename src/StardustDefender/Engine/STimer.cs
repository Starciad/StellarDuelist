using System;

namespace StardustDefender.Engine
{
    internal sealed class STimer
    {
        internal bool IsFinished => this.enable && this.current == 0;

        internal bool IsEnable => this.enable;
        internal bool IsActive => this.active;

        internal float TargetDelay => this.target;
        internal float CurrentDelay => this.target;

        private bool enable = true;
        private bool active = false;

        private float target = 0;
        private float current = 0;

        internal STimer()
        {
            SetDelay(0f);
        }
        internal STimer(float value)
        {
            SetDelay(value);
        }

        internal void Start()
        {
            this.active = true;
        }
        internal void Restart()
        {
            this.current = this.target;
            Start();
        }
        internal void Stop()
        {
            this.active = false;
        }
        internal void Update()
        {
            if (!this.active || !this.enable)
                return;

            if (this.current > 0)
            {
                this.current -= 0.1f;
            }
            else
            {
                this.current = 0;
                this.active = false;
            }
        }

        internal void Enable()
        {
            enable = true;
        }
        internal void Disable()
        {
            enable = false;
        }

        internal void SetDelay(float value)
        {
            this.target = value;
            this.current = value;
        }

        public override string ToString()
        {
            return $"{current}/{target}";
        }
    }
}
