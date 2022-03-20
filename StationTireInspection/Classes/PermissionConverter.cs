using System;
using System.Collections.Generic;
using System.Text;

namespace StationTireInspection.Classes
{
    public class PermissionConverter
    {
        public static List<Permission> NumberToPermissions(int number, List<Permission> PermFromDB)
        {
            List<Permission> permissions = new List<Permission>();

            for (int i = 0; i < 32; i++)
            {
                if ((number & (1 << i)) > 0)
                {
                    permissions.Add(new Permission(PermFromDB[i].Name, PermFromDB[i].BitPosition));
                }
            }

            return permissions;
        }

        public static int PermissionsToNumber(List<Permission> permissions)
        {
            int number = 0;

            foreach (var permission in permissions)
            {
                number += 1 << permission.BitPosition;
            }

            return number;
        }
    }
}
