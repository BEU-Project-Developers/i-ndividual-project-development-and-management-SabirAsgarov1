using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Address_Book_SDF
{
    internal class Connection
    {
        // connects database
        public string MyConnectionDB()
        {
            string conn = "Data Source=SABIR\\SABIRASGAROV;Initial Catalog=AddressBookDB;Integrated Security=True;";
            return conn;
        }
    }
}
