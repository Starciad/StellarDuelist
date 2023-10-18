using StardustDefender.Core.Animation;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Entities.Templates;

namespace StardustDefender.Core.Items
{
    public abstract class SItemRegister
    {
        public SAnimation Animation { get; private set; } = new();
        internal bool CanSpawn => SpawnCondition(SLevelController.Player);

        // Const values
        public const byte SPRITE_SCALE = 16;

        internal void Initialize()
        {
            OnInitialize();
            this.Animation.Initialize();
        }
        internal void ApplyEffect()
        {
            OnEffect(SLevelController.Player);
        }

        protected abstract void OnInitialize();
        protected abstract void OnEffect(SPlayerEntity player);

        protected virtual bool SpawnCondition(SPlayerEntity player) { return true; }
    }
}
