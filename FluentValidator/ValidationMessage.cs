using System.Collections.Generic;

namespace FluentValidator {
    /// <summary>
    /// A single validation message containing all human-readable but also parsable data
    /// </summary>
    public struct ValidationMessage {
        private IEnumerable<string> _paths;
        private IDictionary<string, object> _data;

        /// <summary>
        /// How bad is the validation?
        /// </summary>
        public EValidationSeverity ValidationSeverity { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Paths {
            get => _paths ?? (_paths = new string[0]);
            set => _paths = value;
        }

        public string Message { get; set; }
        public IDictionary<string, object> Data {
            get => _data ?? (_data = new Dictionary<string, object>());
            set => _data = value;
        }
    }
}