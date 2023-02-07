using System.ComponentModel;

namespace AuthService.Models.Enum
{
    public enum Roles
    {
    
        
        [Description("User")]
        USER = 1,
        [Description("Client")]
        CLIENT = 2,
        [Description("Admin")]
        ADMIN = 3,
        [Description("Super Admin")]
        SUPERADMIN = 4
    }
}
