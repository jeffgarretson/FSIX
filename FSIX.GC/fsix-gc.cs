using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSIX.GC
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connect to database

            // Read configuration
            
            // Delete all expired media and items
            
            // Delete "orphaned" files (>5min old and no corresponding Media record)
            // (theoretically this shouldn't happen because we creating a MediaInternalData record
            // to store the IV for encryption implicitly creates a Media record and happens before
            // the file is actually created)
            
            // Mark expired folders as expired (don't delete because we still need to see the logs)
            
            // Retrun exit code
        }
    }
}
