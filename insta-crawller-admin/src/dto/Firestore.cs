using Google.Cloud.Firestore;
using insta_crawller_admin.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace insta_crawller_admin.src.dto
{
    class Firestore
    {
        public static FirestoreDb getInstance()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"insta-api-scrapper-firebase-adminsdk-jx8z6-c56105c9f5.json";
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                FirestoreDb db = FirestoreDb.Create("insta-api-scrapper");
                return db;
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
                return null;
            }
            
        }

        public async static Task<List<User>> getUsers(FirestoreDb db)
        {
            try
            {
                List<User> users = new List<User>();
                var snapshot = await db.Collection("users").GetSnapshotAsync();
                foreach (var doc in snapshot)
                {
                    User user = new User(doc);
                    users.Add(user);
                }
                return users;
            }
            catch
            {
                return null;
            }
            
        }

        public async static Task<bool> updateUserCredential(string userId, bool authorized, FirestoreDb db)
        {
            try
            {
                var userDocument = db.Collection("users").Document(userId);
                var userData = await userDocument.GetSnapshotAsync();
                var user = new User(userData);
                user.authorized = authorized;
                if(userDocument != null)
                {
                    Dictionary<string, object> userDict = new Dictionary<string, object>();
                    userDict.Add("authorized", user.authorized);
                    userDict.Add("email", user.email);
                    userDict.Add("password", user.password);
                    userDict.Add("name", user.name);
                    await userDocument.SetAsync(userDict);
                    return true;
                }
                return false;
            }
            catch(Exception err)
            {
                return false;
            }
        }
    }
}
