using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StationTireInspection.Classes;

namespace StationTireInspection.UserControls
{
    public partial class PermissionPicker : UserControl
    {
        private List<Permission> allPermissions = new List<Permission>();
        private List<Permission> pickablePermissions = new List<Permission>();
        private List<Permission> pickedPermissions = new List<Permission>();

        public PermissionPicker()
        {
            InitializeComponent();
            Translator.LanguageChanged += Translate;
        }

        public void InitializePermissions(List<Permission> permissions)
        {
            allPermissions.Clear();
            ClearPermissions(pickablePermissions, lbPermissionsList);
            ClearPermissions(pickedPermissions, lbUserPermissions);

            foreach (var permission in permissions)
            {
                allPermissions.Add(permission);
                AddPermission(permission, pickablePermissions, lbPermissionsList);
            }
        }

        private void PickPermission(Permission permission)
        {
            AddPermission(permission, pickedPermissions, lbUserPermissions);
            RemovePermission(permission, pickablePermissions, lbPermissionsList);
        }

        private void UnpickPermission(Permission permissionToRemove)
        {
            RemovePermission(permissionToRemove, pickedPermissions, lbUserPermissions);
            ClearPermissions(pickablePermissions, lbPermissionsList);

            foreach (var permission in allPermissions)
            {
                AddPermission(permission, pickablePermissions, lbPermissionsList);
            }

            foreach (var permission in pickedPermissions)
            {
                RemovePermission(permission, pickablePermissions, lbPermissionsList);
            }
        }

        public List<Permission> GetPickedPermissions()
        {
            return new List<Permission>(pickedPermissions);
        }

        public void Reset()
        {
            ClearPermissions(pickablePermissions, lbPermissionsList);
            foreach (var permission in allPermissions)
            {
                AddPermission(permission, pickablePermissions, lbPermissionsList);
            }

            ClearPermissions(pickedPermissions, lbUserPermissions);
        }

        private void AddPermission(Permission permission, List<Permission> list,  ListBox listBox)
        {
            list.Add(permission);
            listBox.Items.Add(permission.Name);
        }

        private void RemovePermission(Permission permissionToRemove, List<Permission> list, ListBox listBox)
        {
            Permission existingPermission = null;

            foreach (var permission in list)
            {
                if(permission.BitPosition == permissionToRemove.BitPosition)
                {
                    existingPermission = permission;
                }
            }

            if (existingPermission is null) return;

            list.Remove(existingPermission);
            listBox.Items.Remove(existingPermission.Name);
        }

        public void LoadPermissions(List<Permission> PermmisionsForLoad)
        {
            ClearPermissions(pickablePermissions, lbPermissionsList);
            ClearPermissions(pickedPermissions, lbUserPermissions);

            foreach (var permission in allPermissions)
            {
                AddPermission(permission, pickablePermissions, lbPermissionsList);
            }

            foreach (var permission in PermmisionsForLoad)
            {
                RemovePermission(permission, pickablePermissions, lbPermissionsList);
                AddPermission(permission, pickedPermissions, lbUserPermissions);
            }
        }

        private void ClearPermissions(List<Permission> list, ListBox listBox)
        {
            list.Clear();
            listBox.Items.Clear();
        }

        private void btnAddPermission_Click(object sender, EventArgs e)
        {
            int index = lbPermissionsList.SelectedIndex;
            if (index >= 0) PickPermission(pickablePermissions[index]);
        }

        private void btnRemovePermission_Click(object sender, EventArgs e)
        {
            int index = lbUserPermissions.SelectedIndex;
            if (index >= 0) UnpickPermission(pickedPermissions[index]);
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblTextPermissionsList.Text = "Seznam Oprávnění";
                lblTextUserPermissions.Text = "Oprávnění Uživatele";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblTextPermissionsList.Text = "Permissions List";
                lblTextUserPermissions.Text = "User Permissions";
            }
        }
    }
}
