using Microsoft.AspNetCore.Identity;
using System.Drawing;
using System.Collections;
using ShopWebCustomer.Models;
namespace ShopWebCustomer.Services
{
    public interface ICommon
    {
        //Task<string> UploadedFile(IFormFile ProfilePicture);
        string GetSHA256(string str);

        //void SendEmail(DataUser request);
        //string GenerateToken();
    }
}
