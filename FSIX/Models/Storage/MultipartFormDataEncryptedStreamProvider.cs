using FSIX.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace FSIX.Models.Storage
{
    public class MultipartFormDataEncryptedStreamProvider : MultipartFormDataStreamProvider
    {
        //private NameValueCollection _formData = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

        //private Collection<bool> _isFormData = new Collection<bool>();

        //private Collection<MultipartFileData> _fileData = new Collection<MultipartFileData>();

        //public NameValueCollection FormData
        //{
        //    get { return _formData; }
        //}

        //public Collection<MultipartFileData> FileData
        //{
        //    get { return _fileData; }
        //}

        public MultipartFormDataEncryptedStreamProvider(string rootPath)
            : base(rootPath)
        {
        }

        public MultipartFormDataEncryptedStreamProvider(string rootPath, int bufferSize)
            : base(rootPath, bufferSize)
        {
        }

        /// <summary>
        /// This body part stream provider examines the headers provided by the MIME multipart parser
        /// and decides whether it should return a file stream or a memory stream for the body part to be 
        /// written to.
        /// </summary>
        /// <param name="parent">The parent MIME multipart HttpContent instance.</param>
        /// <param name="headers">Header fields describing the body part</param>
        /// <returns>The <see cref="Stream"/> instance where the message body part is written to.</returns>
        //public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        //{
        //    //if (parent == null)
        //    //{
        //    //    throw Error.ArgumentNull("parent");
        //    //}

        //    //if (headers == null)
        //    //{
        //    //    throw Error.ArgumentNull("headers");
        //    //}

        //    // For form data, Content-Disposition header is a requirement
        //    ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
        //    if (contentDisposition != null)
        //    {
        //        // If we have a file name then write contents out to temporary file. Otherwise just write to MemoryStream
        //        if (!String.IsNullOrEmpty(contentDisposition.FileName))
        //        {
        //            // We won't post process files as form data
        //            _isFormData.Add(false);

        //            return base.GetStream(parent, headers);
        //        }

        //        // We will post process this as form data
        //        _isFormData.Add(true);

        //        // If no filename parameter was found in the Content-Disposition header then return a memory stream.
        //        return new MemoryStream();
        //    }

        //    // If no Content-Disposition header was present.
        //    throw Error.InvalidOperation(Properties.Resources.MultipartFormDataStreamProviderNoContentDisposition, "Content-Disposition");
        //}

        // private _IV
        // public IV {
        //   get { return _IV; }
        // }

        // Set _IV

    }
}
