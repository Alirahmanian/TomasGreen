using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Data
{
    public class Dependencies
    {
        public bool CheckRelatedRecords(ApplicationDbContext _context, string tableName, string foreignKeyName, int id)
        {
            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "Select OBJECT_NAME(parent_object_Id) from sys.foreign_keys where object_name(referenced_object_id) = '" + tableName + "'";
            _context.Database.OpenConnection();
            var tables = command.ExecuteReader();

            foreach (var table in tables)
            {
                var related = _context.Set<Article>().FromSql("select " + foreignKeyName + " from " + table + " where " + foreignKeyName + " = " + id + "").Count();//.FirstOrDefault();
                if (related > 0)
                    return true;

            }
            //var tables = _context.Database.SqlQuery<string>("Select OBJECT_NAME(parent_object_Id) from sys.foreign_keys where object_name(referenced_object_id) = '" + tableName + "'").ToList();
            //foreach (var table in tables)
            //{
            //    var related = db.Database.SqlQuery<int>("select " + foreignKeyName + " from " + table + " where " + foreignKeyName + " = " + id + "").Count();//.FirstOrDefault();
            //    if (related > 0)
            //        return true;

            //}
            return false;

        }
    }
}
