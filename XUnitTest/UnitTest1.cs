using Microsoft.AspNetCore.Mvc;
using Practical_30.Modal;
using Practical_30.Repositories;
using static Practical_30.Controllers.EmployeeController;

namespace XUnitTest
{
    public class UnitTest1
    {
        EmployeesController _employeesController;
        IEmployeeRepositorie _employeeRepository;

        public UnitTest1()
        {
            //Arrange
            _employeeRepository = new EmployeeRepositories();
            _employeesController = new EmployeesController(_employeeRepository);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            //Act
            var result = _employeesController.Get();

            //Assert
            Assert.IsType<OkObjectResult>(result as OkObjectResult);
        }

       
        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            //Act
            var result = _employeesController.Get();

            //Assert
            var items = Assert.IsType<List<Employee>>((result as OkObjectResult)?.Value);
            Assert.Equal(_employeeRepository.GetAll().Count(), items.Count);
        }

      
     
        [Fact]
        public void GetById_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var existingId =1;

            //Act
            var result = _employeesController.Get(existingId);

            //Assert
            Assert.IsType<OkObjectResult>(result as OkObjectResult);
        }
        [Fact]
        public void GetById_ExistingGuidPassed_ReturnsItem()
        {
            //Arrange
            var existingId = 1;

            //Act
            var result = _employeesController.Get(existingId);

            //Assert
            Assert.IsType<Employee>((result as OkObjectResult)?.Value);
            Assert.Equal(existingId, ((result as OkObjectResult)?.Value as Employee)?.Id);
        }

        [Fact]
        public void Post_InvalidEmployeePassed_ReturnsBadRequestResult()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Email = "Khush@gmail.com", Address = "GJ-3" };
            _employeesController.ModelState.AddModelError(nameof(Employee.Name), "Name is required");

            //Act
            var result = _employeesController.Post(employee);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Post_ValidEmployeePassed_ReturnsCreatedResult()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Name = "Khush Patel", Email = "MK@gmail.com", Address = "GJ-3" };

            //Act
            var result = _employeesController.Post(employee);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Post_ValidEmployeePassed_ReturnesCreatedResultWithEmployee()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Name = "Khush Makadiya", Email = "khush@gmail.com", Address = "rajkot" };

            //Act
            var createdResponse = _employeesController.Post(employee) as CreatedAtActionResult;
            var item = createdResponse?.Value as Employee;

            //Assert
            Assert.IsType<Employee>(item);
            Assert.Equal("Khush Makadiya", item.Name);
        }

        [Fact]
        public void Delete_NotExistingGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingId = 7;

            //Act
            var badResponse = _employeesController.Delete(notExistingId);

            //Assert
            Assert.IsType<NotFoundResult>(badResponse);
        }

        [Fact]
        public void Delete_ExistingGuidPassed_ReturnsNoContentResult()
        {
            //Arrange
            var existingId = 1;

            //Act
            var result = _employeesController.Delete(existingId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        
        [Fact]
        public void Delete_ExistingGuidPassed_RemovesOneItem()
        {
            //Arrange
            var existingId = 1;
            var itemCountBeforeDelete = _employeeRepository.GetAll().Count();

            //Act
            _employeesController.Delete(existingId);

            //Assert
            Assert.Equal(itemCountBeforeDelete - 1, _employeeRepository.GetAll().Count());
        }
    }
}