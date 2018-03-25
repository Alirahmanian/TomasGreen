using System;
using Xunit;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TomasGreen.Web.Models;
using System.Reflection;
using TomasGreen.Web.Areas.Import.ViewModels;

namespace TomasGreen.Test
{
    public class TestSystemUser
    {
        private readonly ApplicationDbContext _context;
        public TestSystemUser(ApplicationDbContext context)
        {
            _context = context;
        }
        [Fact]
        public void TestUserType()
        {
            // Arrange
            var userType = new UserType { ID =1, Name = "Staff", DbTableName="Staffs" };
            var systemUser = new SystemUser { UserId = 1, UserType = userType };
            // Act
            
            //Assert
            Assert.Equal(systemUser.UserType.DbTableName, userType.DbTableName);
        }

        [Fact]
        public void TestGUID()
        {
            // Arrange
            var g = Guid.NewGuid();
            
            // Act

            //Assert
            Assert.NotEmpty(g.ToString());
        }
        [Fact]
        public void TestDbContext()
        {
            // Arrange
           

            // Act

            //Assert
           
        }

    }
}
