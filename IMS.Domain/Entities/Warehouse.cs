
using IMS.Domain.Common;

namespace IMS.Domain.Entities
{
    public class Warehouse : AuditableEntity
    {
        public string Name { get; private set; } = default!;
        public string Code { get; private set; } = default!;

        private Warehouse() { }
        public Warehouse(string name, string code)
        {
            SetName(name);
            SetCode(code);
        }

        public void SetName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Warehouse name is required.", nameof(name));

            Name = name.Trim();
        }

        public void SetCode(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Warehouse code is required.", nameof(code));

            Code = code.Trim().ToUpperInvariant();
        }
    }
}
