using FSIX.Models;
using FSIX.Models.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FSIX.Web.Controllers
{
    [Authorize]
    public class MediaController : ApiController
    {

        // Use the same data repository the REST API uses
        private readonly FSIXRepository _repository;
        public MediaController()
        {
            _repository = new FSIXRepository(User);
        }

        // GET api/media/1
        public HttpResponseMessage Get(int id)
        {
            int mediaId = id;
            var data = from f in _repository.Context.Media
                       where f.Id == mediaId
                       select f;
            Media media = (Media)data.Single();
            // This will be a file stream, not a MemoryStream
            //MemoryStream ms = new MemoryStream(item.Content);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(media.MimeType);
            response.Content.Headers.ContentDisposition.FileName = media.FileName;
            response.Content.Headers.ContentDisposition.CreationDate = media.CreatedTime;
            response.Content.Headers.ContentDisposition.ModificationDate = media.ModifiedTime;
            return response;
        }

        // POST api/media/5
        [HttpPost]
        //public async Task<IEnumerable<Item>> Post(int id)
        public async Task<int> Post(int id)
        {
            int folderId = id;

            User user = new User();
            user = _repository.Context.Users.Find(User.Identity.Name.Split('\\')[1]);
            
            if (!_repository.VerifyFolderWritePermission(folderId))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            // Verify we have a multipart payload
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string uploadDir = HttpContext.Current.Server.MapPath("~/App_Data");
            MultipartFormDataEncryptedStreamProvider provider = new MultipartFormDataEncryptedStreamProvider(uploadDir);

            try
            {
                // Find the specified folder, create a new Item record, and attach it to the folder
                Folder folder = _repository.Context.Folders.Find(folderId);
                Item item = _repository.Context.Items.Add(new Item());
                if ( (null == folder) || (null == item) )
                {
                    throw new UnauthorizedAccessException("Access denied");
                }
                else
                {
                    item.Type = "File";  // TODO: push this property from Item down to Media

                    // It's really stupid that I have to supply values for these, because
                    // _repository won't trust them anyway.
                    item.CreatedTime = DateTime.UtcNow;
                    item.ModifiedTime = DateTime.UtcNow;
                    item.CreatedBy = user;
                    
                    folder.Items.Add(item);
                }


                // Wait for form data to finish loading
                await Request.Content.ReadAsMultipartAsync(provider);

                // *** TODO: Process form data and set Item attributes (e.g., Note)

                // Process file data
                foreach (var file in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);

                    // Create new Media record, attach it to its parent Item, and set media metadata
                    Media media = _repository.Context.Media.Add(new Media());
                    media.Item = item;
                    media.FileName = fileInfo.Name;
                    media.MimeType = file.Headers.ContentType.ToString();
                    media.Bytes = fileInfo.Length;
                    media.CreatedTime = (null == file.Headers.ContentDisposition.CreationDate) ?
                        DateTime.UtcNow : ((DateTimeOffset)file.Headers.ContentDisposition.CreationDate).UtcDateTime;
                    media.ModifiedTime = (null == file.Headers.ContentDisposition.ModificationDate) ?
                        DateTime.UtcNow : ((DateTimeOffset)file.Headers.ContentDisposition.ModificationDate).UtcDateTime;
                    media.SubmittedBy = user;

                    // Create new MediaStorage record to save media storage/encryption details to DB
                    MediaStorage mediaStorage = _repository.Context.MediaStorage.Add(new MediaStorage());
                    media.MediaStorage = mediaStorage;
                    mediaStorage.LocalFileName = file.LocalFileName;
                    // *** TODO: get the initialization vector from the stream provider
                    mediaStorage.IV = Enumerable.Repeat((byte)0x00, 32).ToArray();

                    // Save changes to the database
                    _repository.Context.SaveChanges();
                }

                // Will this work, or should I just return item.Id as int and leave it up to the client
                // to fetch the new item in a separate request?
                //return _repository.Context.Items
                //    .Include("Media")
                //    .Where(i => i.Id == item.Id);

                return item.Id;

            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        // Updating existing files not currently supported
        // PUT api/media/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // Do this by deleting the Item via the Breeze API
        // DELETE api/media/5
        //public void Delete(int id)
        //{
        //}

    }
}
