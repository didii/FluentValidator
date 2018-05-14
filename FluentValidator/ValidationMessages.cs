using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentValidator {
    /// <summary>
    /// A collection of <see cref="ValidationMessage"/>
    /// </summary>
    public struct ValidationMessages : IEnumerable<ValidationMessage> {
        /// <summary>
        /// An empty collection that should indicate a successful validation result
        /// </summary>
        public static ValidationMessages Empty => new ValidationMessages();

        private ICollection<ValidationMessage> _messages;

        private ICollection<ValidationMessage> Messages => _messages ?? (_messages = new List<ValidationMessage>());

        /// <summary>
        /// True if no validation messages are present which indicates a succesful validation.
        /// </summary>
        public bool Success => Messages.All(m => m.ValidationSeverity != EValidationSeverity.Error);

        /// <summary>
        /// The amount of validation messages this collection has
        /// </summary>
        public int Count => Messages.Count;

        /// <summary>
        /// Adds a single <see cref="ValidationMessage"/> to the collection
        /// </summary>
        /// <param name="message"></param>
        public void Add(ValidationMessage message) {
            Messages.Add(message);
        }

        /// <inheritdoc />
        public IEnumerator<ValidationMessage> GetEnumerator()
        {
            return Messages.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}