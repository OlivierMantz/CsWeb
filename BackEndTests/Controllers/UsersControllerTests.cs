//using BackEnd.Controllers;
//using BackEnd.Data;
//using BackEnd.Models.DTOs;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Xunit;

//namespace BackEndTests.Controllers
//{
//    public class UsersControllerTests
//    {
//        private readonly UserService _userService;
//        private readonly UserContext _userContext;
//        public UsersControllerTests()
//        {
//            var options = new DbContextOptionsBuilder<UserContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;

//            var userContext = new UserContext(options);
//            userContext.Database.EnsureDeleted();
//            userContext.Database.EnsureCreated();

//            _userService = new UserService(new UserRepository(userContext));
//        }

//        // GET
//        [Fact()]
//        public async Task Get_AllUsers()
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Act
//            var result = await controller.GetUsers();
//            var expectedUsers = await _userService.GetUsersAsync();
//            // Assert
//            var okResult = result.Result as OkObjectResult;
//            Assert.NotNull(okResult);

//            var userDtos = okResult.Value as List<UserDTO>;
//            Assert.NotNull(userDtos);

//            Assert.Equal(expectedUsers.Count(), userDtos.Count);
//            Assert.Equal(expectedUsers[0].Name, userDtos[0].Name);
//            Assert.Equal(expectedUsers[1].Name, userDtos[1].Name);
//            Assert.Equal(expectedUsers[2].Name, userDtos[2].Name);
//        }

//        // GET {id}
//        [Theory]
//        [InlineData(1)]
//        [InlineData(2)]
//        [InlineData(3)]
//        public async Task Get_OneUser(long id)
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Act
//            var result = (await controller.GetUser(id)).Result as OkObjectResult;
//            var expectedUser = await _userService.GetUserByIdAsync(id);
//            // Assert
//            var userDto = (UserDTO)result.Value;
//            Assert.NotNull(userDto);

//            Assert.Equal(expectedUser.Name, userDto.Name);
//            Assert.Equal(expectedUser.Email, userDto.Email);
//            Assert.Equal(expectedUser.Password, userDto.Password);
//        }


//        [Theory]
//        [InlineData(-1)]
//        public async Task Get_OneUser_InvalidInputId(long id)
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Arrange
//            if (id < 0)
//            {
//                // Act and Assert
//                var result = await controller.GetUser(id);
//                Assert.IsType<BadRequestObjectResult>(result.Result);
//            }
//        }

//        [Theory]
//        [InlineData(4)]
//        public async Task Get_OneUser_UserNotFound(long id)
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Act and Assert
//            var result = await controller.GetUser(id);
//            Assert.IsType<NotFoundObjectResult>(result.Result);
//        }

//        // GET {name}
//        [Theory()]
//        [InlineData("John")]
//        public async Task Get_UserByName(string name)
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Act
//            var result = (await controller.GetUserByName(name)).Result as OkObjectResult;
//            // Assert
//            var okResult = (UserDTO)result.Value;
//            Assert.NotNull(okResult);
//            Assert.Equal(okResult.Name, okResult.Name);
//        }

//        [Theory]
//        [InlineData("")]
//        public async Task Get_UserByName_InvalidInputName(string name)
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Act
//            var result = await controller.GetUserByName(name);
//            // Assert
//            var badRequestResult = result.Result as BadRequestObjectResult;
//            Assert.NotNull(badRequestResult);
//            Assert.Equal(400, badRequestResult.StatusCode);
//            Assert.Equal("Invalid name parameter. The name must not be null or empty.", badRequestResult.Value);
//        }

//        [Theory]
//        [InlineData("Blorg")]
//        public async Task Get_UserByName_NotFound(string name)
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Act and Assert
//            var result = await controller.GetUserByName(name);
//            Assert.IsType<NotFoundResult>(result.Result);
//        }

