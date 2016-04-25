using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop.lib
{
    /// <summary>
    /// Represents a file request of an outgoing transmission.
    /// </summary>
    class OutgoingFileRequest
    {
        /// <summary>
        /// Id of the file
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
        /// The filename of the file on the current system
        /// </summary>
        public string filename;

        /// <summary>
        /// If true, the file has been accepted by the server
        /// </summary>
        public bool accepted;
    }
}
