using System;

namespace StellarDuelist.Core.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SEntityRegisterAttribute : Attribute
    {
        private readonly Type definitionType;

        public SEntityRegisterAttribute(Type definitionType)
        {
            this.definitionType = definitionType;
        }

        public SEntityDefinition GetEntityDefinition()
        {
            return (SEntityDefinition)Activator.CreateInstance(this.definitionType);
        }
    }
}
