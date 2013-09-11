using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace FSIX.Web.Util
{
    public class MultipartFormDataEncryptedStreamProvider : MultipartFormDataStreamProvider
    {
        // private _IV
        // public IV {
        //   get { return _IV; }
        // }

        // Set _IV
        public MultipartFormDataEncryptedStreamProvider(string rootPath)
            : base(rootPath) {}
    }
}
