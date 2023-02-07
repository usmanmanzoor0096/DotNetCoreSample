using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AuthService.Common.Models.Helper
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }

            public static class CrudOpertions
            {
                public const string Insert = "INSERT", Update = "UPDATE", Delete = "DELETE";
            }

            public static class CrudMessages
            {
                public const string Insert = "Record has been added successfully",
                    Update = "Record has been updated successfully",
                    Delete = "Record has been deleted successfully",
                    NotFound = "No record found";
            }
        }
    }
}
