using System;
using System.Collections.Generic;

namespace Pennywhistle.Domain.Common
{
    /// <summary>
    /// User login responce
    /// </summary>
    public class UserManagerResponse
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
    }
}
