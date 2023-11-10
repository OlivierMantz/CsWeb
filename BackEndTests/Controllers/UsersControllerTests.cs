using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd.Controllers;
using BackEnd.Data;
using BackEnd.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Repositories;
using BackEnd.Services;
using Microsoft.EntityFrameworkCore;

namespace BackEndTests.Controllers
{
    [TestClass()]
    public class UsersControllerTests
    {
        private readonly UserService _userService;
        private readonly UserContext _userContext;
        public UsersControllerTests()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var userContext = new UserContext(options);
            userContext.Database.EnsureDeleted();
            userContext.Database.EnsureCreated();

            _userService = new UserService(new UserRepository(userContext));
        }

        // GET
        [TestMethod()]
        public async Task Get_AllUsers()
        {
            // Arrange
            var controller = new UsersController(_userService);
            // Act
            var result = await controller.GetUsers();
            var expectedUsers = await _userService.GetUsersAsync();
            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var userDtos = okResult.Value as List<UserDTO>;
            Assert.IsNotNull(userDtos);

            Assert.AreEqual(expectedUsers.Count(), userDtos.Count);
            Assert.AreEqual(expectedUsers[0].Name, userDtos[0].Name);
            Assert.AreEqual(expectedUsers[1].Name, userDtos[1].Name);
            Assert.AreEqual(expectedUsers[2].Name, userDtos[2].Name);
        }

        // GET {id}
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task Get_OneUser(long id)
        {
            // Arrange
            var controller = new UsersController(_userService);
            // Act
            var result = (await controller.GetUser(id)).Result as OkObjectResult;
            var expectedUser = await _userService.GetUserByIdAsync(id);
            // Assert
            var userDto = (UserDTO)result.Value;
            Assert.IsNotNull(userDto);

            Assert.AreEqual(expectedUser.Name, userDto.Name);
            Assert.AreEqual(expectedUser.Email, userDto.Email);
            Assert.AreEqual(expectedUser.Password, userDto.Password);
        }


        [TestMethod]
        [DataRow(-1)]
        public async Task Get_OneUser_InvalidInputId(long id)
        {
            // Arrange
            var controller = new UsersController(_userService);
            // Arrange
            if (id < 0)
            {
                // Act and Assert
                var result = await controller.GetUser(id);
                Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            }
        }

        [TestMethod]
        [DataRow(4)]
        public async Task Get_OneUser_UserNotFound(long id)
        {
            // Arrange
            var controller = new UsersController(_userService);
            // Act and Assert
            var result = await controller.GetUser(id);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        // GET {name}
        [TestMethod()]
        [DataRow("John")]
        public async Task Get_UserByName(string name)
        {
            // Arrange
            var controller = new UsersController(_userService);
            // Act
            var result = (await controller.GetUserByName(name)).Result as OkObjectResult;
            // Assert
            var okResult = (UserDTO)result.Value;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.Name, okResult.Name);
        }

        [TestMethod]
        [DataRow("")]
        public async Task Get_UserByName_InvalidInputName(string name)
        {
            // Arrange
            var controller = new UsersController(_userService);
            // Act
            var result = await controller.GetUserByName(name);
            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid name parameter. The name must not be null or empty.", badRequestResult.Value);
        }

        [TestMethod]
        [DataRow("Blorg")]
        public async Task Get_UserByName_NotFound(string name)
        {
            // Arrange
            var controller = new UsersController(_userService);
            // Act and Assert
            var result = await controller.GetUserByName(name);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // PUT {id}
        [TestMethod()]
        [DataRow(1)]
        public async Task Put_User(long id)
        {
            UserDTO userDto = new UserDTO { Id = 1, Name = "JohnDoe", Email = "JohnDoe@gmail.com", Password = "5678" };
            // Arrange
            var controller = new UsersController(_userService);
            // Act
            var result = await controller.PutUser(id, userDto);
            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod()]
        [DataRow(4)]
        [DataRow(null)]
        public async Task Put_User_InvalidInputId(long id)
        {
            // Arrange
            UserDTO userDto = new UserDTO { Id = 1, Name = "JohnDoe", Email = "JohnDoe@gmail.com", Password = "5678" };
            var controller = new UsersController(_userService);
            // Act
            var result = await controller.PutUser(id, userDto);
            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod()]
        [DataRow(5)]
        public async Task Put_User_NotFound(long id)
        {
            // Arrange
            UserDTO userDtoDoesntExist = new UserDTO
            { Id = 5, Name = "JohnDoe", Email = "JohnDoe@gmail.com", Password = "5678" };
            var controller = new UsersController(_userService);
            // Act
            var result = await controller.PutUser(id, userDtoDoesntExist);
            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // POST
        [TestMethod]
        public async Task Post_User()
        {
            // Arrange
            var controller = new UsersController(_userService);
            var newUserDto = new UserDTO
            {
                Name = "JohnDoe",
                Email = "johndoe@gmail.com",
                Password = "password",
            };
            // Act
            var result = await controller.PostUser(newUserDto);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(nameof(UsersController.GetUser), createdResult.ActionName);
            Assert.AreEqual(newUserDto.Name, (createdResult.Value as UserDTO)?.Name);
        }

        [TestMethod]
        public async Task Post_User_InvalidInput()
        {
            // Arrange
            var invalidUserDto = new UserDTO
            {
                Name = "John",
                Email = null,
                Password = "1234"
            };
            var controller = new UsersController(_userService);
            // Act
            var result = await controller.PostUser(invalidUserDto);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            // var objectResult = (ObjectResult)result.Result;
            // Assert.AreEqual(500, objectResult.StatusCode);
            // Assert.IsInstanceOfType(objectResult.Value, typeof(ProblemDetails));
            // var problemDetails = (ProblemDetails)objectResult.Value;
            // Assert.AreEqual("One or more invalid inputs", problemDetails.Detail);
            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.IsInstanceOfType(objectResult.Value, typeof(ProblemDetails));
            var problemDetails = objectResult.Value as ProblemDetails;
            Assert.AreEqual("One or more invalid inputs", problemDetails?.Detail);
        }

        // DELETE {id}
        [TestMethod]
        [DataRow(1)]
        public async Task Delete_User(long id)
        {
            // Arrange
            var controller = new UsersController(_userService);
            // Act
            var result = await controller.DeleteUser(id);
            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        [DataRow(4)]
        public async Task Delete_User_InvalidInputId(long id)
        {
            // Arrange
            var controller = new UsersController(_userService);
            // Act
            var result = await controller.DeleteUser(id);
            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        [DataRow(1)]
        public async Task Delete_User_NotFound(long id)
        {
            // Arrange
            var controller = new UsersController(_userService);

            // Act
            var result = await controller.DeleteUser(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            var deletedUser = await _userService.GetUserByIdAsync(id);
            Assert.IsNull(deletedUser);
        }
    }

    // UserExists
    // CheckInputInvalid
    // CheckIsValidEmail
}