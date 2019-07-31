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

        public bool IsLocked { get; }

        public bool DoesMatch(IKey key)
        {
            throw new NotImplementedException();
        }

        public void Lock(IKey key)
        {
            throw new NotImplementedException();
        }

        public void Unlock(IKey key)
        {
            throw new NotImplementedException();
        }
    }

    public class BasicKey : IKey
    {
        public BasicKey(string signature)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }

        public string Signature { get; }

        public bool InsertInto(ILock @lock)
        {
            throw new NotImplementedException();
        }

        public void TurnToLock(ILock @lock)
        {
            throw new NotImplementedException();
        }

        public void TurnToUnlock(ILock @lock)
        {
            throw new NotImplementedException();
        }
    }

    public interface ILock
    {
        /// <summary>
        /// Gets the matching key's expected signature.
        /// Only matching keys should be able to lock and unlock the lock.
        /// </summary>
        string ExpectedSignature { get; }

        /// <summary>
        /// Gets if the lock is locked or not.
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// Locks the lock using the specified <see cref="IKey"/>.
        /// </summary>
        /// <param name="key">The <see cref="IKey"/> used to lock the lock.</param>
        /// <exception cref="KeyDoesNotMatchException">Thrown when the key's <see cref="IKey.Signature"/> does not match the <see cref="ExpectedSignature"/>.</exception>
        void Lock(IKey key);

        /// <summary>
        /// Unlocks the lock using the specified <see cref="IKey"/>.
        /// </summary>
        /// <param name="key">The <see cref="IKey"/> used to unlock the lock.</param>
        /// <exception cref="KeyDoesNotMatchException">Thrown when the key's <see cref="IKey.Signature"/> does not match the <see cref="ExpectedSignature"/>.</exception>
        void Unlock(IKey key);

        /// <summary>
        /// Validate that the key's <see cref="IKey.Signature"/> match the <see cref="ExpectedSignature"/>.
        /// </summary>
        /// <param name="key">The <see cref="IKey"/> to validate.</param>
        /// <returns><c>true</c> if the key's <see cref="IKey.Signature"/> match the <see cref="ExpectedSignature"/>; otherwise <c>false</c>.</returns>
        bool DoesMatch(IKey key);
    }

    public interface IKey
    {
        /// <summary>
        /// Gets the key's signature that will open matching locks.
        /// </summary>
        string Signature { get; }

        /// <summary>
        /// Insert the key into the specified <see cref="ILock"/>.
        /// </summary>
        /// <param name="lock">The <see cref="ILock"/> to insert the key into.</param>
        /// <returns><c>true</c> if the key was inserted; otherwise <c>false</c>.</returns>
        bool InsertInto(ILock @lock);

        /// <summary>
        /// Lock the specified <see cref="ILock"/>.
        /// </summary>
        /// <param name="lock">The <see cref="ILock"/> to lock.</param>
        /// <exception cref="KeyDoesNotMatchException">Thrown when the key's <see cref="Signature"/> does not match the <see cref="ILock.ExpectedSignature"/>.</exception>
        void TurnToLock(ILock @lock);

        /// <summary>
        /// Unlock the specified <see cref="ILock"/>.
        /// </summary>
        /// <param name="lock">The <see cref="ILock"/> to unlock.</param>
        /// <exception cref="KeyDoesNotMatchException">Thrown when the key's <see cref="Signature"/> does not match the <see cref="ILock.ExpectedSignature"/>.</exception>
        void TurnToUnlock(ILock @lock);
    }

    public class KeyDoesNotMatchException : Exception
    {
        public KeyDoesNotMatchException(IKey key, ILock @lock)
            : base($"The key {key.Signature} does not match the lock's expected signature of {@lock.ExpectedSignature}.")
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public IKey Key { get; }
    }
}
