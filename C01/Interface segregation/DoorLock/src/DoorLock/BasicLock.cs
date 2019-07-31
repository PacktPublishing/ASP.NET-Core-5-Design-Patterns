using System;

namespace DoorLock
{
    public class BasicLock : ILock
    {
        public BasicLock(string expectedSignature)
        {
            ExpectedSignature = expectedSignature ?? throw new ArgumentNullException(nameof(expectedSignature));
        }

        public string ExpectedSignature { get; }

        public bool IsLocked { get; private set; }

        public bool DoesMatch(IKey key)
        {
            return key.Signature.Equals(ExpectedSignature);
        }

        public void Lock(IKey key)
        {
            if (!DoesMatch(key))
            {
                throw new KeyDoesNotMatchException(key);
            }
            IsLocked = true;
        }

        public void Unlock(IKey key)
        {
            if (!DoesMatch(key))
            {
                throw new KeyDoesNotMatchException(key);
            }
            IsLocked = false;
        }
    }

    public class BasicKey : IKey
    {
        public BasicKey(string signature)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }

        public string Signature { get; }
    }

    public interface ILock
    {
        /// <summary>
        /// Gets if the lock is locked or not.
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// Locks the lock using the specified <see cref="IKey"/>.
        /// </summary>
        /// <param name="key">The <see cref="IKey"/> used to lock the lock.</param>
        /// <exception cref="KeyDoesNotMatchException">Thrown when the key's <see cref="IKey.Signature"/> does not match the expected signature.</exception>
        void Lock(IKey key);

        /// <summary>
        /// Unlocks the lock using the specified <see cref="IKey"/>.
        /// </summary>
        /// <param name="key">The <see cref="IKey"/> used to unlock the lock.</param>
        /// <exception cref="KeyDoesNotMatchException">Thrown when the key's <see cref="IKey.Signature"/> does not match the expected signature.</exception>
        void Unlock(IKey key);

        /// <summary>
        /// Validate that the key's <see cref="IKey.Signature"/> match the expected signature.
        /// </summary>
        /// <param name="key">The <see cref="IKey"/> to validate.</param>
        /// <returns><c>true</c> if the key's <see cref="IKey.Signature"/> match the expected signature; otherwise <c>false</c>.</returns>
        bool DoesMatch(IKey key);
    }

    public interface IKey
    {
        /// <summary>
        /// Gets the key's signature that will open matching locks.
        /// </summary>
        string Signature { get; }
    }

    public class KeyDoesNotMatchException : Exception
    {
        public KeyDoesNotMatchException(IKey key)
            : base($"The key {key.Signature} does not match the lock's expected signature.")
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public IKey Key { get; }
    }
}
