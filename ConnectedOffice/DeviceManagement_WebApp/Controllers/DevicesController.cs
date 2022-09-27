using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using DeviceManagement_WebApp.Repository;

namespace DeviceManagement_WebApp.Controllers
{
    public class DevicesController : Controller
    {
        private readonly ConnectedOfficeContext _context;
        private readonly IDeviceRepository _deviceRepository;

        public DevicesController(ConnectedOfficeContext context, IDeviceRepository deviceRepository)
        {
            _context = context;
            _deviceRepository = deviceRepository;
        }

        // GET: Devices
        public async Task<IActionResult> Index()
        {
            return View(await _deviceRepository.GetAll());
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var device = await _deviceRepository.FindById(id);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            ViewData["ZoneId"] = new SelectList(_context.Zone, "ZoneId", "ZoneName");
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
        {
            device.DeviceId = Guid.NewGuid();
            
            _deviceRepository.Add(device);

            return RedirectToAction(nameof(Index));

        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var device = await _deviceRepository.FindById(id);

            if (device == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", device.CategoryId);
            ViewData["ZoneId"] = new SelectList(_context.Zone, "ZoneId", "ZoneName", device.ZoneId);
            
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }
            try
            {
                _deviceRepository.Edit(device);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_deviceRepository.FindById(device.DeviceId) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var device = await _deviceRepository.FindById(id);

            if (device == null) return NotFound();

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var device = await _deviceRepository.FindById(id);

            if (device == null) return NotFound();

            try
            {
                _deviceRepository.Remove(device);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_deviceRepository.FindById(device.CategoryId) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
