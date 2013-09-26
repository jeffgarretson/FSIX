namespace FSIX.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Breeze.WebApi;
    using Newtonsoft.Json.Linq;
    using FSIX.Models;

    [Authorize]
    [BreezeController]
    public class FSIXController : ApiController
    {

        private readonly FSIXRepository _repository;
        public FSIXController()
        {
            _repository = new FSIXRepository(User);
        }

        // Users
        [HttpGet]
        public IQueryable<User> Users()
        {
            return _repository.Users;
        }

        // Permissions
        [HttpGet]
        public IQueryable<Permission> Permissions()
        {
            return _repository.Permissions;
        }

        // Folders
        [HttpGet]
        public IQueryable<Folder> Folders()
        {
            return _repository.Folders;
        }

        // Items
        [HttpGet]
        public IQueryable<Item> Items()
        {
            return _repository.Items;
        }

        // TODO: Logs, Categories, Severities

        // Metadata
        [HttpGet]
        public string Metadata()
        {
            return _repository.Metadata();
        }

        // SaveChanges
        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _repository.SaveChanges(saveBundle);
        }

        // WhoAmI
        [HttpGet]
        public IQueryable<User> WhoAmI()
        {
            return _repository.WhoAmI;
        }

        //[HttpGet]
        //public int Test()
        //{
        //    Media media = _repository.Context.Media.Add(new Media());
        //    media.FileName = "Bob";
        //    media.MimeType = "text/plain";
        //    media.Bytes = 100;
        //    media.CreatedTime = DateTime.Now;
        //    media.ModifiedTime = DateTime.Now;
        //    media.ExpirationTime = DateTime.Now;

        //    //MediaInternalData mediastore = _repository.Context.MediaInternalData.Add(new MediaInternalData());
        //    media.MediaStorage = new MediaStorage();

        //    media.MediaStorage.IV = Enumerable.Repeat((byte)0x20, 32).ToArray();
        //    media.MediaStorage.LocalFileName = "BodyPart_BlahBlahBlah.txt";
        //    int foo = _repository.Context.SaveChanges();
        //    return foo;
        //}

    }
}
