using System.Collections.Generic;

namespace FluentValidator
{
    public struct ValidationMessage
    {
        private IDictionary<string, object> _data;
        public EValidationSeverity ValidationSeverity { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string Message { get; set; }
        public IDictionary<string, object> Data => _data ?? (_data = new Dictionary<string, object>());
    }
}