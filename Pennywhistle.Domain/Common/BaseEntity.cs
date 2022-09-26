using System;

namespace Pennywhistle.Domain.Common
{
    /// <summary>
    /// Implements base entity
    /// </summary>
    public class BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
