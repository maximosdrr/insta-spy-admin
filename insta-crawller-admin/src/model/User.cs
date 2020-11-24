using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insta_crawller_admin.src.model
{
    class User
    {
        public bool authorized;
        public string email;
        public string name;
        public string id;
        public string password;

        public User(DocumentSnapshot doc)
        {
            if(doc != null)
            {
                this.id = doc.Id;
                this.authorized = doc.GetValue<bool>("authorized");
                this.email = doc.GetValue<string>("email");
                this.name = doc.GetValue<string>("name");
                this.password = doc.GetValue<string>("password");
            }
        }
    }
}
