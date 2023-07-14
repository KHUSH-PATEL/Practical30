using Microsoft.AspNetCore.Mvc;
using Moq;
using Practical_30.Modal;
using Practical_30.Repositories;
using static Practical_30.Controllers.EmployeeController;

namespace NUnitTesting
{
    public class Tests
    {
        EmployeesController _employeesController;
        Mock<IEmployeeRepositorie> _employeeRepository;

        [SetUp]
        public void SetUp()
        {
            //Arrange
            _employeeRepository = new Mock<IEmployeeRepositorie>();
            _employeesController = new EmployeesController(_employeeRepository.Object);
        }

        /// <summary>
        /// Get Method, When Called, Should Returns Ok
        /// </summary>
        [Test]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            //Act
            var result = _employeesController.Get();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        /// <summary>
        /// Get Method, When Called, Should Returns Ok, with All Items
        /// </summary>
        [Test]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            //Arrange
            var employeesList = new List<Employee>()
            {
                new Employee{Id = 1, Name = "Khush Patel", Email = "khush@gmail.com", Address = "GJ-3"},
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

            Assert.That(employeesList.Count, Is.EqualTo(items?.Count));
        }

        [Test]
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
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

       
        [Test]
        public void GetById_ExistingGuidPassed_ReturnsItem()
        {
            //Arrange
            var existingID = 1;
            var employee = new Employee { Id = 1, Name = "Khush Patel", Email = "mk@gmail.com", Address = "Rajkot" };

            _employeeRepository
            .Setup(x => x.GetById(existingID))
            .Returns(employee);

            //Act
            var result = _employeesController.Get(existingID);

            //Assert
            Assert.IsInstanceOf<Employee>((result as OkObjectResult)?.Value);
            Assert.That(existingID, Is.EqualTo(((result as OkObjectResult)?.Value as Employee)?.Id));
        }

     
        [Test]
        public void Post_InvalidEmployeePassed_ReturnsBadRequestResult()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Email = "MK@gmail.com", Address = "Rajkot" };
            _employeesController.ModelState.AddModelError(nameof(Employee.Name), "Name is required");

            //Act
            var result = _employeesController.Post(employee);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

     
        [Test]
        public void Post_ValidEmployeePassed_ReturnsCreatedResult()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Name = "Khush Makadiya", Email = "mk@gmail.com", Address = "Rajkot" };
            _employeeRepository
            .Setup(x => x.Add(employee))
            .Returns(employee);

            //Act
            var result = _employeesController.Post(employee);

            //Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public void Post_ValidEmployeePassed_ReturnesCreatedResultWithEmployee()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Name = "Khush Makadiya", Email = "mk@gmail.com", Address = "Rajkot" };
            _employeeRepository
            .Setup(x => x.Add(employee))
            .Returns(employee);

            //Act
            var createdResponse = _employeesController.Post(employee) as CreatedAtActionResult;
            var item = createdResponse?.Value as Employee;

            //Assert
            Assert.IsInstanceOf<Employee>(item);
            Assert.That("Khush Makadiya", Is.EqualTo(item?.Name));
        }

        
        [Test]
        public void Delete_NotExistingGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingId = 7;

            //Act
            var badResponse = _employeesController.Delete(notExistingId);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(badResponse);
        }

        [Test]
        public void Delete_ExistingGuidPassed_ReturnsNoContentResult()
        {
            //Arrange
            Employee employee = new Employee { Id = 1, Name = "Khush Makadiya", Email = "mk@gmail.com", Address = "Rajkot" };

            _employeeRepository
            .Setup(x => x.GetById(employee.Id))
            .Returns(employee);

            _employeeRepository
            .Setup(x => x.Remove(employee));

            //Act
            var result = _employeesController.Delete(employee.Id);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}