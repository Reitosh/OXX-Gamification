using System;
using Xunit;
using OXXGame.Controllers;
using OXXGame.Models;
using Moq;
using Microsoft.AspNetCore.Http;
using Autofac.Extras.Moq;

namespace OXXGame.test
{
    public class UnitTest1
    {

        private OXXGameDBContext dbContext;

        [Fact]
        public void TestSSH()
        {
            throw new NotImplementedException("Not implemented");
        }

        
        /*public void Index_shouldReturnFalse()
        {

            // Arrange
            int expectedFalse = 0;

            // Actual
            int actual;
            if (LoginController.loggedIn() == false)
            {

            }
            // Assert
            Assert.Equal(expectedFalse, actual);
            throw new NotImplementedException("Not implemented");
        }*/
        
        [Fact]
        public void isAdmin_shouldNotBeAdmin()
        {   
            throw new NotImplementedException("Not implemented");
        }

        [Theory]
        [InlineData("hest@best.no")]
        public void checkIfExist_shouldExist(string uname)
        {
            throw new NotImplementedException("Not implemented");
        }

        [Fact]
        public void allUsers_ValidCall()
        {
            throw new NotImplementedException("Not implemented");
        }


        // Denne testen skal feile fordi det ikke er mulig å legge inn en ny bruker med spesifikk userId.
        // Skal senere endres til å sjekke om metoden kaster en exception. 
        [Theory]
        [InlineData("Andreas", "Reitan", "andreas.reitan@oxx.no", "kebab123")]
        public void addUser_ShouldNotBeSuccessful(string firstName, string lastName, string eMail, string passWord)
        {
            DB db = new DB(null);

            User expected = new User
            {
                firstname = firstName,
                lastname = lastName,
                email = eMail,
                password = passWord,
                userId = 0
            };

            var actual = db.addUser(expected);

            Assert.Equal(expected.userId, actual.userId);
            Assert.Equal(expected.firstname, actual.firstname);
            Assert.Equal(expected.lastname, actual.lastname);
            Assert.Equal(expected.email, actual.email);
            Assert.Equal(expected.password, actual.password);
        }


        [Theory]
        [InlineData("Pappa", "iSjappa", "pappa@monaco.no", "phenger")]
        public void addUser_ShouldBeSuccessful(string firstName, string lastName, string eMail, string passWord)
        {
            DB db = new DB(null);

            User expected = new User
            {
                firstname = firstName,
                lastname = lastName,
                email = eMail,
                password = passWord
            };

            var actual = db.addUser(expected);

            Assert.Equal(expected.firstname, actual.firstname);
            Assert.Equal(expected.lastname, actual.lastname);
            Assert.Equal(expected.email, actual.email);
            Assert.Equal(expected.password, actual.password);
        }

    }
}
