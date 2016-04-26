using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using drop.Application;
using drop.Desktop.Models;

namespace Drop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RegistryDrop registry = new RegistryDrop();
            List<User> users = new List<User>();
            users.Add(new User("Juan Carlos"));
            users.Add(new User("Marco Antonio"));
            users.Add(new User("Pedro Carlos"));
            users.Add(new User("Marco Antonio"));
            users.Add(new User("Juan Carlos"));
            users.Add(new User("Martin Antonio"));
            users.Add(new User("Gaza Carlos"));
            users.Add(new User("Jose Antonio"));
            registry.addContextMenuHandler(users);

        }
    }
}
