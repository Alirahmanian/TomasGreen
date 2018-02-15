using System;
using Xunit;
using TomasGreen.Model.Models;

namespace TomasGreen.Test
{
    public class TestSystemUser
    {
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

    }
}
