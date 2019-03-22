using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models.SuperAdmin;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImprovementProjectWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ImprovementProjectWebApp.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuperAdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult AddMultiExercise()
        {
            ViewData["BodyPartId"] = new SelectList(_context.BodyPart, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMultiExercise(MutilExercise mutilExercise)
        {
            List<string> Exs = new List<string>();
            string Exercises = mutilExercise.Exercises;
            Exercises = Exercises.Replace("\r\n", " ");
            string[] sArray = Exercises.Split(" ");
            foreach(var item in sArray)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    if (!_context.Exercise.Any(e => e.Name == item))
                    {
                        Exercise exercise = new Exercise();
                        exercise.Name = item;
                        exercise.BodyPartId = mutilExercise.BodyPartId;
                        _context.Exercise.Add(exercise);
                    }
                }
            }
            _context.SaveChanges();
            ViewData["BodyPartId"] = new SelectList(_context.BodyPart, "Id", "Name");
            return View();

        }
    }
}