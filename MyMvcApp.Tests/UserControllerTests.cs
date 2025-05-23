using xunit;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using System.Linq;

namespace MyMvcApp.Tests
{
    public class UserControllerTests
    {
        public UserControllerTests()
        {
            // Clear the static userlist before each test
            UserController.userlist.Clear();
        }

        [Fact]
        public void Index_ReturnsViewWithUserList()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<System.Collections.Generic.List<User>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Details_ReturnsViewWithUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 2, Name = "User2", Email = "user2@email.com" };
            UserController.userlist.Add(user);
            var controller = new UserController();

            // Act
            var result = controller.Details(2) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<User>(result.Model);
            Assert.Equal("User2", model.Name);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Post_AddsUserAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Name = "NewUser", Email = "new@email.com" };
            controller.ModelState.Clear();

            // Act
            var result = controller.Create(user) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Single(UserController.userlist);
            Assert.Equal("NewUser", UserController.userlist[0].Name);
        }

        [Fact]
        public void Edit_Get_ReturnsViewWithUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 3, Name = "EditUser", Email = "edit@email.com" };
            UserController.userlist.Add(user);
            var controller = new UserController();

            // Act
            var result = controller.Edit(3) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<User>(result.Model);
            Assert.Equal("EditUser", model.Name);
        }

        [Fact]
        public void Edit_Get_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Edit(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_UpdatesUserAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            var user = new User { Id = 4, Name = "OldName", Email = "old@email.com" };
            UserController.userlist.Add(user);
            var controller = new UserController();
            var updatedUser = new User { Name = "NewName", Email = "new@email.com" };
            controller.ModelState.Clear();

            // Act
            var result = controller.Edit(4, updatedUser) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("NewName", UserController.userlist.First(u => u.Id == 4).Name);
        }

        [Fact]
        public void Edit_Post_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();
            var updatedUser = new User { Name = "DoesNotExist", Email = "none@email.com" };

            // Act
            var result = controller.Edit(999, updatedUser);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Get_ReturnsViewWithUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 5, Name = "DeleteUser", Email = "delete@email.com" };
            UserController.userlist.Add(user);
            var controller = new UserController();

            // Act
            var result = controller.Delete(5) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<User>(result.Model);
            Assert.Equal("DeleteUser", model.Name);
        }

        [Fact]
        public void Delete_Get_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Post_RemovesUserAndRedirects_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 6, Name = "ToDelete", Email = "todelete@email.com" };
            UserController.userlist.Add(user);
            var controller = new UserController();
            var form = new Microsoft.AspNetCore.Http.FormCollection(new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

            // Act
            var result = controller.Delete(6, form) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.DoesNotContain(UserController.userlist, u => u.Id == 6);
        }

        [Fact]
        public void Delete_Post_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();
            var form = new Microsoft.AspNetCore.Http.FormCollection(new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

            // Act
            var result = controller.Delete(999, form);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Search_ReturnsMatchingUsers_WhenQueryMatchesNameOrEmail()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Alice", Email = "alice@email.com" });
            UserController.userlist.Add(new User { Id = 2, Name = "Bob", Email = "bob@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Search("alice") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<System.Collections.Generic.List<User>>(result.Model);
            Assert.Single(model);
            Assert.Equal("Alice", model[0].Name);
        }

        [Fact]
        public void Search_ReturnsEmptyList_WhenNoMatch()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Alice", Email = "alice@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Search("notfound") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<System.Collections.Generic.List<User>>(result.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Search_ReturnsAll_WhenQueryIsEmpty()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Alice", Email = "alice@email.com" });
            UserController.userlist.Add(new User { Id = 2, Name = "Bob", Email = "bob@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Search("") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<System.Collections.Generic.List<User>>(result.Model);
            Assert.Empty(model); // Should return empty list for empty query
        }
    }
}