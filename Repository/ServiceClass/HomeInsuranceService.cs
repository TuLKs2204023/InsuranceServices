using Microsoft.EntityFrameworkCore;
using test0000001.DB;
using test0000001.Models;
using test0000001.Repository.InterfaceClass;

namespace test0000001.Repository.ServiceClass
{
    public class HomeInsuranceService : IHomeInsurance
    {
        private DatabaseContext db;

        public HomeInsuranceService(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<bool> addHomeInsurance(Home_Insurance newHomeInsurance)
        {
            db.Add(newHomeInsurance);
            await db.SaveChangesAsync();
            return true;
        }
        //ViewBag.product = newHomeInsurance;

        public async Task<bool> deleteHomeInsurance(int id)
        {
            var homeInsurance = await db.Home_Insurance!.SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (homeInsurance != null)
            {
                db.Home_Insurance!.Remove(homeInsurance);
                db.SaveChangesAsync();
                return true;
            }
            else { return false; }
        }

        public async Task<bool> editHomeInsurance(Home_Insurance editHomeInsurance)
        {
            var homeInsurance = await db.Home_Insurance!.SingleOrDefaultAsync(x => x.Id.Equals(editHomeInsurance.Id));
            if (homeInsurance != null)
            {
                homeInsurance.Name = editHomeInsurance.Name;
                homeInsurance.Address = editHomeInsurance.Address;
                homeInsurance.Gmail = editHomeInsurance.Gmail;
                homeInsurance.Structure = editHomeInsurance.Structure;
                homeInsurance.NumberOfBasement = editHomeInsurance.NumberOfBasement;
                homeInsurance.YearBuilt = editHomeInsurance.YearBuilt;
                homeInsurance.Area = editHomeInsurance.Area;
                homeInsurance.Duration = editHomeInsurance.Duration;
                homeInsurance.LinkDriver = editHomeInsurance.LinkDriver;
                /*
                //Upload image
                string fileName = Path.GetFileName(file.FileName);
                string file_path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Images/", fileName);
                using (var stream = new FileStream(file_path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                editHomeInsurance.Photo = "Images/" + fileName;
                //----------------------------
                db.Add(country);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                */
                db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<Home_Insurance>> getAllHomeInsurance()
        {
            return await db.Home_Insurance!.Include(e => e.Duration).ToListAsync();
        }

        public async Task<Home_Insurance> getHomeInsurance(int id)
        {
            var homeInsurance = db.Home_Insurance!.SingleOrDefault(x => x.Id.Equals(id));
            if (homeInsurance != null)
            {
                return homeInsurance;
            }
            else
            {
                return null!;
            }
        }


    }
}
