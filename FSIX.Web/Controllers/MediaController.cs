using FSIX.Models;
using FSIX.Web.Util;
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

        // GET api/media/5
        //[HttpGet]
        //public HttpResponseMessage Get(int id)
        //{
        //    var data = from f in _repository.Context.Media
        //               where f.Id == id
        //               select f;
        //    Media media = (Media)data.Single();
        //    MemoryStream ms = new MemoryStream(item.Content);
        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    response.Content = new StreamContent(ms);
        //    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(media.MimeType);
        //    response.Content.Headers.ContentDisposition.FileName = media.FileName;
        //    response.Content.Headers.ContentDisposition.CreationDate = media.CreatedTime;
        //    response.Content.Headers.ContentDisposition.ModificationDate = media.ModifiedTime;
        //    return response;
        //}

        // POST api/media/PostFile
        [HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string uploadDir = HttpContext.Current.Server.MapPath("~/App_Data");
            MultipartFormDataEncryptedStreamProvider provider = new MultipartFormDataEncryptedStreamProvider(uploadDir);

            try
            {
                //HttpResponseMessage response = new HttpResponseMessage();
                StringBuilder sb = new StringBuilder();  // Holds the response body

                await Request.Content.ReadAsMultipartAsync(provider);

                // Q: Why not just pass a Collection<HttpContent> to the repository?
                // A: Because understanding HTTP is not the repository's job. This is a Web API controller; do it here.
                //return _repository.AddMedia(provider.Contents);

                // Process form data
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        sb.Append(string.Format("{0}: {1}\n", key, val));
                    }
                }

                // Process file data
                foreach (var file in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                }

                //provider.Contents.Select(
                //    async (media) =>
                //    {
                //        //ContentDispositionHeaderValue disposition = media.Headers.ContentDisposition;
                //        //string FileName = (disposition != null && disposition.FileName != null) ? disposition.FileName : String.Empty;
                //        //string MimeType = media.Headers.ContentType.ToString();
                //        //byte[] Content = await media.ReadAsByteArrayAsync();
                //        //int FileSize = Content.Length;

                //        await _repository.AddMediaAsync(media);

                //        //mediaAttributes = new
                //        //{
                //        //    FileName = FileName,
                //        //    MimeType = MimeType,
                //        //    FileSize = FileSize,
                //        //    Content = Content,
                //        //};
                //    });

                ////_repository.AddMedia(mediaAttributes);
                
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
