using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test0000001.DB;
using test0000001.Models;
using test0000001.Repository.InterfaceClass;
using test0000001.Repository.ServiceClass;

namespace test0000001.Controllers
{
    public class MotorInsuranceController : Controller
    {
        private IInsuranceCategory insurCateSer;
        private IPolicy policyService;
        private IMotorInsurance motorInsSer;

        private DatabaseContext dbContext;

        private readonly UserManager<ApplicationUser> userManager;
        public MotorInsuranceController
            (IPolicy _policyService, 
            DatabaseContext _dbContext, 
            IMotorInsurance _motorInsSer, 
            IInsuranceCategory _insurCateSer, 
            UserManager<ApplicationUser> _userManager)
        {
            insurCateSer = _insurCateSer;
            policyService = _policyService;
            dbContext = _dbContext;
            motorInsSer = _motorInsSer;
            userManager = _userManager;
        }



        // GET: Home/MotorInsurance
        public async Task<IActionResult> Index()
        {
            //Lấy tất cả car theo user id
            var user = await GetUser();
            var car = await dbContext.CarInsuredObject!
            .Where(p => p.UserId == user.Id) // So sánh với user.Id mà không cần ép kiểu
                .ToListAsync();

            return View(car);
        }

        // GET: Home/MotorInsurance
        [HttpGet]
        public IActionResult TakeInforCar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TakeInforCar(CarInsuredObject newCar)
        {
            if (newCar.YearsOfManufacture == DateTime.MinValue)
            {
                ModelState.AddModelError("YearsOfManufacture", "Years of Manufacture is required.");
            }

            if (string.IsNullOrEmpty(newCar.Automaker))
            {
                ModelState.AddModelError("Automaker", "Automaker is required.");
            }

            if (string.IsNullOrEmpty(newCar.CarBand))
            {
                ModelState.AddModelError("CarBand", "Car Band is required.");
            }

            if (string.IsNullOrEmpty(newCar.CarType))
            {
                ModelState.AddModelError("CarType", "Car Type is required.");
            }

            if (string.IsNullOrEmpty(newCar.CityOfCarReg))
            {
                ModelState.AddModelError("CityOfCarReg", "City of Car Registration is required.");
            }
            if (!ModelState.IsValid)
            {
                // If ModelState is not valid, redisplay the form with validation errors
                return View(newCar);
            }

            var user = await GetUser();
            newCar.UserId = user.Id; 
            await dbContext.CarInsuredObject!.AddAsync(newCar);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "MotorInsurance");
        }

        [HttpGet]
        public async Task<IActionResult> CreateHolder()
        {
            try
            {
                var insurCate = await insurCateSer.GetCategoryById(4);
                var policies = await dbContext.Policy.Include(t => t.Duration).Where(p => p.InsuranceCategoryId == insurCate!.Id).ToListAsync();
                ViewBag.Policies = policies;

                var user = await GetUser();
                var carInsuredObjects = await dbContext.CarInsuredObject!.Where(p => p.UserId == user.Id).ToListAsync();
                ViewBag.CarInsuredObjects = carInsuredObjects;
                ViewBag.UserId = user.Id;
                return View();





            }
            catch (Exception ex)
            {
                // In inner exception để xem lỗi cụ thể
                ViewBag.MsgError = ex.InnerException?.Message;
                return RedirectToAction("Index", "MotorInsurance");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateHolder(Policyholder policyHolder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Xác minh rằng dữ liệu là hợp lệ trước khi lưu vào cơ sở dữ liệu
                    await dbContext.Policyholder.AddAsync(policyHolder);
                    await dbContext.SaveChangesAsync();

                    // Lưu dữ liệu thành công, chuyển hướng hoặc thực hiện các hành động khác ở đây
                    return RedirectToAction("Index", "MotorInsurance");
                }
                else
                {
                    // Trả về View với ModelState không hợp lệ để hiển thị thông báo lỗi
                    var insurCate = await insurCateSer.GetCategoryById(4);
                    var policies = await dbContext.Policy.Include(t => t.Duration).Where(p => p.InsuranceCategoryId == insurCate!.Id).ToListAsync();
                    ViewBag.Policies = policies;

                    var user = await GetUser();
                    var carInsuredObjects = await dbContext.CarInsuredObject!.Where(p => p.UserId == user.Id).ToListAsync();
                    ViewBag.CarInsuredObjects = carInsuredObjects;

                    ModelState.AddModelError(string.Empty, "There are errors in the form. Please correct them.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                // In inner exception để xem lỗi cụ thể
                ViewBag.MsgError = ex.InnerException?.Message;
                return View();
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateHolder(Policyholder policyHolder)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // Kiểm tra xem cặp userId và policyId đã tồn tại trong Policyholder chưa
        //            var existingPolicyHolder = await dbContext.Policyholder
        //                .FirstOrDefaultAsync(ph => ph.UserId == policyHolder.UserId && ph.PolicyId == policyHolder.PolicyId);

        //            if (existingPolicyHolder != null)
        //            {
        //                // Cặp userId và policyId đã tồn tại, trả về thông báo đã đăng ký rồi
        //                ModelState.AddModelError(string.Empty, "This user has already registered for this policy.");
        //                return View();
        //            }

        //            // Nếu cặp userId và policyId chưa tồn tại, thêm dữ liệu mới
        //            await dbContext.Policyholder.AddAsync(policyHolder);
        //            int affectedRecords = await dbContext.SaveChangesAsync();

        //            if (affectedRecords > 0)
        //            {
        //                // Dữ liệu đã được lưu thành công, chuyển hướng hoặc thực hiện các hành động khác ở đây
        //                return RedirectToAction("Index", "MotorInsurance");
        //            }
        //            else
        //            {
        //                // Có lỗi khi thêm dữ liệu, có thể xử lý lỗi ở đây hoặc trả về View với thông báo lỗi
        //                ModelState.AddModelError(string.Empty, "There was an error while saving data. Please try again.");
        //            }
        //        }
        //        else
        //        {
        //            // Trả về View với ModelState không hợp lệ để hiển thị thông báo lỗi
        //            ModelState.AddModelError(string.Empty, "There are errors in the form. Please correct them.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // In inner exception để xem lỗi cụ thể
        //        ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
        //    }

        //    // Nếu bạn đến đây, có nghĩa có lỗi xảy ra. Trả về View với thông báo lỗi hoặc xử lý lỗi ở đây nếu cần.
        //    return View();
        //}





        //protected async Task<ApplicationUser> GetUser()
        //{
        //    string username = User.Identity!.Name!.ToString();
        //    ApplicationUser user = await userManager.FindByNameAsync(username);
        //    return user;
        //}

        protected async Task<ApplicationUser> GetUser()
        {
            if (User.Identity != null && User.Identity.Name != null)
            {
                string username = User.Identity.Name;
                ApplicationUser user = await userManager.FindByNameAsync(username);
                return user;
            }
            else
            {
                // Xử lý khi User.Identity hoặc User.Identity.Name là null
                // Có thể trả về null hoặc thông báo lỗi tùy thuộc vào logic của bạn.
                return null;
            }
        }

    }
}
