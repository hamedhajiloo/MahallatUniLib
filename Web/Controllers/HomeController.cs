using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Enums;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Dto;
using Web.Model;
using WebFramework.Api;
using WebFramework.Filters;

namespace Web.Controllers
{
   [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRepository<Field> _fieldRepository;
        private readonly IRepository<BookList> _bookRepository;

        public HomeController(IBookService bookService,IRepository<Field> fieldRepository,IRepository<BookList> bookRepository)
        {
            _bookService = bookService;
            _fieldRepository = fieldRepository;
            _bookRepository = bookRepository;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var pagable = new Pagable
            {
                Desc = true,
                Page = 1,
                PageSize = 10
            };

            var vm = new HomeIndexVM();

            vm.Fields = await _fieldRepository.TableNoTracking.ToListAsync(cancellationToken);
            vm.ComputerBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldBookList.Select(a=>a.FieldId).Contains(1) &&c.BookListIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.UlomBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldBookList.Select(a => a.FieldId).Contains(2) && c.BookListIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.SanayeBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldBookList.Select(a => a.FieldId).Contains(3) && c.BookListIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.MechanickBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldBookList.Select(a => a.FieldId).Contains(4) && c.BookListIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.OmranBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldBookList.Select(a => a.FieldId).Contains(5) && c.BookListIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.Memary = await _bookRepository.TableNoTracking.Where(c => c.FieldBookList.Select(a => a.FieldId).Contains(6) && c.BookListIsDeleted==false).Take(10).ToListAsync(cancellationToken);
           
            return View(vm);
        }
        public async Task<IActionResult> Alaki(CancellationToken cancellationToken,Language lan,CourseType ctype,int field,BookStatus bstatus,Pagable pagable)
        {
            var model = await _bookService.GetAllBookAsync(cancellationToken,ctype,bstatus,lan,field,pagable.Search);
            return Ok(model);
        }

    }
}