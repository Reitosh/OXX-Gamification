using System;
using Xunit;
using OXXGame.Controllers;
using OXXGame.Models;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;

namespace OXXGame.test
{
    public class UnitTest1
    {
        OXXGameDBContext dBContext;

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
        [InlineData(0, "Andreas", "Reitan", "andreas.reitan@oxx.no", "kebab123")]
        public void addUser_ShouldNotBeSuccessful(int id, string firstName, string lastName, string eMail, string passWord)
        {

            DB db = new DB(null);

            User expected = new User
            {
                userId = id,
                firstname = firstName,
                lastname = lastName,
                email = eMail,
                password = passWord
            };

            var actual = db.addUser(expected);

            Assert.Equal(expected.userId, actual.userId);
            Assert.Equal(expected.firstname, actual.firstname);
            Assert.Equal(expected.lastname, actual.lastname);
            Assert.Equal(expected.email, actual.email);
            Assert.Equal(expected.password, actual.password);
        }

        // Metoden er ikke komplett. 
        [Theory]
        [InlineData("Pappa", "iSjappa", "pappa@monaco.no", "phenger", 1, false, false, false, false, false, false, false, false, false, false, false)]
        public void addUser_ShouldBeSuccessful(string firstName, string lastName, string eMail, string passWord, int loginCount, bool yeAdmin, bool knoHtml, bool knoCss, bool knoJavscript, bool knoCsharp, bool knoMvc, bool knoNetFramework, bool knoTypescript, bool knoVue, bool knoReact, bool knoAngular)
        {
            User user = new User
            {
                firstname = firstName,
                lastname = lastName,
                email = eMail,
                password = passWord,
                loginCounter = loginCount,
                isAdmin = yeAdmin,
                knowHtml = knoHtml,
                knowCss = knoCss,
                knowJavascript = knoJavscript,
                knowCsharp = knoCsharp,
                knowMvc = knoMvc,
                knowNetframework = knoNetFramework,
                knowTypescript = knoTypescript,
                knowVue = knoVue,
                knowReact = knoReact,
                knowAngular = knoAngular
            };

            var mock = new Mock<DB>();
            mock.Setup(x => x.addUser(user)).Returns(user);

            var userObject = new DB(mock.Object);
            var retrnData = userObject.addUser(user);
        }

        [Fact]
        public async Task TestController_TestInfo_ShouldReturnResult()
        {
            // Arrange
            var controller = new TestController(dBContext);

            // Act
            var result = await controller.TestInfo(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);

        }
    }
}
