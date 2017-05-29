using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WorldResources
{
    class RegisterControl
    {
        private RegWindow rw;
        private User user;
        private List<User> userList;
        private FileStream stream = null;
        private BinaryFormatter formater = new BinaryFormatter();
        private bool success = false;
        private readonly string path;
        public RegisterControl(RegWindow regw)
        {
            rw = regw;
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "usersList");
        }

        public void openData()
        {

            user = new User(rw.getUser(), rw.getPass());

            if (File.Exists(path))
            {
                try
                {
                    stream = File.Open(path, FileMode.OpenOrCreate);
                    userList = (List<User>)formater.Deserialize(stream);
                    if (checkUser())
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                catch
                {
                    //
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                }
            }
            else
            {
                userList = new List<User>();
                success = true;
                createUser();
            }

        }

        public void createUser()
        {
            userList.Add(user);
            if(stream != null)
            {
                stream.Dispose();
            }

            try
            {
                stream = File.OpenWrite(path);
                formater.Serialize(stream, userList);
            }
            catch
            {

            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        public bool checkUser()
        {
            foreach (User usr in userList)
            {
                if (usr.getName().Equals(user.getName()))
                {
                    rw.userExists();                //ako korisnik postoji, nista
                    return false;
                }
            }
            createUser();
            return true;
        }
        public bool getSuccess()
        {
            return success;
        }
    }
}
