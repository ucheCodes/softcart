using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Cryptography;  
using System.Text; 
using dbreezeDbApi.Database;
using DBreeze;

namespace dbreezeDbApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaystackController : ControllerBase
{
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World, welcome to Peter's Soft Network Payment System with Paystack");
        }
        [HttpPost]
        public IActionResult EncryptString(Data data)//(string key, string plainText)  
        {  
            string key = "b14ca5898a4e4133bbce2ea2315a1916";//this can be changed anytime
            string plainText = data.SerializedValue;
            byte[] iv = new byte[16]; 
            byte[] array;

            using (Aes aes = Aes.Create())  
            {  
                aes.Key = Encoding.UTF8.GetBytes(key);  
                aes.IV = iv;  
  
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);  
  
                using (MemoryStream memoryStream = new MemoryStream())  
                {  
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))  
                    {  
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))  
                        {  
                            streamWriter.Write(plainText);  
                        }  
  
                        array = memoryStream.ToArray();  
                    }  
                }  
            } 
            
            if (database.Create(data.TableName, data.SerializedKey, Convert.ToBase64String(array)))
            {
                return Ok("encrypted string has been saved to database"); 
            }
            else
            {
                return Ok("converted string failed to add to database");
            }
        }  

        [HttpGet("{id}")]
        public IActionResult DecryptString(string id)  
        { 
            string key = "b14ca5898a4e4133bbce2ea2315a1916";//this key was used during encryption
            string[] data = id.Split("_");
            string table = data[0];
	    string _key = data[1]; //this key was used to store the encrypted text into db
            var cipherText = database.Read(table,_key).Value;
            byte[] iv = new byte[16];  
            byte[] buffer = Convert.FromBase64String(cipherText);  
            using (Aes aes = Aes.Create())  
            {  
                aes.Key = Encoding.UTF8.GetBytes(key);  
                aes.IV = iv;  
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);  
  
                using (MemoryStream memoryStream = new MemoryStream(buffer))  
                {  
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))  
                    {  
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))  
                        {  
                            return Ok(streamReader.ReadToEnd());  
                        }  
                    }  
                }  
            }
        }  

}
