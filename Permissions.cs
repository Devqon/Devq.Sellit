using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Localization;
using Orchard.Security.Permissions;

namespace Devq.Sellit
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission AddProduct = new Permission { Name = "AddProduct" };
        public static readonly Permission ManageProducts = new Permission { Name = "ManageProducts" };

        public virtual Feature Feature { get; set; }

        public static Localizer T { get; set; }

        static Permissions()
        {
            T = NullLocalizer.Instance;

            AddProduct.Description = T("Place new advertisements.").Text;
            ManageProducts.Description = T("Manage advertisements.").Text;
        }


        public IEnumerable<Permission> GetPermissions()
        {
            return new[] {
                AddProduct,
                ManageProducts,
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManageProducts, AddProduct}
                },
                new PermissionStereotype {
                    Name = "Anonymous",
                    Permissions = new[] {AddProduct}
                },
                new PermissionStereotype {
                    Name = "Authenticated",
                    Permissions = new[] {AddProduct}
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] {AddProduct}
                },
                new PermissionStereotype {
                    Name = "Moderator",
                    Permissions = new[] {ManageProducts, AddProduct}
                },
                new PermissionStereotype {
                    Name = "Author",
                    Permissions = new[] {AddProduct}
                },
                new PermissionStereotype {
                    Name = "Contributor",
                    Permissions = new[] {AddProduct}
                },
            };
        }

    }
}