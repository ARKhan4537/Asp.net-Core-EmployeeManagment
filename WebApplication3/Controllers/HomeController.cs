using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.Security;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{

    //[Authorize]
    //[AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment;
        private readonly ILogger logger;
        private readonly IDataProtector protector;

        public HomeController(EmployeeRepository employeeRepository, 
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment,
            ILogger<HomeController> logger,
                              IDataProtectionProvider dataProtectionProvider,DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);


        }
        [AllowAnonymous]
        public  IActionResult DefaultPage(DefaultPageViewModel model)
        {

            return View();
        }
        public ViewResult Index()
        {

            var model = _employeeRepository.GetAllEmployee()
                .Select(e =>
                            {
                                // Encrypt the ID value and store in EncryptedId property
                                e.EncryptedId = protector.Protect(e.id.ToString());
                                return e;
                            });
            return View(model);
         
        }

        public ViewResult Details(string id)
        {
            //  throw new Exception("Errors in Details View");
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogError("Warning Log");
            logger.LogCritical("Critical Log");

          int employeeId =Convert.ToInt32( protector.Unprotect(id));
            

            Employee employee = _employeeRepository.GetEmployee(employeeId);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound",employeeId);
            }
            
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };

            //Employee model = _employeeRepository.GetEmployee(1);
            // ViewBag.PageTitle = "Employee Details";
            return View(homeDetailsViewModel);
           }
        [HttpGet]
        [Authorize(Roles = "Admin,Super Admin")]
        //[Authorize(Roles = "Super Admin")]
        //[Authorize]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        //[Authorize]
        public ViewResult Edit(int id)
        {

            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = employee.id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath

            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.PhotoPath != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath= Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUplodedFile(model);
                }
               
                _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUplodedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.PhotoPath != null && model.PhotoPath.Count > 0)
            {
                foreach (IFormFile photo in model.PhotoPath)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var filestream =new FileStream(filePath, FileMode.Create)) 
                    {

                        photo.CopyTo(filestream);
                    }
                        
                }
            }

            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUplodedFile(model);
                Employee NewEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(NewEmployee);
                return RedirectToAction("details", new { id = NewEmployee.id });
            }
            return View();
        }
    }
    }