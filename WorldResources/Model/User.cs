using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldResources
{
    [Serializable]
    public class User
    {
        
        private string name;
        private string pass;
        
        public User(string n, string p)
        {
            name = n;
            pass = p;
        }

        public string getName()
        {
            return name;
        }

        public string getPass()
        {
            return pass;
        }

        public void setUser(string n)
        {
            name = n;
        }
        public void setPW(string PW)
        {
            pass = PW;
        }
    }
}
