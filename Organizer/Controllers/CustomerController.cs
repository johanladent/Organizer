﻿using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Organizer.Controllers
{
    public class CustomerController : Controller
    {
        private OrganizerDbContext db = new OrganizerDbContext();
        // GET: Customer
        public ActionResult ListCustomers(string search)
        {
            var customerQ = from c in db.Customers select c;
            if (!String.IsNullOrEmpty(search))
            {
                customerQ = customerQ.Where(c => c.Firstname.Contains(search) || c.Lastname.Contains(search));
            }
            List<Customer> customers = customerQ.ToList();
            return View(customers);
        }
        // GET: Customer
        public ActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer([Bind(Include = "Firstname, Lastname, Mail, PhoneNumber, Budget")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("ListCustomers");
            }
            return View();
        }
        public ActionResult ProfilCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer([Bind(Include = "CustomerID, Firstname, Lastname, Mail, PhoneNumber, Budget")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListCustomers");
            }
            return View("ProfilCustomer");
        }
        public ActionResult DeleteCustomer(int id)
        {
            Customer CustomerDelete = db.Customers.Find(id);
            db.Entry(CustomerDelete).State = EntityState.Deleted;
            db.Customers.Remove(CustomerDelete);
            db.SaveChanges();
            return RedirectToAction("ListCustomers");
        }
    }
}