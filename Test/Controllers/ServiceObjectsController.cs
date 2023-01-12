using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Test.Migrations;
using Test.Models;

namespace Test.Controllers
{
    public class ServiceObjectsController : Controller
    {
        private readonly SerDbContext _context;

        public ServiceObjectsController(SerDbContext context)
        {
            _context = context;
        }

        // GET: ServiceObjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.serviceObjects.ToListAsync());
        }

        //GET: READ
        public async Task<IActionResult> Read(string? id)
        {
            if (id == null || _context.serviceObjects == null)
            {
                return NotFound();
            }

            var serviceObject = await _context.serviceObjects
                .FirstOrDefaultAsync(m => m.ID == id);
            if (serviceObject == null)
            {
                return NotFound();
            }

            return View(serviceObject);
        }

        // GET: ServiceObjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceObjects/Create?{Name}&{Amount}
        [HttpPost]
        public async Task<IActionResult> Create(string name, int amount = 0)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SerDbContext.serviceObjects'  is null.");
            }

            try
            {
                if (name == null && amount < 0)
                    throw new ArgumentNullException("Bad value!");
            }
            catch (NullReferenceException e)
            {
                return BadRequest(e.Message);
                throw;
            }

            Guid g = Guid.NewGuid();

            var service = new ServiceObject();

            service.Name = name;
            service.Amount = amount;
            service.ID = g.ToString(); //GUID

            try
            {
                if (ServiceObjectExists(service.ID))
                    throw new Exception("Id already exists.");
                _context.Add(service);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return Json(service.ID);
        }

        // GET: ServiceObjects/Update/{id}
        public async Task<IActionResult> Update(string? id)
        {
            if (id == null || _context.serviceObjects == null)
            {
                return NotFound();
            }

            var serviceObject = await _context.serviceObjects.FindAsync(id);
            if (serviceObject == null)
            {
                return NotFound();
            }
            return View(serviceObject);
        }


        //POST services/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(string id, string? name, int amount = 0)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SerDbContext.serviceObjects'  is null.");
            }

            try
            {
                if (name == null && amount < 0)
                    throw new ArgumentNullException("Bad value!");
            }
            catch (NullReferenceException e)
            {
                return BadRequest(e.Message);
                throw;
            }

            var serviceObject = await _context.serviceObjects
                .FirstOrDefaultAsync(m => m.ID == id);

            serviceObject.Amount = amount;

            try
            {
                _context.Update(serviceObject);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceObjectExists(serviceObject.ID))
                {
                    throw new ArgumentException("Not found ID");
                }
                else
                {
                    throw;
                }
            }
            return Json(serviceObject.ID);
        }

        //POST service/booking
        public async Task<IActionResult> Booking(string id, int amount)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SerDbContext.serviceObjects'  is null.");
            }

            var orders = new Order();
            bool state = false;

            //витягиваем объект с БД
            var serviceObject = await _context.serviceObjects
                .FirstOrDefaultAsync(m => m.ID == id);
            if (serviceObject == null)
                return NotFound();

            //Присваиваем значения для сохранения в бд
            orders.NameOrder = serviceObject.Name;
            orders.ID = serviceObject.ID;
            orders.AmountOrder = amount;

            //При положительном Amount
            if (amount >= 0)
            {
                if (serviceObject.Amount - amount >= 0)
                    serviceObject.Amount = serviceObject.Amount - amount;

                else
                    return Json("Error: Balance exceeded!");
            }
            //При отрицательном Amount => value + |value|
            else
                serviceObject.Amount = serviceObject.Amount + Math.Abs(amount);
            try
            {
                //Обновляем показатель Amount для услуги
                _context.Update(serviceObject);
                //Сохраняем заказ в БД
                _context.Add(orders);
                await _context.SaveChangesAsync();
                state = true; //Joke
            }
            catch (DbUpdateConcurrencyException e)
            {
                //Если не найдено ID в БД
                if (!ServiceObjectExists(serviceObject.ID))
                {
                    throw new ArgumentException("Not found ID");
                }
                else
                {
                    return Problem(e.Message);
                    throw;
                }
            }
            //Немного не понял задание: нужно вернуть статус код или просто true в случае успешного оформления заказа?
            string json = JsonConvert.SerializeObject((State: state, Service: serviceObject), Formatting.Indented);
            return Ok(json);
        }

        // GET: ServiceObjects/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.serviceObjects == null)
            {
                return NotFound();
            }

            var serviceObject = await _context.serviceObjects
                .FirstOrDefaultAsync(m => m.ID == id);
            if (serviceObject == null)
            {
                return NotFound();
            }

            return View(serviceObject);
        }

        // POST: ServiceObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SerDbContext.serviceObjects'  is null.");
            }
            var serviceObject = await _context.serviceObjects.FindAsync(id);
            if (serviceObject != null)
            {
                _context.serviceObjects.Remove(serviceObject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Ищет совпадения по ID в БД
        private bool ServiceObjectExists(string id)
        {
            return _context.serviceObjects.Any(e => e.ID == id);
        }
    }
}
