using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop.lib
{
    /// <summary>
    /// Represents a file request of an incoming transmission.
    /// </summary>
    class IncomingFileRequest
    {
        /// <summary>
        /// The id of the file
        /// </summary>
        public UInt16 id;

        /// <summary>
        /// The file size
        /// </summary>
        public UInt64 size;

        /// <summary>
        /// The name of the file
        /// </summary>
        public string name;

        /// <summary>
        /// If true, the file has been accepted
        /// </summary>
        public bool accepted;
    }
}
