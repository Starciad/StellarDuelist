using System;

namespace StellarDuelist.Core.Entities.Register
{
    /// <summary>
    /// An attribute used to register entity headers with associated entity classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SEntityRegisterAttribute : Attribute
    {
        private readonly Type entityHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEntityRegisterAttribute"/> class with the specified entity header type.
        /// </summary>
        /// <param name="entityHeader">The type of the associated entity header.</param>
        public SEntityRegisterAttribute(Type entityHeader)
        {
            this.entityHeader = entityHeader;
        }

        /// <summary>
        /// Creates an instance of the associated entity header.
        /// </summary>
        /// <returns>An instance of the associated entity header.</returns>
        internal SEntityHeader CreateHeader()
        {
            return (SEntityHeader)Activator.CreateInstance(this.entityHeader);
        }
    }
}
