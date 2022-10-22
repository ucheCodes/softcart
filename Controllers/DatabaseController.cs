using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using dbreezeDbApi.Database;
using DBreeze;

namespace dbreezeDbApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        [HttpPut]
        public JsonResult Exists (Data data)
        {
            if (database.Exists(data.TableName, data.SerializedKey))
            {
                return new JsonResult(true);
            }
            return new JsonResult(false);
        }
        [HttpPost]
        public JsonResult CreateUpdate (Data data)
        {
            try
            {
                if (!string.IsNullOrEmpty(data.TableName) && !string.IsNullOrEmpty(data.SerializedKey))
                {
                    if (database.Create(data.TableName, data.SerializedKey, data.SerializedValue))
                    {
                        return new JsonResult("Data Added successfully!");
                    }
                    else
                    {
                        return new JsonResult("Operation Failed. JSON data needs stringify");
                    }
                }
                else
                {
                    return new JsonResult("TableName and / or serialized key cannot be null or empty.");
                }
            }
            catch (System.Exception e)
            {
                return new JsonResult(e.Message);
            }
        }
        [HttpPost]
        [Route("read")]
        public JsonResult Read(Data info)
        {
            var data = database.Read(info.TableName, info.SerializedKey);
            if (data.Key != "")
            {
                return new JsonResult(data);
            }
            else
            {
                return new JsonResult("data not found, ensure the key passed in is serialized with JSON.Parse(key)");
            }
        }
        [HttpGet("{tablename}")]
        public JsonResult ReadAll(string TableName)
        {
            var data = database.ReadAll(TableName);
            if (data.Count > 0)
            {
                return new JsonResult(data);
            }
            else
            {
                return new JsonResult("data not found in the Tablename passed and / or table is empty, ensure correct data is passed");
            }
        }
        [HttpDelete("{id}")]
        public JsonResult DeleteAll (string id)
        {
            if (database.DeleteAll(id,true))
            {
                return new JsonResult(true);
            }
            else
            {
                return new JsonResult(false);
            }
        }
        [HttpPut]
        [Route("delete")]
        public JsonResult Delete (Data data)
        {
            if (database.Delete(data.TableName, data.SerializedKey))
            {
                return new JsonResult("Delete operation is successful");
            }
            else
            {
                return new JsonResult("Delete operation failed.");
            }
        }
    }
}