using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Data
{
    public static class Dependencies
    {
        public static bool CheckRelatedRecords(ApplicationDbContext _context, string tableName, string foreignKeyName, int id)
        {
          
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "Select OBJECT_NAME(parent_object_Id) from sys.foreign_keys where object_name(referenced_object_id) = '" + tableName + "'";
                _context.Database.OpenConnection();
                var tables = command.ExecuteReader();
                foreach (var table in tables)
                {
                   var counter= _context.Set<Article>().FromSql("use TomasGreen; select " + foreignKeyName + " from " + table + " where " + foreignKeyName + " = " + id + "").Count();
                    if (counter > 0)
                        return true;
                    //using (var innerCommand = _context.Database.GetDbConnection().CreateCommand())
                    //{
                    //    innerCommand.CommandText = "use TomasGreen; select " + foreignKeyName + " from " + table + " where " + foreignKeyName + " = " + id + "";
                    //    var related = innerCommand.ExecuteReader(); //_context.Set<BaseEntity>().FromSql().Count();//.FirstOrDefault();
                    //    if (related != null)
                    //        return true;
                    //}
                   

                }
                
                    
            };
           
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
