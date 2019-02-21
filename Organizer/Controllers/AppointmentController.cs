using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Organizer.Controllers
{
    public class AppointmentController : Controller
    {
        private OrganizerDbContext db = new OrganizerDbContext();
        // GET: Appointment
        public ActionResult ListAppointment()
        {
            List<Appointment> appointments = db.Appointments
                .Include(a => a.Broker)
                .Include(a => a.Customer)
                .ToList();
            return View(appointments);
        }
        public ActionResult AddAppointment()
        {
            ViewBag.BrokerID = new SelectList(db.Brokers, "BrokerID", "FullName");
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FullName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppointment([Bind(Include = "AppointmentID, DateHour, BrokerID, CustomerID, Subject")] Appointment appointment)
        {
            var queryResult = db.Appointments.SingleOrDefault(a => (a.BrokerID == appointment.BrokerID && a.DateHour == appointment.DateHour) || (a.CustomerID == appointment.CustomerID && a.DateHour == appointment.DateHour));
            if (queryResult != null)
            {
                ModelState.AddModelError("DateHour", "le rendez vous est déjà pris");
            }
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrokerID = new SelectList(db.Brokers, "BrokerID", "FullName");
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FullName");

            return View();
        }
    }
}
