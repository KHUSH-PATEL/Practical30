using Microsoft.AspNetCore.Mvc;
using Moq;
using Practical_30.Modal;
using Practical_30.Repositories;
using static Practical_30.Controllers.EmployeeController;

namespace MsUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        EmployeesController _employeesController;
        Mock<IEmployeeRepositorie> _employeeRepository;

        [TestInitialize]
        public void SetUp()
        {
            //Arrange
            _employeeRepository = new Mock<IEmployeeRepositorie>();
            _employeesController = new EmployeesController(_employeeRepository.Object);
        }

        [TestMethod]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            //Act
            var result = _employeesController.Get();

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

      
        [TestMethod]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            //Arrange
            var employeesList = new List<Employee>()
            {
            new Employee{Id = 1, Name = "Khush Patel", Email = "MK@gmail.com", Address = "Rajkot"},
            new Employee{Id = 2, Name = "Karan Raj", Email = "karan@gmail.com", Address = "Vadodra"},
            new Employee{Id = 3, Name = "Jainam Bhavsar", Email = "Jainam@gmail.com", Address = "Nadiad"},
            new Employee{Id = 4, Name = "Khush Makadiya", Email = "Khush@gmail.com", Address = "Rajkot"},
            new Employee{Id = 5, Name = "Jimit Patel", Email = "Jimit@gmail.com", Address = "Anand"},
            };

            _employeeRepository
            .Setup(x => x.GetAll())
            .Returns(employeesList);

            //Act
            var result = _employeesController.Get();

            //Assert
            var items = ((result as OkObjectResult)?.Value) as List<Employee>;

            Assert.AreEqual(employeesList.Count, items?.Count);
        }

     
        [TestMethod]
        public void GetById_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var existingId = 1;
            var employee = new Employee { Id = 1, Name = "Khush Patel", Email = "MK@gmail.com", Address = "Rajkot" };

            _employeeRepository
            .Setup(x => x.GetById(existingId))
            .Returns(employee);

            //Act
            var result = _employeesController.Get(existingId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

       
        [TestMethod]
        public void GetById_ExistingGuidPassed_ReturnsItem()
        {
            //Arrange
            var existingId = 1;
            var employee = new Employee { Id = 1, Name = "Khush Patel", Email = "MK@gmail.com", Address = "Rajkot" };

            _employeeRepository
            .Setup(x => x.GetById(existingId))
            .Returns(employee);

            //Act
            var result = _employeesController.Get(existingId);

            //Assert
            Assert.IsInstanceOfType((result as OkObjectResult)?.Value, typeof(Employee));
            Assert.AreEqual(existingId, ((result as OkObjectResult)?.Value as Employee)?.Id);
        }

      
        [TestMethod]
        public void Post_InvalidEmployeePassed_ReturnsBadRequestResult()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Email = "MK@gmail.com", Address = "Rajkot" };
            _employeesController.ModelState.AddModelError(nameof(Employee.Name), "Name is required");

            //Act
            var result = _employeesController.Post(employee);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Post_ValidEmployeePassed_ReturnsCreatedResult()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Name = "Khush Patel", Email = "MK@gmail.com", Address = "Rajkot" };
            _employeeRepository
            .Setup(x => x.Add(employee))
            .Returns(employee);

            //Act
            var result = _employeesController.Post(employee);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void Post_ValidEmployeePassed_ReturnesCreatedResultWithEmployee()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Name = "Khush Patel", Email = "MK@gmail.com", Address = "Rajkot" };
            _employeeRepository
            .Setup(x => x.Add(employee))
            .Returns(employee);

            //Act
            var createdResponse = _employeesController.Post(employee) as CreatedAtActionResult;
            var item = createdResponse?.Value as Employee;

            //Assert
            Assert.IsInstanceOfType(item, typeof(Employee));
            Assert.AreEqual("Khush Patel", item?.Name);
        }

    
        [TestMethod]
        public void Delete_NotExistingGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingId =7;

            //Act
            var badResponse = _employeesController.Delete(notExistingId);

            //Assert
            Assert.IsInstanceOfType(badResponse, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Delete_ExistingGuidPassed_ReturnsNoContentResult()
        {
            //Arrange
            Employee employee = new Employee { Id =1, Name = "Khush Patel", Email = "MK@gmail.com", Address = "Rajkot" };

            _employeeRepository
            .Setup(x => x.GetById(employee.Id))
            .Returns(employee);

            _employeeRepository
            .Setup(x => x.Remove(employee));

            //Act
            var result = _employeesController.Delete(employee.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
