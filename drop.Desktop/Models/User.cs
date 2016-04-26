using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drop.Desktop.Models
{
    public class User
    {
        private string _name { get; set; }
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        private string _ip { get; set; }
        public string ip
        {
            get
            {
                return this._ip;
            }
            set
            {
                this._ip = value;
            }
        }

        public User(string name)
        {
            this._name = name;
        }
        
        public User(string name,string ip)
        {
            this._name = name;
            this._ip = ip;
        }


    }
}
