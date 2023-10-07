using System;

namespace StardustDefender.Core.Engine
{
    public sealed class STimer
    {
        public bool IsFinished => this.enable && this.current == 0;

        public bool IsEnable => this.enable;
        public bool IsActive => this.active;

        public float TargetDelay => this.target;
        public float CurrentDelay => this.target;

        private bool enable = true;
        private bool active = false;

        private float target = 0;
        private float current = 0;

        public STimer()
        {
            this.target = 0f;
            this.current = 0f;
        }
        public STimer(float value)
        {
            this.target = value;
            this.current = value;
        }

        public void Start()
        {
            this.active = true;
        }
        public void Restart()
        {
            this.current = this.target;
            Start();
        }
        public void Stop()
        {
            this.active = false;
        }
        public void Update()
        {
            if (!this.active || !this.enable)
            {
                return;
            }

            if (this.current > 0)
            {
                this.current -= 0.1f;
            }
            else
            {
                this.current = 0;
                this.active = false;
            }

            this.current = Math.Clamp(this.current, 0, this.target);
        }

        public void Enable()
        {
            this.enable = true;
        }
        public void Disable()
        {
            this.enable = false;
        }

        public void SetDelay(float value)
        {
            this.target = value;
            this.current = Math.Clamp(this.current, 0, this.target);
        }

        public override string ToString()
        {
            return $"{this.current}/{this.target}";
        }
    }
}
