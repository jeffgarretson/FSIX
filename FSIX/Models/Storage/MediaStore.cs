using FSIX.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FSIX.Models.Storage
{
    public class MediaStore
    {
        // Use the same data repository the REST API uses
        private readonly FSIXRepository _repository;
        private readonly int _itemId;

        public MediaStore(IPrincipal User, int ItemId)
        {
            _repository = new FSIXRepository(User);
            // Verify user has write permission to item; otherwise throw security exception
            _itemId = ItemId;
        }

        //private ICollection<Media> _media;

        public async Task<HttpResponseMessage> SaveMedia(HttpRequestMessage Request)
        {
            // if (!_repository.VerifyItemWritePermission(itemid)) { throw security exception }

            string uploadDir = HttpContext.Current.Server.MapPath("~/App_Data");
            MultipartFormDataEncryptedStreamProvider provider = new MultipartFormDataEncryptedStreamProvider(uploadDir);

            try
            {
                //HttpResponseMessage response = new HttpResponseMessage();
                StringBuilder sb = new StringBuilder();  // Holds the response body

                // We *COULD* accept a Note field (and exclude itemid from the URI) and create the Item here,
                // saving an HTTP request. Return value would be the newly-created ItemId, which the client
                // could then fetch and refresh the view.

                await Request.Content.ReadAsMultipartAsync(provider);

                // Process file data
                foreach (var file in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));

                    // Create new Media record to save media details to DB
                    Media media = _repository.Context.Media.Add(new Media());
                    media.FileName = fileInfo.Name;
                    media.MimeType = file.Headers.ContentType.ToString();
                    media.Bytes = fileInfo.Length;

                    // Attach new media record to its parent item
                    Item item = _repository.Context.Items.Find(_itemId);
                    if (null == item) {
                        // throw security exception
                    } else {
                        media.Item = item;
                    }

                    // Create new MediaStorage record to save media storage/encryption details to DB
                    MediaStorage mediaStorage = _repository.Context.MediaStorage.Add(new MediaStorage());
                    mediaStorage.LocalFileName = file.LocalFileName;
                    // TODO: get the initialization vector from the stream provider
                    mediaStorage.IV = Enumerable.Repeat((byte)0x00, 32).ToArray();

                    // Attach MediaStorage record to Media record
                    media.MediaStorage = mediaStorage;

                    // Save changes to the database
                    _repository.Context.SaveChanges();
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(sb.ToString())
                };
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

        }
    }
}
