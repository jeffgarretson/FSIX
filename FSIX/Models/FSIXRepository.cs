using System;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Security;
using System.Security.Principal;
using Breeze.WebApi;
using System.Data.Entity;

namespace FSIX.Models
{
    public class FSIXRepository : EFContextProvider<FSIXContext>
    {
        public FSIXRepository(IPrincipal user)
        {
            Username = user.Identity.Name.Split('\\')[1];
        }
        public string Username { get; private set; }

        // Users - No filtering (unless we add a password hash column, then that should be excluded)
        public DbQuery<User> Users
        {
            get
            {
                return (DbQuery<User>)Context.Users;
            }
        }

        // Permissions - Return only permissions for folders accessible by logged-in user
        public DbQuery<Permission> Permissions
        {
            get
            {
                return (DbQuery<Permission>)Context.Permissions
                    .Where(p => p.Folder.Permissions.Any(fp => fp.Username == Username));
            }
        }

        // Folders - Return only folders accessible by logged-in user
        public DbQuery<Folder> Folders
        {
            get
            {
                return (DbQuery<Folder>)Context.Folders
                    .Where(f => f.Permissions.Any(p => p.Username == Username));
            }
        }

        // Items - Return only items in folders accessible by logged-in user
        public DbQuery<Item> Items
        {
            get
            {
                return (DbQuery<Item>)Context.Items
                    .Where(i => i.Folder.Permissions.Any(p => p.Username == Username));
            }
        }

        // TODO: Logs, Categories, Severities, Configurations

        // WhoAmI - Returns username of logged-in user
        public string WhoAmI
        {
            get
            {
                return Username;
            }
        }

        #region Save processing

        // TODO: Delegate to helper classes when it gets more complicated

        protected override bool BeforeSaveEntity(EntityInfo entityInfo)
        {
            var entity = entityInfo.Entity;

            if (entity is User) { return BeforeSaveUser(entity as User, entityInfo); }
            if (entity is Permission) { return BeforeSavePermission(entity as Permission, entityInfo); }
            if (entity is Folder) { return BeforeSaveFolder(entity as Folder, entityInfo); }
            if (entity is Item) { return BeforeSaveItem(entity as Item, entityInfo); }

            if (entity is Log) { return BeforeSaveLog(entity as Log, entityInfo); }
            if (entity is Category) { return BeforeSaveCategory(entity as Category, entityInfo); }
            if (entity is Severity) { return BeforeSaveSeverity(entity as Severity, entityInfo); }

            if (entity is Configuration) { return BeforeSaveConfiguration(entity as Configuration, entityInfo); }

            throw new InvalidOperationException("Cannot save entity of unknown type");
        }

        private bool BeforeSaveUser(User user, EntityInfo info)
        {
            // Users can be written only by Admins
            return ValidationContext.Users.Find(Username).UserType == "Admin";
        }

        private bool BeforeSavePermission(Permission permission, EntityInfo info)
        {
            // Permissions can be written only on folders the user owns
            return true;
        }

        private bool BeforeSaveFolder(Folder folder, EntityInfo info)
        {
            if (info.EntityState == EntityState.Added)
            {
                // TODO: Make sure everything has Username set
                return true;
            }
            return folder.Permissions.Any(p => p.IsOwner && p.Username == Username) || throwCannotSaveEntityForThisUser();
        }

        private bool BeforeSaveItem(Item item, EntityInfo info)
        {
            var folder = ValidationContext.Folders.Find(item.Folder);
            return (null == folder)
                       ? throwCannotFindParentFolder()
                       : folder.Permissions.Any(p => p.PermWrite && p.Username == Username) || throwCannotSaveEntityForThisUser();
        }

        private bool BeforeSaveLog(Log log, EntityInfo info)
        {
            throw new NotImplementedException();
        }

        private bool BeforeSaveCategory(Category category, EntityInfo info)
        {
            // Categories cannot be updated from within the application
            return false;
        }

        private bool BeforeSaveSeverity(Severity severity, EntityInfo info)
        {
            // Severities cannot be updated from within the application
            return false;
        }

        private bool BeforeSaveConfiguration(Configuration configuration, EntityInfo info)
        {
            // Configurations can be written only by Admins
            return ValidationContext.Users.Find(Username).UserType == "Admin";
        }

        // "this.Context" is reserved for Breeze save only! A second, lazily instantiated
        // DbContext will be used for db access during custom save validation. See this
        // stackoverflow question and reply for an explanation:
        // http://stackoverflow.com/questions/14517945/using-this-context-inside-beforesaveentity
        private FSIXContext ValidationContext
        {
            get { return _validationContext ?? (_validationContext = new FSIXContext()); }
        }
        private FSIXContext _validationContext;

        private bool throwCannotSaveEntityForThisUser()
        {
            throw new SecurityException("Unauthorized user");
        }

        private bool throwCannotFindParentFolder()
        {
            throw new InvalidOperationException("Invalid item");
        }

        #endregion

    }
}
