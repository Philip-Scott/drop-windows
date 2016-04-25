using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop.lib
{
    /// <summary>
    /// This class provides access to the configuration of the drop daemon.
    /// </summary>
    class Settings
    {
        /// <summary>
        /// The name of the server that's displayed to the user. Leave this empty to use the real user name.
        /// </summary>
        public string server_name;

        /// <summary>
        /// Sets wether the server is enabled and should receive transmissions.
        /// </summary>
        public bool server_enabled;

        /// <summary>
        /// Creates a new Settings interface.
        /// </summary>
        public Settings()
        {
            
        }

        /// <summary>
        /// Returns the same value like server_name, but already checks, if it's empty and returns the real user name instead.
        /// </summary>
        /// <returns>server_name</returns>
        public string get_display_name()
        {
            return "";
        }
    }
}
