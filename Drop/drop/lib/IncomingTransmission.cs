using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop.lib
{
    class IncomingTransmission
    {
        public enum ServerState {
            AWAITING_INITIALISATION,
            SENDING_INITIALISATION,
            AWAITING_REQUEST,
            NEEDS_CONFIRMATION,
            SENDING_CONFIRMATION,
            REJECTED,
            RECEIVING_DATA,
            FINISHED,
            CANCELED,
            FAILURE
        }

        public struct FileRequest
        {
            //uint16 id;
            //uint64 size;
            string name;
            bool accepted;
        }
    }
}
