 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneToManyMVC.Models;

namespace OneToManyMVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly OneToManyMVCContext _context;

        public StudentsController(OneToManyMVCContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString)
        {
            string searchGrdId = Request.Query.FirstOrDefault(x => x.Key == "[0].CurrentGradeID").Value;
            string searchGridId = Request.Query.FirstOrDefault(x => x.Key == "[0].CurrentAmbionID").Value;
            IQueryable<Grade> gradoner = from o in _context.Grade
                                         where o.GradeId == Int32.Parse(searchGrdId)
                                         select o;

            IQueryable<Ambion> amboner = from o in _context.Ambion
                                         where o.AmbionId == Int32.Parse(searchGridId)
                                         select o;

            IQueryable<Grade> allGrades = from grd in _context.Grade select grd;

            IQueryable<Ambion> allAmbions = from amb in _context.Ambion select amb;

            IQueryable<Student> studoner =
                  from s in _context.Student
                  from g in gradoner
                  where s.CurrentGradeID == g.GradeId
                  select new Student
                  {
                      Grade = g,
                      Id = s.Id,
                      StudentName = s.StudentName
                  };

            IQueryable<Student> studoner2 =
                  from s in _context.Student
                  from a in amboner
                  where s.CurrentAmbionID == a.AmbionId
                  select new Student
                  {
                      Ambion = a,
                      Id = s.Id,
                      StudentName = s.StudentName
                  };


            //join
            IQueryable<Student> studs =
                  from s in _context.Student
                  from g in _context.Grade
                  where s.CurrentGradeID == g.GradeId
                  select new Student
                  {
                      Grade = g,
                      Id = s.Id,
                      StudentName = s.StudentName
                  };
            IQueryable<Student> studs2 =
                  from s in _context.Student
                  from a in _context.Ambion
                  where s.CurrentAmbionID == a.AmbionId
                  select new Student
                  {
                      Ambion = a,
                      Id = s.Id,
                      StudentName = s.StudentName
                  };

            //Linq Join
            IQueryable<Student> students = _context.Student
                            .Join(_context.Grade, student => student.CurrentGradeID, grade => grade.GradeId,
                            (student, grade) => new Student
                            {
                                Id = student.Id,
                                StudentName = student.StudentName,
                                CurrentGradeID = grade.GradeId,
                                Grade = new Grade { GradeId = grade.GradeId, GradeName = grade.GradeName }
                            });
            ViewData["selectlist"] = new SelectList(await allGrades.Distinct().ToListAsync(), "GradeId", "GradeName");
            IQueryable<Student> students2 = _context.Student
                            .Join(_context.Ambion, student => student.CurrentAmbionID, ambion => ambion.AmbionId,
                            (student, ambion) => new Student
                            {
                                Id = student.Id,
                                StudentName = student.StudentName,
                                CurrentAmbionID = ambion.AmbionId,
                                Ambion = new Ambion { AmbionId = ambion.AmbionId, AmbionName = ambion.AmbionName }
                            });
            ViewData["selectlist2"] = new SelectList(await allAmbions.Distinct().ToListAsync(), "AmbionId", "AmbionName");

            IQueryable<Student> kov;
            if (searchGrdId == null && searchGridId == null)
            {
                kov = studs;
            }
            else
            {
                kov = studoner;
            }

            return View(await kov.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Grade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create()
        {
            //select
            IQueryable<Grade> GradesQuery = from m in _context.Grade
                                            orderby m.GradeId
                                            select m;
            IQueryable<Ambion> AmbionsQuery = from m in _context.Ambion
                                              orderby m.AmbionId
                                              select m;
            //linq select
            IQueryable<Grade> grd = _context.Grade.Select(g => new Grade { GradeId = g.GradeId });

            IQueryable<Ambion> grid = _context.Ambion.Select(a => new Ambion { AmbionId = a.AmbionId });

            ////linq select to viewmodel
            //IQueryable<ExampleViewModel> grd1 = _context.Grade.Select(
            //   g => new ExampleViewModel { Name = g.GradeName });

            //IQueryable<ExampleViewModel> grid2 = _context.Ambion.Select(
            //   g => new ExampleViewModel { Name = g.AmbionName });

            ViewData["selectlist"] = new SelectList(await GradesQuery.Distinct().ToListAsync(), "GradeId", "GradeName");

            ViewData["selectlist2"] = new SelectList(await AmbionsQuery.Distinct().ToListAsync(), "AmbionId", "AmbionName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IQueryable<object> GradesQuery = from m in _context.Grade
                                             orderby m.GradeId
                                             select m;
            IQueryable<object> AmbionsQuery = from m in _context.Ambion
                                              orderby m.AmbionId
                                              select m;

            ViewData["selectlist"] = new SelectList(await GradesQuery.Distinct().ToListAsync(), "GradeId", "GradeName");

            ViewData["selectlist2"] = new SelectList(await AmbionsQuery.Distinct().ToListAsync(), "AmbionId", "AmbionName");


            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentName,CurrentGradeID,CurrentAmbionID")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["CurrentGradeID"] = new SelectList(_context.Set<Grade>(), "GradeId", "GradeId", student.CurrentGradeID);
            ViewData["CurrentAmbionID"] = new SelectList(_context.Set<Ambion>(), "AmbionId", "AmbionId", student.CurrentAmbionID);

            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Grade)
                .FirstOrDefaultAsync(m => m.Id == id);
            var studnt = await _context.Student
                .Include(s => s.Ambion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null && studnt == null)
            {
                return NotFound();
            }

            return View(student);
        }



        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
