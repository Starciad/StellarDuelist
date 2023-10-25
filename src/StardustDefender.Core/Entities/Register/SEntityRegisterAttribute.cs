using System;

namespace StardustDefender.Core.Entities.Register
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SEntityRegisterAttribute : Attribute
    {
        private readonly Type entityHeader;

        public SEntityRegisterAttribute(Type entityHeader)
        {
            this.entityHeader = entityHeader;
        }

        internal SEntityHeader CreateHeader()
        {
            return (SEntityHeader)Activator.CreateInstance(this.entityHeader);
        }
    }
}