//        // PUT {id}
//        [Theory()]
//        [InlineData(1)]
//        public async Task Put_User(long id)
//        {
//            UserDTO userDto = new UserDTO { Id = 1, Name = "JohnDoe", Email = "JohnDoe@gmail.com", Password = "5678" };
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Act
//            var result = await controller.PutUser(id, userDto);
//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }

//        [Theory()]
//        [InlineData(4)]
//        [InlineData(null)]
//        public async Task Put_User_InvalidInputId(long id)
//        {
//            // Arrange
//            UserDTO userDto = new UserDTO { Id = 1, Name = "JohnDoe", Email = "JohnDoe@gmail.com", Password = "5678" };
//            var controller = new UsersController(_userService);
//            // Act
//            var result = await controller.PutUser(id, userDto);
//            // Assert
//            Assert.IsType<BadRequestObjectResult>(result);
//        }

//        [Theory()]
//        [InlineData(5)]
//        public async Task Put_User_NotFound(long id)
//        {
//            // Arrange
//            UserDTO userDtoDoesntExist = new UserDTO
//            { Id = 5, Name = "JohnDoe", Email = "JohnDoe@gmail.com", Password = "5678" };
//            var controller = new UsersController(_userService);
//            // Act
//            var result = await controller.PutUser(id, userDtoDoesntExist);
//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }

//        // POST
//        [Fact()]
//        public async Task Post_User()
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            var newUserDto = new UserDTO
//            {
//                Name = "JohnDoe",
//                Email = "johndoe@gmail.com",
//                Password = "password",
//            };
//            // Act
//            var result = await controller.PostUser(newUserDto);
//            // Assert
//            Assert.IsType<CreatedAtActionResult>(result.Result);
//            var createdResult = result.Result as CreatedAtActionResult;
//            Assert.Equal(201, createdResult.StatusCode);
//            Assert.Equal(nameof(UsersController.GetUser), createdResult.ActionName);
//            Assert.Equal(newUserDto.Name, (createdResult.Value as UserDTO)?.Name);
//        }

//        [Fact()]
//        public async Task Post_User_InvalidInput()
//        {
//            // Arrange
//            var invalidUserDto = new UserDTO
//            {
//                Name = "John",
//                Email = null,
//                Password = "1234"
//            };
//            var controller = new UsersController(_userService);
//            // Act
//            var result = await controller.PostUser(invalidUserDto);
//            // Assert
//            Assert.IsType<ObjectResult>(result.Result);
//            // var objectResult = (ObjectResult)result.Result;
//            // Assert.Equal(500, objectResult.StatusCode);
//            // Assert.IsInstanceOfType(objectResult.Value, typeof(ProblemDetails));
//            // var problemDetails = (ProblemDetails)objectResult.Value;
//            // Assert.Equal("One or more invalid inputs", problemDetails.Detail);
//            var objectResult = result.Result as ObjectResult;
//            Assert.Equal(500, objectResult.StatusCode);
//            Assert.IsType<ProblemDetails>(objectResult.Value);
//            var problemDetails = objectResult.Value as ProblemDetails;
//            Assert.Equal("One or more invalid inputs", problemDetails?.Detail);
//        }

//        // DELETE {id}
//        [Theory]
//        [InlineData(1)]
//        public async Task Delete_User(long id)
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Act
//            var result = await controller.DeleteUser(id);
//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }

//        [Theory]
//        [InlineData(4)]
//        public async Task Delete_User_InvalidInputId(long id)
//        {
//            // Arrange
//            var controller = new UsersController(_userService);
//            // Act
//            var result = await controller.DeleteUser(id);
//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }

//        [Theory]
//        [InlineData(1)]
//        public async Task Delete_User_NotFound(long id)
//        {
//            // Arrange
//            var controller = new UsersController(_userService);

//            // Act
//            var result = await controller.DeleteUser(id);

//            // Assert
//            Assert.IsType<NoContentResult>(result);

//            var deletedUser = await _userService.GetUserByIdAsync(id);
//            Assert.Null(deletedUser);
//        }
//    }

//    // UserExists
//    // CheckInputInvalid
//    // CheckIsValidEmail
//}