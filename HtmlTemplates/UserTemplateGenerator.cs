using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RKdigitalsAPI.Controllers;
using RKdigitalsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKdigitalsAPI.HtmlTemplates
{
    public class UserTemplateGenerator
    {
        private readonly RKdigitalsDBContext _db;
        public UserTemplateGenerator()
        {
            _db = new RKdigitalsDBContext();
        }
        public List<User> GetUsers()
        {
            return  _db.Users.ToList();
        }
        public static string GetUserHtmlStirng()
        {
            UserTemplateGenerator userTemplateGenerator = new UserTemplateGenerator();
            List<User> userlist = userTemplateGenerator.GetUsers();
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>User's List</h1></div>
                                <table align='center'>
                                    <tr>
                                         <th>Id</th>
                                        <th>Fullname</th>
                                        <th>Username</th>
                                        <th>Email</th>
                                        <th>Mobilenumber</th>
                                        <th>Role</th>
                                    </tr>");
            foreach (var item in userlist)
            {
                int i = 1;
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                </tr>",
                                i=i+1,
                                item.Fullname,
                                item.Username,
                                item.Email,
                                item.Mobilenumber,
                                item.Role);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
