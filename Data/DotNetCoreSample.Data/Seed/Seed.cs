using AuthService.Common.Models.DBModels;
using AuthService.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data.Seed
{
    public static class Seed
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            SeedUsers(modelBuilder);
            SeedRoles(modelBuilder);
            SeedUserRoles(modelBuilder);
        }
        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasData(
                new ApplicationUser
                {
                    Id = "d41c9419-991b-4088-af60-c5a099ef5877",
                    UserName = "superadmin",
                    NormalizedUserName = "SUPERADMIN",
                    Email = "noumankhan@troontechnologies.com",

                    NormalizedEmail = "SUPERADMIN@TROONTECHNOLOGIES.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "+923335000000",
                    PhoneNumberConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAENGJrqoFveCpjG9VnhFC8q9dV0ZF80UFe9eyN+xIDnsm2u26QYY2z9Vcgp2Lhb+M/w==",
                    Avatar = "",
                    FirstName = "Nouman",
                    LastName = "Khan",
                    PasswordChangeRequired = false,
                    ClientId = "402d6579-043a-40b6-bd45-fc63854769ec",
                    Mobile = String.Empty
                },
                  new ApplicationUser
                  {
                      Id = "77f3cf5b-a996-4456-918a-9d527e81001f",
                      UserName = "usmanmanzoor@troontechnologies.com",
                      NormalizedUserName = "usmanmanzoor@troontechnologies.com",
                      Email = "usmanmanzoor@troontechnologies.com",
                      NormalizedEmail = "usmanmanzoor@troontechnologies.com",
                      EmailConfirmed = true,
                      PhoneNumber = "+923335000000",
                      PhoneNumberConfirmed = true,
                      PasswordHash = "AQAAAAEAACcQAAAAEDY1Z/QeVYTeZ3l9UgLxM7XVKeXgkAGkCi5ZUMR+RhLzatCqoSviIZVPbhU/0hfw1A==",
                      Avatar = "",
                      FirstName = "Muhammad",
                      LastName = "Usman",
                      PasswordChangeRequired = false,
                      ClientId = "402d6579-043a-40b6-bd45-fc63854769ec", // TODO : need to add default client as well!!
                      Mobile = String.Empty
                  },
                  new ApplicationUser
                  {
                      Id = "5ebd51ea-d6d2-43cf-82b5-b08395cfcb40",
                      UserName = "paul@troontechnologies.com",
                      NormalizedUserName = "paul@troontechnologies.com",
                      Email = "paul@troontechnologies.com",
                      NormalizedEmail = "paul@troontechnologies.com",
                      EmailConfirmed = true,
                      PhoneNumber = "+923335000000",
                      PhoneNumberConfirmed = true,
                      PasswordHash = "AQAAAAEAACcQAAAAEDY1Z/QeVYTeZ3l9UgLxM7XVKeXgkAGkCi5ZUMR+RhLzatCqoSviIZVPbhU/0hfw1A==",
                      Avatar = "",
                      FirstName = "Paul",
                      LastName = "Dube",
                      PasswordChangeRequired = false,
                      ClientId = "402d6579-043a-40b6-bd45-fc63854769ec", // TODO : need to add default client as well!!
                      Mobile = String.Empty
                  },
                  new ApplicationUser
                  {
                      Id = "6058f09d-5d22-42dc-bded-140eee0ace72",
                      UserName = "abdullah@troontechnologies.com",
                      NormalizedUserName = "abdullah@troontechnologies.com",
                      Email = "abdullah@troontechnologies.com",
                      NormalizedEmail = "abdullah@troontechnologies.com",
                      EmailConfirmed = true,
                      PhoneNumber = "+923335000000",
                      PhoneNumberConfirmed = true,
                      PasswordHash = "AQAAAAEAACcQAAAAEDY1Z/QeVYTeZ3l9UgLxM7XVKeXgkAGkCi5ZUMR+RhLzatCqoSviIZVPbhU/0hfw1A==",
                      Avatar = "",
                      FirstName = "Adullah",
                      LastName = "Khalid",
                      PasswordChangeRequired = false,
                      ClientId = "402d6579-043a-40b6-bd45-fc63854769ec", // TODO : need to add default client as well!!
                      Mobile = String.Empty
                  }
                );
        }

        //AQAAAAEAACcQAAAAEDY1Z/QeVYTeZ3l9UgLxM7XVKeXgkAGkCi5ZUMR+RhLzatCqoSviIZVPbhU/0hfw1A== 
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData
                (
                new IdentityRole { Name = "Super Admin", NormalizedName = "SUPERADMIN", Id = "2e04364b-e9e6-4c94-9253-97a96f7ee04b" },
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", Id = "a3763094-eb71-4bb2-93a7-dd9451d3bf3c" },
                new IdentityRole { Name = "User", NormalizedName = "USER", Id = "57e7e924-659d-4c9c-b477-117799dc3315" },
                new IdentityRole { Name = "Client", NormalizedName = "CLIENT", Id = "3cf1e769-ceed-4a1e-af7d-84b5f05634f4" }

                );
        }
        private static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData
                (
                    new IdentityUserRole<string> { UserId = "d41c9419-991b-4088-af60-c5a099ef5877", RoleId = "2e04364b-e9e6-4c94-9253-97a96f7ee04b" },
                    new IdentityUserRole<string> { UserId = "77f3cf5b-a996-4456-918a-9d527e81001f", RoleId = "2e04364b-e9e6-4c94-9253-97a96f7ee04b" },
                    new IdentityUserRole<string> { UserId = "5ebd51ea-d6d2-43cf-82b5-b08395cfcb40", RoleId = "2e04364b-e9e6-4c94-9253-97a96f7ee04b" },
                    new IdentityUserRole<string> { UserId = "6058f09d-5d22-42dc-bded-140eee0ace72", RoleId = "2e04364b-e9e6-4c94-9253-97a96f7ee04b" }

                );
        }


    }
}
