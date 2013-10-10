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
        public HttpResponseMessage GetMedia(int id)
        {
            int mediaId = id;
            string username = User.Identity.Name.Split('\\')[1];

            // TODO: SECURITY CHECK!!!
            Media media = _repository.Context.Media.Include("MediaStorage").Where(m => m.Id == mediaId).Single();

            string uploadDir = HttpContext.Current.Server.MapPath("~/App_Data");
            String localFileName = media.MediaStorage.LocalFileName;
            String path = uploadDir + "\\" + localFileName;
            FileStream fs = new FileStream(path, FileMode.Open);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(fs);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(media.MimeType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
            response.Content.Headers.ContentDisposition.FileName = media.FileName;
            response.Content.Headers.ContentDisposition.CreationDate = media.CreatedTime;
            response.Content.Headers.ContentDisposition.ModificationDate = media.ModifiedTime;
            return response;
        }

        // POST api/media/5
        [HttpPost]
        //public async Task<IEnumerable<Item>> Post(int id)
        public async Task PostMedia(int id)
        {
            int itemId = id;

            string username = User.Identity.Name.Split('\\')[1];
            User user = _repository.Context.Users.Find(username);

            // Verify we have a multipart payload
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string uploadDir = HttpContext.Current.Server.MapPath("~/App_Data");
            MultipartFormDataEncryptedStreamProvider provider = new MultipartFormDataEncryptedStreamProvider(uploadDir);

            try
            {
                Item item = _repository.Context.Items.Find(itemId);

                // Only the user who created the Item can attach media to it
                if (item.CreatedByUsername != username)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }

                if (null == item)
                {
                    throw new UnauthorizedAccessException("Access denied");
                }

                // Find the specified folder, create a new Item record, and attach it to the folder
                //Folder folder = _repository.Context.Folders.Find(folderId);
                //Item item = _repository.Context.Items.Add(new Item());
                //if ( (null == folder) || (null == item) )
                //{
                //    throw new UnauthorizedAccessException("Access denied");
                //}
                //else
                //{
                //    item.Type = "File";  // TODO: push this property from Item down to Media

                //    // It's really stupid that I have to supply values for these, because
                //    // _repository won't trust them anyway.
                //    item.CreatedTime = DateTime.UtcNow;
                //    item.ModifiedTime = DateTime.UtcNow;
                //    item.CreatedBy = user;
                    
                //    folder.Items.Add(item);
                //}


                // Wait for form data to finish loading
                await Request.Content.ReadAsMultipartAsync(provider);

                // Process file data
                foreach (var file in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);

                    // Create new Media record, attach it to its parent Item, and set media metadata
                    Media media = _repository.Context.Media.Add(new Media());
                    //media.Item = item;
                    item.Media.Add(media);

                    //media.FileName = fileInfo.Name;
                    media.FileName = (null == file.Headers.ContentDisposition.FileName) ?
                        fileInfo.Name : UnquoteToken(file.Headers.ContentDisposition.FileName);
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
                    //mediaStorage.LocalFileName = file.LocalFileName;
                    mediaStorage.LocalFileName = fileInfo.Name;
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

                //return item.Id;

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

        /// <summary>
        /// Remove bounding quotes on a token if present
        /// </summary>
        /// <param name="token">Token to unquote.</param>
        /// <returns>Unquoted token.</returns>
        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }


    }
}
